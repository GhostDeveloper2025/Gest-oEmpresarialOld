using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class ProdutoModel
    {
        public ProdutoModel(DateTime DataCadastro)
        {
            this.DataCadastro = DataCadastro;
        }

        public ProdutoModel() : this(DateTime.Now)
        {
        }

        public int IdProduto { get; set; }
        public string CodProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int IdMarca { get; set; }
        public decimal ValorCusto { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdCadastrante { get; set; }
        public int IdCategoria { get; set; }

        public EstoqueModel Estoque { get; set; }

        public CategoriaModel Categoria { get; set; } = new CategoriaModel();
        public ColaboradorModel Colaborador { get; set; }
    }
}
