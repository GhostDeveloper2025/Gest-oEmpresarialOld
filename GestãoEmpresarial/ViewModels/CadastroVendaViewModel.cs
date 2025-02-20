using FluentValidation;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using MaterialDesignThemes.Wpf;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroVendaViewModel : CadastroViewModel<VendaModel, EditarVendaModel>
    {
        private readonly RCodigosDAL _codigosDal;
        private readonly RItensVendaDAL _itemVendaDal;
        private readonly ItemVendaValidar _itemValidador;
        public Dictionary<int, string> TiposPagamento { get; internal set; }

        public ICommand AdicionarItemVendaCommand { get; set; }
        public ICommand ApagarItemVendaCommand { get; set; }

        public CadastroVendaViewModel(int? id, VendaValidar validar, IDAL<VendaModel> repositorio, RCodigosDAL codigosDAL, RItensVendaDAL itensVendaDAL, ItemVendaValidar itemVendaValidar)
            : base(id, validar, repositorio)
        {
            AdicionarItemVendaCommand = new RelayCommandWithParameterAsync(ExecutarGuardarItemNaListaAsync, CanExecuteAdicionarItem);
            ApagarItemVendaCommand = new RelayCommandWithParameterAsync(ExecutarApagarItemNaListaAsync, CanExecuteApagarItem);
            ProdutoProviderItem = new ProdutoProvider();

            _itemVendaDal = itensVendaDAL;
            _codigosDal = codigosDAL;
            _itemValidador = itemVendaValidar;

            // Chamada assíncrona ao inicializar
            InitializeAsync(id).ConfigureAwait(false);

        }

        private async Task InitializeAsync(int? id)
        {
            TiposPagamento = (await _codigosDal.GetListaTiposPagamentosAsync()).ToDictionary(b => b.Id, a => a.Nome);

            // Adiciona produtos já existentes na ordem de serviço à lista de exclusão
            foreach (var item in ObjectoEditar.ListItensVenda)
            {
                ProdutoProviderItem.ListaExclusoes.Add(item.Produto.IdProduto);
            }
        }

        public bool CanExecuteApagarItem(object parameter)
        {
            return true;
        }

        public async Task ExecutarApagarItemNaListaAsync(object tag)
        {
            var item = (ItemVendaModelObservavel)tag;
            if (item.IdVenda > 0)
            {
                var objBD = ItemVendaModelObservavel.MapearItemVendaModel(item);
                await _itemVendaDal.DeleteAsync(objBD);  // Alterado para assíncrono
                ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
            }
            ObjectoEditar.RemoverDaLista(item);
        }

        public bool CanExecuteAdicionarItem(object parameter)
        {
            return true;
        }

        public async Task ExecutarGuardarItemNaListaAsync(object tag)
        {
            // Mapeia o modelo de validação
            var objBD = ItemVendaModelObservavel.MapearItemVendaModel(ObjectoEditar.ItemVendaAdicionarPlanilha);

            // Realiza a validação
            var result = _itemValidador.Validate(objBD);

            if (!result.IsValid)
            {
                // Exibe as mensagens de erro
                string erros = string.Join("\n", result.Errors.Select(e => e.ErrorMessage));
                MessageBox.Show($"Não foi possível adicionar o item:\n{erros}", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verifica manualmente o estoque
            var estoqueDisponivel = ObjectoEditar.ItemVendaAdicionarPlanilha.Produto?.Estoque?.Quantidade ?? 0;
            if (ObjectoEditar.ItemVendaAdicionarPlanilha.Quantidade > estoqueDisponivel)
            {
                MessageBox.Show(
                    $"A quantidade solicitada ({ObjectoEditar.ItemVendaAdicionarPlanilha.Quantidade}) excede o estoque disponível ({estoqueDisponivel}).",
                    "Estoque Insuficiente",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Adiciona o item à lista
            ProdutoProviderItem.ListaExclusoes.Add(ObjectoEditar.ItemVendaAdicionarPlanilha.Produto.IdProduto);
            ObjectoEditar.AdicionarNaLista();

            await Task.CompletedTask; // Task dummy para manter o método assíncrono
        }

        public ProdutoProvider ProdutoProviderItem { get; set; }

        public override async Task<int> InserirObjectoBDAsync()
        {
            int idVenda = await base.InserirObjectoBDAsync();
            await AtualizarItensVendaAsync(idVenda);
            ObjectoEditar.IdVenda = idVenda;
            await base.AtualizarObjectoBDAsync();
            return idVenda;
        }

        public override async Task AtualizarObjectoBDAsync()
        {
            await AtualizarItensVendaAsync(ObjectoEditar.IdVenda);
            await base.AtualizarObjectoBDAsync();
        }

        public override async Task ExecutarSalvar(object parameter)
        {
            // Verifique se ObjectoEditar é o tipo EditarVendaModel e se a lista de itens está vazia
            if (ObjectoEditar is EditarVendaModel editarVendaModel && !editarVendaModel.ListItensVenda.Any())
            {
                MessageBox.Show("Não é possível salvar a venda sem produtos adicionados.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verificação de situação da venda
            if (ObjectoEditar.Situacao != true)
            {
                MessageBoxResult result = MessageBox.Show("Quer finalizar a venda?", "Finalizar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ObjectoEditar.Situacao = true;
                }
            }

            await base.ExecutarSalvar(parameter); // Salvar ou atualizar
        }


        private async Task AtualizarItensVendaAsync(int id)
        {
            foreach (var item in ObjectoEditar.ListItensVenda)
            {
                var objBD = ItemVendaModelObservavel.MapearItemVendaModel(item);
                objBD.IdVenda = id;
                if (objBD.IdItensVenda > 0)
                {
                    await _itemVendaDal.UpdateAsync(objBD);  // Alterado para assíncrono
                }
                else
                {
                    await _itemVendaDal.InsertAsync(objBD);  // Alterado para assíncrono
                }
            }
        }

        protected override void ApresentarDialogSucesso(string text)
        {
            new Views.Cadastro.DialogImprimir(this).ShowDialog();
        }
    }
}

