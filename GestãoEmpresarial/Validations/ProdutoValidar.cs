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
            //RuleFor(x => x.Categoria).NotEmpty().WithMessage("O Campo Categoria Esta Em Branco");
            //RuleFor(x => x.IdMarca).NotEmpty().WithMessage("O Campo Marca Esta Em Branco");
            //RuleFor(x => x.Nome).NotEmpty().WithMessage("O Campo `Nome` È Obrigatório. Favor Preencher");
            //RuleFor(x => x.CodProduto).NotEmpty().WithMessage("O Campo Codigo Produto Esta Em Branco");
            //RuleFor(x => x.ValorCusto).NotEmpty().WithMessage("O Campo Valor Custo Esta Em Branco");
            //RuleFor(x => x.ValorVenda).NotEmpty().WithMessage("O Campo Valor Venda Esta Em Branco");
            
            //RuleFor(x => x.Estoque.Localizacao).NotEmpty();
            ////if (LoginViewModel.colaborador.Cargo != "chefe")
            //RuleFor(x => x.Estoque.Quantidade).GreaterThan(0);
        }
    }
}
