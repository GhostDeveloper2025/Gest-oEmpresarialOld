using GestãoEmpresarial.Models.Atributos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class RelatorioComissaoVendasModel
    {
        [DisplayName("Nº Venda")]
        public int NumVenda { get; set; }

        [DisplayName("Data Venda")]
        [DisplayFormatDataBR()]
        public DateTime DataInicial { get; set; }

        [DisplayName("Data Finalização")]
        [DisplayFormatDataBR()]
        public DateTime DataFinalizacao { get; set; }

        public int VendedorId { get; set; }
        public string VendedorCpf { get; set; }
        public string VendedorNome { get; set; }
        public string Situacao { get; set; }
        public decimal TotalVenda { get; set; }
        public decimal PercComissao { get; set; }
        public decimal ValComissao { get; set; }
    }
}
