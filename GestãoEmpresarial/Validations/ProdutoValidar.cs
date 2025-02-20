using FluentValidation;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Validations
{
    public class ProdutoValidar : AbstractValidator<ProdutoModel>
    {
        public ProdutoValidar()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.CodProduto).NotEmpty();
            RuleFor(x => x.IdMarca).NotEmpty();
            RuleFor(x => x.Categoria).NotEmpty();
            //RuleFor(x => x.ValorCusto).NotEmpty();
            RuleFor(x => x.ValorVenda).NotEmpty();
            RuleFor(x => x.Estoque.Localizacao).NotEmpty();
            ////if (LoginViewModel.colaborador.Cargo != "chefe")
            //RuleFor(x => x.Estoque.Quantidade).GreaterThan(0);
        }
    }
}
