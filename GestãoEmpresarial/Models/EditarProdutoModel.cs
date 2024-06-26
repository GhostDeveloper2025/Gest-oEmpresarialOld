using GestãoEmpresarial.Validations;
using System;

namespace GestãoEmpresarial.Models
{
    public class EditarProdutoModel : EditarBaseModel<ProdutoModel>
    {
        public EditarProdutoModel(ProdutoValidar validar) : this(null, validar)
        {
            DataCadastro = DateTime.Now;
        }

        public EditarProdutoModel(ProdutoModel obj, ProdutoValidar validar) : base(obj, validar)
        {
        }

        public override ProdutoModel DevolveObjectoBD()
        {
            return new ProdutoModel()
            {
                IdProduto = IdProduto,
                IdCategoria = IdCategoria,
                Nome = Nome,
                CodProduto = CodProduto,
                ValorCusto = ValorCusto,
                ValorVenda = ValorVenda,
                IdMarca = IdMarca,
                Descricao = Descricao,
                DataCadastro = DataCadastro,

                Estoque = new EstoqueModel()
                {
                    IdEstoque = IdEstoque,
                    Quantidade = EstoqueQuantidade,
                    Localizacao = EstoqueLocalizacao
                }
            };
        }

        protected override void SetPropriedadesDoObjectoBD(ProdutoModel obj)
        {
            IdProduto = obj.IdProduto;
            CodProduto = obj.CodProduto;
            Nome = obj.Nome;
            IdMarca = obj.IdMarca;
            IdCategoria = obj.IdCategoria;
            ValorCusto = obj.ValorCusto;
            ValorVenda = obj.ValorVenda;
            Descricao = obj.Descricao;
            DataCadastro = obj.DataCadastro;

            if (obj.Estoque != null)
            {
                IdEstoque = obj.Estoque.IdEstoque;
                EstoqueQuantidade = obj.Estoque.Quantidade;
                EstoqueLocalizacao = obj.Estoque.Localizacao;
            }
        }

        public int IdProduto { get; set; }

        public string CodProduto { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal ValorCusto { get; set; }
        public decimal ValorVenda { get; set; }

        public int IdEstoque { get; set; }
        public int EstoqueQuantidade { get; set; }
        public string EstoqueLocalizacao { get; set; }

        public DateTime DataCadastro { get; private set; }
    }
}