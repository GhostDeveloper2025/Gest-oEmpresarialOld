using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using MicroMvvm;
using System;
using System.Collections.Generic;
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
            var objBD = ItemVendaModelObservavel.MapearItemVendaModel(ObjectoEditar.ItemVendaAdicionarPlanilha);
            var result = _itemValidador.Validate(objBD);
            return result.IsValid;
        }

        public async Task ExecutarGuardarItemNaListaAsync(object tag)
        {
            ProdutoProviderItem.ListaExclusoes.Add(ObjectoEditar.ItemVendaAdicionarPlanilha.Produto.IdProduto);
            ObjectoEditar.AdicionarNaLista();

            // Adicione qualquer operação assíncrona aqui ou use Task.CompletedTask para evitar o aviso.
            await Task.CompletedTask;
        }

        public ProdutoProvider ProdutoProviderItem { get; set; }

        public override async Task<int> InserirObjectoBDAsync()
        {
            int idVenda = await base.InserirObjectoBDAsync();
            await AtualizarItensVendaAsync(idVenda);
            return idVenda;
        }

        public override async Task AtualizarObjectoBDAsync()
        {
            await AtualizarItensVendaAsync(ObjectoEditar.IdVenda);
            await base.AtualizarObjectoBDAsync();
        }

        public override async Task ExecutarSalvar(object parameter)
        {
            if (ObjectoEditar.Situacao != true)
            {
                MessageBoxResult result = MessageBox.Show("Quer finalizar a venda já?", "Finalizar", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
    }
}

