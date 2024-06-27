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
            RuleGenericaVazioFor(x => x.Nome);
            RuleGenericaVazioFor(x => x.Descricao);
            //RuleFor(x => x.Nome).NotEmpty().WithMessage("O campo `Nome` é Obrigatório. Favor Preencher!");
            //RuleFor(x => x.Descricao).NotEmpty().WithMessage("O Campo Descrição Esta Em Branco");
        }
        
    }
}
