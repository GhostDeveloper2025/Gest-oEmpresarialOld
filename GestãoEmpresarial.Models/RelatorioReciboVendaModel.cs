using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class RelatorioReciboVendaModel
    {
        public VendaModel VendaModel { get; set; }

        public string NomeVendedor { get; set; }
        public string NomePagamento { get; set; }
    }
}
