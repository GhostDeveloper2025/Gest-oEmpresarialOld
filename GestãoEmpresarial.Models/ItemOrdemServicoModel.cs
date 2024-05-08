using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class ItemOrdemServicoModel 
    {
        public int IdOs { get; set; }
        public int IdItensOs { get; set; }
        
        public int Quantidade { get; set; }
        public decimal ValUnitario { get; set; }
                
        public decimal Desconto { get; set; }
        public decimal CustoTotal { get; set; }

        public ProdutoModel Produto { get; set; }
    }
}
