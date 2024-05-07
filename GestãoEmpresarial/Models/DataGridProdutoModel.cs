using System.ComponentModel;

namespace GestãoEmpresarial.Models
{
    public class DataGridProdutoModel
    {
        public DataGridProdutoModel(ProdutoModel model)
        {
            IdProduto = model.IdProduto;
            CodProduto = model.CodProduto;
            Nome = model.Nome;
            Descricao = model.Descricao;
            Marca = model.IdMarca.ToString();
            ValorCusto = model.ValorCusto;
            ValorVenda = model.ValorVenda;
            NomeColaborador = model.Colaborador.Nome;
        }

        public int IdProduto { get; set; }
        public string CodProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Marca { get; set; }
        public decimal ValorCusto { get; set; }
        public decimal ValorVenda { get; set; }

        [DisplayName("Cadastrante")]
        public string NomeColaborador { get; set; }
    }
}