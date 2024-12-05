using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using MicroMvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroOrdemServicoViewModel : CadastroViewModel<OrdemServicoModel, EditarOsModel>
    {
        private readonly RCodigosDAL _codigosDal;
        private readonly RItensOSDAL _itensOsDal;
        private readonly ItemOrdemServicoValidar _itemOsvalidador;
        private readonly ROsDAL _ordemServicoRepositorio;

        public CadastroOrdemServicoViewModel(int? id, OrdemServicoValidar validar, ROsDAL repositorio, ItemOrdemServicoValidar itemOsvalidador, RCodigosDAL codigosDAL, RItensOSDAL itensOSDAL)
           : base(id, validar, repositorio)
        {
            AdicionarItemOsCommand = new RelayCommandWithParameterAsync(ExecutarGuardarItemNaListaAsync, CanExecuteAdicionarItem);
            ApagarItemOsCommand = new RelayCommandWithParameterAsync(ExecutarApagarItemNaListaAsync, CanExecuteApagarItem);
            ProdutoProviderItem = new ProdutoProvider();

            _itemOsvalidador = itemOsvalidador;
            _codigosDal = codigosDAL;
            _itensOsDal = itensOSDAL;
            _ordemServicoRepositorio = repositorio;  // Armazene o valor do repositório

            // Chamada assíncrona ao inicializar
            InitializeAsync(id).ConfigureAwait(false);
        }

        private async Task InitializeAsync(int? id)
        {
            // Carrega a lista de marcas de ferramentas
            MarcasList = (await _codigosDal.GetListaMarcasFerramentaAsync()).ToDictionary(b => b.Id, a => a.Nome);

            // Define se é uma nova ordem de serviço ou uma já existente
            NovaOrdemServico = ObjectoEditar.Status == 0;

            if (NovaOrdemServico)
            {
                // Para uma nova ordem de serviço, define o status inicial
                var statusInicial = await _codigosDal.GetStatusAbertaAsync();
                ObjectoEditar.Status = statusInicial.Id;
                StatusList = new Dictionary<int, string>() { { statusInicial.Id, statusInicial.Nome } };
            }
            else
            {
                // Para uma ordem de serviço existente, verifica se pode editar e carrega os status seguintes
                PodeEditar = await _ordemServicoRepositorio.PodeEditarAsync(ObjectoEditar.Status);
                StatusList = (await _codigosDal.ListaStatusSeguintesAsync(ObjectoEditar.Status)).ToDictionary(b => b.Id, a => a.Nome);

                // Adiciona produtos já existentes na ordem de serviço à lista de exclusão
                foreach (var item in ObjectoEditar.ListItensOs)
                {
                    ProdutoProviderItem.ListaExclusoes.Add(item.Produto.IdProduto);
                }
            }
        }

        public ProdutoProvider ProdutoProviderItem { get; set; }

        public ICommand AdicionarItemOsCommand { get; set; }

        public ICommand ApagarItemOsCommand { get; set; }

        public Dictionary<int, string> MarcasList { get; internal set; }

        public Dictionary<int, string> StatusList { get; internal set; }

        public bool PodeEditar { get; internal set; }

        public bool NovaOrdemServico { get; internal set; }

        private async Task AtualizarItensOsAsync(int idOs)
        {
            foreach (var item in ObjectoEditar.ListItensOs)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                objBD.IdOs = idOs;
                if (objBD.IdItensOs > 0)
                {
                    await _itensOsDal.UpdateAsync(objBD);
                }
                else
                {
                    await _itensOsDal.InsertAsync(objBD);
                }
            }
        }

        public override async Task AtualizarObjectoBDAsync()
        {
            await AtualizarItensOsAsync(ObjectoEditar.IdOs);
            await base.AtualizarObjectoBDAsync();
        }

        public override EditarOsModel NovoObjectoEditar()
        {
            var obj = base.NovoObjectoEditar();
            if (obj.ListItensOs != null && !Id.HasValue)
                obj.ListItensOs.Clear();
            return obj;
        }

        public override async Task<int> InserirObjectoBDAsync()
        {
            int idOs = await base.InserirObjectoBDAsync();
            await AtualizarItensOsAsync(idOs);
            return idOs;
        }

        public bool CanExecuteApagarItem(object parameter)
        {
            return _codigosDal.PodeApagarItemAsync(ObjectoEditar.Status).GetAwaiter().GetResult(); // Chamada síncrona
        }

        public async Task ExecutarApagarItemNaListaAsync(object tag)
        {
            var item = (ItensOrdemServicoModelObservavel)tag;
            if (item.IdItensOs > 0)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                await _itensOsDal.DeleteAsync(objBD);
            }
            ObjectoEditar.RemoverDaLista(item);
            ProdutoProviderItem.ListaExclusoes.Remove(item.Produto.IdProduto);
        }

        public bool CanExecuteAdicionarItem(object parameter)
        {
            var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(ObjectoEditar.ItemOsAdicionarPlanilha);
            var result = _itemOsvalidador.Validate(objBD);
            return result.IsValid;
        }

        public async Task ExecutarGuardarItemNaListaAsync(object tag)
        {
            ProdutoProviderItem.ListaExclusoes.Add(ObjectoEditar.ItemOsAdicionarPlanilha.Produto.IdProduto);
            ObjectoEditar.AdicionarNaLista();

            // Adicione qualquer operação assíncrona aqui ou use Task.CompletedTask para evitar o aviso.
            await Task.CompletedTask;
        }
    }
}

