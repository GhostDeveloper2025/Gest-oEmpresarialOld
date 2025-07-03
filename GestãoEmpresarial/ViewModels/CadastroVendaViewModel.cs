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
        public ProdutoProvider ProdutoProviderItem { get; set; }

        public ICommand CancelarVendaCommand { get; set; }

        public CadastroVendaViewModel(int? id, VendaValidar validar, IDAL<VendaModel> repositorio, RCodigosDAL codigosDAL, RItensVendaDAL itensVendaDAL, ItemVendaValidar itemVendaValidar)
    : base(id, validar, repositorio)
        {
            AdicionarItemVendaCommand = new RelayCommandWithParameterAsync(ExecutarGuardarItemNaListaAsync, CanExecuteAdicionarItem);

            ApagarItemVendaCommand = new RelayCommandWithParameterAsync(ExecutarApagarItemGrideAsync, CanExecuteAdicionarItemGrid);

            CancelarVendaCommand = new RelayCommandWithParameterAsync(ExecutarCancelarVendaAsync, CanExecuteCancelarVenda); // Novo comando
            ProdutoProviderItem = new ProdutoProvider();

            _itemVendaDal = itensVendaDAL;
            _codigosDal = codigosDAL;
            _itemValidador = itemVendaValidar;

            InitializeAsync(id).ConfigureAwait(false);

            if (ObjectoEditar.Situacao)
            {
                if (LoginViewModel.Instancia.colaborador.Gerente == false)
                    ApenasVisualizar = true;
            }
        }
        public bool PodeCancelar => LoginViewModel.Instancia.colaborador.Cargo.ToUpper() == "GERENTE";

        public async Task ExecutarApagarItemGrideAsync(object tag)
        {
            var item = (ItemVendaModelObservavel)tag;
            if (item.IdVenda > 0)
            {
                var objBD = ItemVendaModelObservavel.MapearItemVendaModel(item);
                await _itemVendaDal.DeleteDelItemAsync(objBD);  // Alterado para assíncrono
                
            }
            ObjectoEditar.RemoverDaLista(item);
            ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
        }

        public bool CanExecuteAdicionarItemGrid(object parameter)
        {
            return true;
        }
        
        private bool CanExecuteCancelarVenda(object parameter)
        {
            return ObjectoEditar != null && ObjectoEditar.IdVenda > 0 && !ObjectoEditar.Cancelada;
        }

        private async Task ExecutarCancelarVendaAsync(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Tem certeza que deseja cancelar esta venda?", "Cancelar Venda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Chama o método para cancelar a venda
                await _repositorio.DeleteAsync(ObjectoEditar.DevolveObjectoBD());

                // Atualiza o status da venda no ViewModel
                ObjectoEditar.Cancelada = true;

                MessageBox.Show("Venda cancelada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Id = null; // obrigamos a limpar
            PodeInserir = true; // Garante que a próxima operação será de inserção
            ObjectoEditar = NovoObjectoEditar();
            RaisePropertyChanged(nameof(ObjectoEditar));
        }

        //
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
            // ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
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
        // Codigo ExecutarSalvar
        public override async Task ExecutarSalvar(object parameter)
        {
            // Verifique se ObjectoEditar é o tipo EditarVendaModel e se a lista de itens está vazia
            if (ObjectoEditar is EditarVendaModel editarVendaModel && !editarVendaModel.ListItensVenda.Any())
            {
                MessageBox.Show("Não é possível salvar a venda sem produtos adicionados.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verificação de situação da venda
            //if (ObjectoEditar.Situacao != true)
            //{
            //    MessageBoxResult result = MessageBox.Show("Quer finalizar a venda?", "Finalizar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        ObjectoEditar.Situacao = true;
            //    }
            //}


            if (ObjectoEditar.Situacao != true)
            {
                MessageBoxResult result = MessageBox.Show("Quer finalizar a venda?", "Finalizar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Verifica o estoque de cada item
                    var itensComEstoqueInsuficiente = ObjectoEditar.ListItensVenda
                        .Where(item => item.Quantidade > (item.Produto?.Estoque?.Quantidade ?? 0))
                        .ToList();

                    if (itensComEstoqueInsuficiente.Any())
                    {
                        string mensagemErro = "Não foi possível finalizar a venda. Os seguintes produtos estão com estoque insuficiente:\n\n";
                        foreach (var item in itensComEstoqueInsuficiente)
                        {
                            mensagemErro += $"- {item.Produto.Nome} (Disponível: {item.Produto.Estoque?.Quantidade ?? 0}, Solicitado: {item.Quantidade})\n";
                        }

                        MessageBox.Show(mensagemErro, "Erro ao Finalizar", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Impede a finalização
                    }

                    ObjectoEditar.Situacao = true; // Só define como finalizada se tudo estiver OK
                }
            }


            await base.ExecutarSalvar(parameter); // Salvar ou atualizar
            ProdutoProviderItem.ListaExclusoes.Clear();
        }


        // Substitua o seu método ExecutarSalvar existente por este
        #region
        //public override async Task ExecutarSalvar(object parameter)
        //{
        //    // 1. Verificação inicial para garantir que a venda não está vazia
        //    if (ObjectoEditar is EditarVendaModel editarVendaModel && !editarVendaModel.ListItensVenda.Any())
        //    {
        //        MessageBox.Show("Não é possível salvar a venda sem produtos adicionados.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    // Flag para saber se estamos tentando finalizar a venda nesta execução
        //    bool tentandoFinalizar = false;

        //    // 2. Verifica se a venda ainda não foi finalizada e pergunta ao usuário
        //    if (ObjectoEditar.Situacao != true)
        //    {
        //        MessageBoxResult result = MessageBox.Show("Quer finalizar a venda?", "Finalizar", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            // O usuário quer finalizar. Marcamos isso.
        //            tentandoFinalizar = true;
        //        }
        //    }

        //    // 3. NOVA LÓGICA: Se o usuário decidiu finalizar, verificamos o estoque ANTES de salvar
        //    if (tentandoFinalizar)
        //    {
        //        // Lista para guardar os nomes dos produtos com estoque insuficiente
        //        var produtosComEstoqueInsuficiente = new List<string>();

        //        // Percorre cada item na lista de venda
        //        foreach (var item in ObjectoEditar.ListItensVenda)
        //        {
        //            // Compara a quantidade na venda com o estoque atual do produto
        //            // Assumindo que 'item.Produto.Estoque.Quantidade' tem o valor mais recente.
        //            if (item.Quantidade > item.Produto.Estoque.Quantidade)
        //            {
        //                // Adiciona o produto e a informação de estoque na lista de erros
        //                produtosComEstoqueInsuficiente.Add(
        //                    $"- {item.Produto.Nome} (Pedido: {item.Quantidade}, Estoque: {item.Produto.Estoque.Quantidade})"
        //                );
        //            }
        //        }

        //        // 4. Se a lista de erros não estiver vazia, exibe a mensagem e para a execução
        //        if (produtosComEstoqueInsuficiente.Any())
        //        {
        //            // Monta a mensagem de erro final
        //            string mensagemErro = "A venda não pôde ser finalizada por falta de estoque:\n\n" +
        //                                  string.Join("\n", produtosComEstoqueInsuficiente) +
        //                                  "\n\nAjuste os itens ou o estoque. A venda continuará salva como um orçamento.";

        //            MessageBox.Show(mensagemErro, "Estoque Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);

        //            // IMPORTANTE: Não prossegue com a finalização. A venda continua como orçamento.
        //            return;
        //        }

        //        // Se o estoque estiver OK, definimos a situação como finalizada
        //        ObjectoEditar.Situacao = true;
        //    }

        //    // 5. Se chegamos até aqui, ou o usuário não quis finalizar, ou o estoque estava OK.
        //    // Então, podemos prosseguir com o salvamento padrão.
        //    await base.ExecutarSalvar(parameter);
        //    ProdutoProviderItem.ListaExclusoes.Clear();
        //}
        #endregion
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

