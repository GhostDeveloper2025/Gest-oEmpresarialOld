using System;
using GestãoEmpresarial.Models.Atributos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class ItemVendaModel 
    {
        public int IdVenda { get; set; }
        public int IdItensVenda { get; set; }
        
        public int Quantidade { get; set; }

        public decimal ValUnitario { get; set; }

        public decimal Desconto { get; set; }

        public decimal CustoTotal { get; set; }
        //public decimal DescontoValor { get { return Desconto <= 0 ? 0 : CustoTotal * Desconto / 100; } }

        //public decimal TotalItem { get { return Math.Round(CustoTotal - DescontoValor, 2); } }

        public ProdutoModel Produto { get; set; }
    }
}
