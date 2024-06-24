using FluentValidation;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Validations
{
    public class VendaValidar : AbstractValidator<VendaModel>
    {
        public VendaValidar()
        {
            RCodigosDAL codigos = new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario);
            var prontoParaConsertoId = codigos.GetStatusProntoParaConserto().Id;
            RuleFor(x => x.Cliente.Idcliente).NotEmpty().When(a => a.Cliente != null);
            RuleFor(x => x.Cliente).NotEmpty();
            RuleFor(x => x.IdCodigoTipoPagamento).NotEmpty();
        }
    }
}
