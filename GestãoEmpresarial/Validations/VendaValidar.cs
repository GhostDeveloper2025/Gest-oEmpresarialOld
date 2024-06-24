using FluentValidation;
using NewProject.Models;
using NewProject.Repositorios;
using NewProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject.Validations
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
