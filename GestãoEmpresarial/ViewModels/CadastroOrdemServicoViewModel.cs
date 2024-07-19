using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using MicroMvvm;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public CadastroOrdemServicoViewModel(int? id, OrdemServicoValidar validar, ROsDAL repositorio, ItemOrdemServicoValidar itemOsvalidador, RCodigosDAL codigosDAL, RItensOSDAL itensOSDAL) 
            : base(id, validar, repositorio)
        {
            AdicionarItemOsCommand = new RelayCommandWithParameter(ExecutarGuardarItemNaLista, CanExecuteAdicionarItem);
            ApagarItemOsCommand = new RelayCommandWithParameter(ExecutarApagarItemNaLista, CanExecuteApagarItem);
            ProdutoProviderItem = new ProdutoProvider();

            _itemOsvalidador = itemOsvalidador;
            _codigosDal = codigosDAL;
            _itensOsDal = itensOSDAL;

            if (id.HasValue)
            {
                _itensOsDal.GetByIdOs(id.Value);
            }

            MarcasList = _codigosDal.GetListaMarcasFerramenta().ToDictionary(b => b.Id, a => a.Nome);
            NovaOrdemServico = ObjectoEditar.Status == 0; //se estiver diferente de 0 é pq já tem um status associado

            if (NovaOrdemServico)
            {
                var statusInicial = _codigosDal.GetStatusAberta();
                ObjectoEditar.Status = statusInicial.Id;
                StatusList = new Dictionary<int, string>() { { statusInicial.Id, statusInicial.Nome } };
            }
            else
            {
                PodeEditar = repositorio.PodeEditar(ObjectoEditar.Status);
                StatusList = _codigosDal.ListaStatusSeguintes(ObjectoEditar.Status).ToDictionary(b => b.Id, a => a.Nome);
            }
        }

        //public List<string> StatusList => ValoresEstaticos.Status.ToList();
        public ProdutoProvider ProdutoProviderItem { get; set; }
        public ICommand AdicionarItemOsCommand { get; set; }
        public ICommand ApagarItemOsCommand { get; set; }

        // Propriedades para obter listas de marcas e status da classe ValoresEstaticos,
        // convertidas em List<string> para facilitar a ligação a ComboBoxes.
        public Dictionary<int, string> MarcasList { get; internal set; }
        public Dictionary<int, string> StatusList { get; internal set; }

        public bool PodeEditar { get; internal set; }
        public bool NovaOrdemServico { get; internal set; }


        private void AtualizarItensOs(int idOs)
        {
            foreach (var item in ObjectoEditar.ListItensOs)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                objBD.IdOs = idOs;
                if (objBD.IdItensOs > 0)
                {
                    _itensOsDal.Update(objBD);
                }
                else
                {
                    _itensOsDal.Insert(objBD);
                }
            }
        }

        public override void AtualizarObjectoBD()
        {
            AtualizarItensOs(ObjectoEditar.IdOs);
            base.AtualizarObjectoBD();
        }

        public override EditarOsModel NovoObjectoEditar()
        {
            var obj = base.NovoObjectoEditar();
            if (obj.ListItensOs != null)
                obj.ListItensOs.Clear();
            return obj;
        }

        public override int InserirObjectoBD()
        {
            int idOs = base.InserirObjectoBD();
            AtualizarItensOs(idOs);
            return idOs;
        }

        public bool CanExecuteApagarItem(object parameter)
        {
            return _codigosDal.PodeApagarItem(ObjectoEditar.Status);
        }

        public void ExecutarApagarItemNaLista(object tag)
        {
            var item = (ItensOrdemServicoModelObservavel)tag;
            if (item.IdItensOs > 0)
            {
                var objBD = ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(item);
                _itensOsDal.Delete(objBD);
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

        public void ExecutarGuardarItemNaLista(object tag)
        {
            ProdutoProviderItem.ListaExclusoes.Add(ObjectoEditar.ItemOsAdicionarPlanilha.Produto.IdProduto);
            ObjectoEditar.AdicionarNaLista();
        }

        public IEnumerable GetSuggestions(string filter)
        {
            var repo = new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario);
            var list = repo.List(filter).Take(50); // Limita a 50 registros
            List<ProdutoModel> listaA = new List<ProdutoModel>();
            foreach (var item in list)
            {
                if (ObjectoEditar.ListItensOs.Any(a => Equals(a.Produto.IdProduto, item.IdProduto)))
                    continue;
                else
                {
                    listaA.Add(item);
                }
            }
            return listaA;
        }
    }
    
}
