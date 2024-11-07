using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GestãoEmpresarial.Models;

namespace GestãoEmpresarial.Validations
{
    public class CategoriaValidar : BaseValidar<CategoriaModel>
    {
        public CategoriaValidar()
        {
            //RuleGenericaVazioFor(x => x.Nome); //Para usar este tenho que abilitar a BaseValidar
            //RuleGenericaVazioFor(x => x.Descricao);

            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.Descricao).NotEmpty();

            //RuleFor(x => x.Descricao).NotEmpty().WithMessage("O Campo Descrição Esta Em Branco");
        }
        
    }
}
