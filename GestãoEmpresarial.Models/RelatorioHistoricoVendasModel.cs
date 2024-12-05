using GestãoEmpresarial.Models.Atributos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class RelatorioHistoricoVendasModel
    {
        [DisplayName("Nº Venda")]
        public int NumVenda { get; set; }

        [DisplayName("Data Emissão")]
        [DisplayFormatDataBR()]
        public DateTime DataEmissao { get; set; }
        public DateTime DataHoraEmissao { get; set; }
        public DateTime DataFinalizacao { get; set; }
        public int ClienteId { get; set; }
        public string ClienteCpf { get; set; }
        public string ClienteNome { get; set; }
        public int VendedorId { get; set; }
        public string VendedorCpf { get; set; }
        public string VendedorNome { get; set; }
        public int Situacao { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalFrete { get; set; }
        public decimal TotalVenda { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}
