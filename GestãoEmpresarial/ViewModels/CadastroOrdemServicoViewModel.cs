using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
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
    internal class CadastroOrdemServicoViewModel : CadastroViewModel<OsModel, EditarOsModel>
    {
        private readonly RCodigosDAL codigosDal;
        private readonly RItensOSDAL itensOsDal;
        //private readonly ItemOsValidar itemOsvalidador;

        public CadastroOrdemServicoViewModel(int? id, IDAL<OsModel> Repositorio) : base(id, Repositorio)
        {
        }

        // Inicializa a instância de OsViewModel com os parâmetros:
        //public CadastroOrdemServicoViewModel(int? id) : base(id, new OsValidar(), new ROsDAL(LoginViewModel.colaborador.IdFuncionario))
        //{
        //    itemOsvalidador = new ItemOsValidar();
        //    AdicionarItemOsCommand = new RelayCommandWithParameter(ExecutarGuardarItemOsNaLista, CanExecuteAdicionarItem);
        //    ApagarItemOsCommand = new RelayCommandWithParameter(ExecutarApagarItemOsNaLista, CanExecuteApagarItem);

        //    codigosDal = new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario);
        //    itensOsDal = new RItensOSDAL(LoginViewModel.colaborador.IdFuncionario);

        //    if (id.HasValue)
        //    {
        //        itensOsDal.GetByIdOs(id.Value);
        //    }

        //    MarcasList = codigosDal.GetListaMarcasFerramenta().ToDictionary(b => b.Id, a => a.Nome);
        //    NovaOrdemServico = ObjectoEditar.Status == 0; //se estiver diferente de 0 é pq já tem um status associado

        //    if (NovaOrdemServico)
        //    {
        //        var statusInicial = codigosDal.GetStatusAberta();
        //        ObjectoEditar.Status = statusInicial.Id;
        //        StatusList = new Dictionary<int, string>() { { statusInicial.Id, statusInicial.Nome } };
        //    }
        //    else
        //    {
        //        PodeEditar = ((ROsDAL)Repositorio).PodeEditar(ObjectoEditar.Status);
        //        StatusList = codigosDal.ListaStatusSeguintes(ObjectoEditar.Status).ToDictionary(b => b.Id, a => a.Nome);
        //    }
        //}

        //public List<string> StatusList => ValoresEstaticos.Status.ToList();
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
                item.IdOs = idOs;
                if (item.IdItensOs > 0)
                {
                    itensOsDal.Update(item);
                }
                else
                {
                    itensOsDal.Insert(item);
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
            return codigosDal.PodeApagarItem(ObjectoEditar.Status);
        }

        public void ExecutarApagarItemOsNaLista(object tag)
        {
            var item = (ItemOrdemServicoModel)tag;
            if (item.IdItensOs > 0)
            {
                itensOsDal.Delete(item);
            }
            ObjectoEditar.ListItensOs.Remove(item);
            AtualizarTotaisItens();
        }

        //public bool CanExecuteAdicionarItem(object parameter)
        //{
        //    var result = itemOsvalidador.Validate(ObjectoEditar.ItemOsAdicionar);
        //    return result.IsValid;
        //}

        public void ExecutarGuardarItemOsNaLista(object tag)
        {
            ObjectoEditar.AdicionarNaLista();
            AtualizarTotaisItens();
        }

        private void AtualizarTotaisItens()
        {
            //RaisePropertyChanged(nameof(ObjectoEditar.TotalDescontoProduto));
            ////Aqui estamos a dizer, que no "ObjectoEditar" a propriedade SubtotalProduto foi alterada
            ////E como tal o ecrã deve atualizar o seu valor
            //RaisePropertyChanged(nameof(ObjectoEditar.SubTotalProduto));
            //RaisePropertyChanged(nameof(ObjectoEditar.TotalProduto));
            //RaisePropertyChanged(nameof(ObjectoEditar.TotalOS));
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
