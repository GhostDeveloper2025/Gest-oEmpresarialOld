using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class EstoqueModel
    {
        public int IdEstoque { get; set; }
        public string Localizacao { get; set; }
        public int Quantidade { get; set; }

        public int IdProduto { get; set; }
    }
}
