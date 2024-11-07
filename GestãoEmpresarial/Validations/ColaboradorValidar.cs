using FluentValidation;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;

namespace GestãoEmpresarial.Validations
{
    public class ColaboradorValidar : AbstractValidator<ColaboradorModel>
    {
        public ColaboradorValidar()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.Cargo).NotEmpty();
            RuleFor(x => x.Email).NotEmpty(); 
            RuleFor(x => x.Comissao).NotEmpty(); 
            RuleFor(x => x.Senha).NotEmpty(); 
        }
    }
}
