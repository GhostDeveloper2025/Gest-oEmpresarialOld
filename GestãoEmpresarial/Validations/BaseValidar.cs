using FluentValidation;
using GestãoEmpresarial.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GestãoEmpresarial.Validations
{
    public class BaseValidar<T> : AbstractValidator<T>
    {
        //public IRuleBuilderOptions<T, TProperty> RuleGenericaVazioFor<TProperty>(Expression<Func<T, TProperty>> expression)
        //{
        //    return base.RuleFor<TProperty>(expression).NotEmpty().WithMessage("O Campo é obrigatório!");
        //}
    }
}
