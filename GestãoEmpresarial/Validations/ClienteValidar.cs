using FluentValidation;
using GestãoEmpresarial.Models;
using System;

namespace GestãoEmpresarial.Validations
{
    public class ClienteValidar : AbstractValidator<ClienteModel>
    {
        public ClienteValidar()
        {
            RuleFor(x => x.Nome).NotEmpty();
            RuleFor(x => x.CPF).NotEmpty();
            RuleFor(x => x.CNPJ).NotEmpty();
            RuleFor(x => x.Celular).NotEmpty();
            RuleFor(x => x.Telefone).NotEmpty();
            RuleFor(x => x.LimiteCredito).NotEmpty();
        }
    }
}
