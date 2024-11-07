using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioReciboOrdemServicoViewModel : ObservableObject
    {
        public RelatorioReciboOrdemServicoViewModel(RRelatorioReciboDAL relatorioReciboDAL, int idOs /*, ROsDAL ROsDAL, int id*/)
        {
            //_relatorioReciboDAL = relatorioReciboDAL;

            //1. obter objecto da BD e colocar no propriedade
            //2. a propriedade faz bind na view
            //3. a impressao acontece

            Relatorio = relatorioReciboDAL.ObterReciboOrdemServico(idOs);
        }

        public RelatorioReciboOsModel Relatorio { get; set; }

        public ObservableCollection<ItensOrdemServicoModelObservavel> ListItemsObservaveis
        {
            get
            {
                var listDeObservaveis = Relatorio.OsModel.ListItensOs.Select(a => ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(a));
                return new ObservableCollection<ItensOrdemServicoModelObservavel>(listDeObservaveis);
            }
        }
    }
}
