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
        [DisplayName("Codigo Produo")]
        public string CodProduto { get; set; }
        [DisplayName("Nome Produto")]
        public string Nome { get; set; }
        [DisplayName("Descrição Produto")]
        public string Descricao { get; set; }
        public string Marca { get; set; }
        [DisplayName("Valor Custo")]
        public decimal ValorCusto { get; set; }
        [DisplayName("Valor Venda")]
        public decimal ValorVenda { get; set; }

        [DisplayName("Cadastrante")]
        public string NomeColaborador { get; set; }
    }
}