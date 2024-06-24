using FluentValidation;
using NewProject.Models;
using System;

namespace NewProject.Validations
{
    public class ClienteValidar : AbstractValidator<ClienteModel>
    {
        public ClienteValidar()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O Campo `Nome` È Obrigatório. Favor Preencher");
            //RuleFor(x => x.CPF).NotEmpty().WithMessage("O Campo CPF Esta Em Branco");
            RuleFor(x => x.Cep).NotEmpty().WithMessage("O Campo Cep Esta Em Branco");
            RuleFor(x => x.Celular).NotEmpty().WithMessage("O Campo Celular Esta Em Branco"); 
        }
    }
}
