using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioReciboVendaViewModel : ObservableObject
    {
        private readonly RRelatorioReciboDAL _relatorioReciboDAL;

        public RelatorioReciboVendaViewModel(RRelatorioReciboDAL relatorioReciboDAL)
        {
            _relatorioReciboDAL = relatorioReciboDAL;
        }
    }
}
