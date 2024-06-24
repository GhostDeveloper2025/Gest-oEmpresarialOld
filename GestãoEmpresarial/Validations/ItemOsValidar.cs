using FluentValidation;
using NewProject.Models;
using NewProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject.Validations
{
    public class ItemOsValidar : AbstractValidator<ItensOsModel>
    {
        public ItemOsValidar()
        {
            //Func<ItensOsModel, bool> temEstoque = a => a.Produto != null && a.Produto.Estoque != null;

            //RuleFor(x => x.Produto.Estoque.Quantidade).GreaterThan(0).When(temEstoque);
            //RuleFor(x => x.Quantidade).GreaterThan(0).LessThanOrEqualTo(x => x.Produto.Estoque.Quantidade).When(temEstoque);
            RuleFor(x => x.Quantidade).NotEmpty();
            RuleFor(x => x.ValUnitario).GreaterThan(0);
            RuleFor(x => x.Produto).NotEmpty();

        }
    }
}
