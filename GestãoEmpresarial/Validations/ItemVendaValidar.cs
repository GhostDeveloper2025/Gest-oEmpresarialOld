using FluentValidation;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Validations
{
    internal class ItemVendaValidar : AbstractValidator<ItemVendaModel>
    {
        public ItemVendaValidar()
        {
            // Regra para garantir que a quantidade não está vazia
            RuleFor(x => x.Quantidade)
                .NotEmpty();
            //.WithMessage("A quantidade é obrigatória.");

            // Regra para garantir que o valor unitário é maior que zero
            RuleFor(x => x.ValUnitario)
                .GreaterThan(0);
            //.WithMessage("O valor unitário deve ser maior que zero.");

            // Regra para garantir que o produto está selecionado
            RuleFor(x => x.Produto)
                .NotEmpty();
            //.WithMessage("O produto é obrigatório.");

            // Regra personalizada para validar o estoque
            RuleFor(x => x)
                .Custom((item, context) =>
                {
                    if (item.Produto != null && item.Produto.Estoque != null)
                    {
                        var estoqueDisponivel = item.Produto.Estoque.Quantidade;

                        if (item.Quantidade > estoqueDisponivel)
                        {
                            context.AddFailure("Quantidade",
                                $"A quantidade solicitada ({item.Quantidade}) não pode exceder o estoque disponível ({estoqueDisponivel}).");
                        }
                    }
                });
        }
    }
}
