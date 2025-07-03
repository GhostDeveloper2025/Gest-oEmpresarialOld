using System;
using System.ComponentModel;
using GestãoEmpresarial.Models.Atributos;

namespace GestãoEmpresarial.Models
{
    public class RelatorioHistoricoOsModel
    {
        [DisplayName("Nº Os")]
        public int NumOs { get; set; }
        [DisplayName("Data Entrada")]
        [DisplayFormatDataBR()]
        public DateTime DataEntrada { get; set; }
        [DisplayName("Data Finalização")]
        [DisplayFormatDataBR()]
        public DateTime? DataFinalizacao { get; set; }

        public int ClienteId { get; set; }
        public string ClienteCpf { get; set; }
        [DisplayName("Cliente")]
        public string ClienteNome { get; set; }

        public int? TecnicoId { get; set; }
        public string TecnicoCpf { get; set; }
        [DisplayName("Tecnico")]
        public string TecnicoNome { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }

        public decimal TotalProdutos { get; set; }
        public decimal TotalDesconto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalMaoObra { get; set; }
        public decimal TotalOs { get; set; }
        [DisplayName("Total Os")]
        public decimal TotalOsCorrigido => TotalOs - TotalDesconto;

        public int QuantidadeTotal { get; set; }
    }
}
