using FluentValidation;
using NewProject.Model;
using System;
using System.Collections.Generic;

namespace NewProject.Validations
{
    public class ColaboradorValidar : AbstractValidator<ColaboradorModel>
    {
        public ColaboradorValidar()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O Campo `Nome` È Obrigatório. Favor Preencher");
            RuleFor(x => x.Cargo).NotEmpty().WithMessage("O Campo Cargo Esta Em Branco");
            RuleFor(x => x.CPF).NotEmpty().WithMessage("O Campo CPF Esta Em Branco");
            RuleFor(x => x.Telefone).NotEmpty().WithMessage("O Campo Telefone Esta Em Branco");
            RuleFor(x => x.Senha).NotEmpty().WithMessage("O Campo Senha Esta Em Branco");  
        }
    }
}
