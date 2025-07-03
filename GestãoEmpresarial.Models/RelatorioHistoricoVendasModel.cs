using GestãoEmpresarial.Models.Atributos;
using System;
using System.ComponentModel;

namespace GestãoEmpresarial.Models
{
    public class RelatorioHistoricoVendasModel
    {
        [DisplayName("Nº Venda")]
        public int NumVenda { get; set; }

        [DisplayName("Cliente")]
        public string ClienteNome { get; set; }

        [DisplayName("Vendedor")]
        public string VendedorNome { get; set; }

        [DisplayName("Quantidade Produto")]
        public int QuantidadeTotal { get; set; }

        [DisplayName("Total Venda")]
        public decimal TotalLiquido => (TotalProdutos - TotalDesconto) + TotalFrete;

        //[DisplayName("Total Venda")]
        public decimal TotalVenda { get; set; }

        [DisplayName("Data Emissão")]
        [DisplayFormatDataBR()]
        public DateTime DataEmissao { get; set; }
        public DateTime DataHoraEmissao { get; set; }

        [DisplayName("Data Finalização")]
        [DisplayFormatDataBR()]
        public DateTime DataFinalizacao { get; set; }
        public int ClienteId { get; set; }
        public string ClienteCpf { get; set; }

        public int VendedorId { get; set; }
        public string VendedorCpf { get; set; }

        public int Situacao { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalFrete { get; set; }


    }
}
