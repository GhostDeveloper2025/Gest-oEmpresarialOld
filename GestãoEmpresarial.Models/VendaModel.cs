using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    /// <summary>
    /// Este object apenas server para passar e enviar informação da Base de dados para o ViewModel ou vice versa.
    /// </summary>
    public class VendaModel
    {
        //cada vez que é chamado o new, este construtor é chamado
        public VendaModel()
        {
            ListItensVenda = new List<ItemVendaModel>();
        }

        public IList<ItemVendaModel> ListItensVenda { get; set; }

        // public ColaboradorModel Cadastrante { get; set; }

        public int IdVenda { get; set; }
        public bool Situacao { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public ColaboradorModel Cadastrante { get; set; }

        public decimal ValorFrete { get; set; }
        public int IdCodigoTipoPagamento { get; set; }
        public int IdFuncionario { get; set; }

        public ClienteModel Cliente { get; set; }
        public int IdCliente { get; set; }
        public int IdCadastrante { get; set; }
    }
}
