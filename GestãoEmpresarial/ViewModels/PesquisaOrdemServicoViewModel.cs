using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaOrdemServicoViewModel : PesquisaViewModel<OsModel, DataGridOrdemServicoModel>
    {
        public Dictionary<int, string> StatusList { get; internal set; }

        public PesquisaOrdemServicoViewModel(IDAL<OsModel> Repositorio, RCodigosDAL RepositorioCodigos) : base(Repositorio)
        {
            StatusList = new Dictionary<int, string>
            {
                { 0, null }
            };
            foreach (var codigo in RepositorioCodigos.GetListaStatus())
            {
                StatusList.Add(codigo.Id, codigo.Nome);
            }
        }

        public override DataGridOrdemServicoModel GetDataGridModel(OsModel item)
        {
            return new DataGridOrdemServicoModel(item, StatusList);
        }
    }
}
