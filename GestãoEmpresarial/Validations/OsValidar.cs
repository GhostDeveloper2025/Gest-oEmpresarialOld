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
    public class OsValidar : AbstractValidator<OsModel>
    {
        //Cada regra é independente (ou seja, cada regra tem de ser verdade)        
        public OsValidar()
        {
            RCodigosDAL codigos = new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario);
            var prontoParaConsertoId = codigos.GetStatusProntoParaConserto().Id;

            //When = Quando, portanto quando o estado é "X", o idresponsavel tem de estar preenchido
            When(x => x.Status == prontoParaConsertoId, () =>
            {
                RuleFor(x => x.IdResponsavel).NotEmpty();
            });//.WithMessage("O Campo `Responsavel` È Obrigatório. Favor Preencher");
            RuleFor(x => x.Cliente.Idcliente).NotEmpty().When(a => a.Cliente != null);

            RuleFor(x => x.Cliente).NotEmpty();
            RuleFor(x => x.Ferramenta).NotEmpty();
            RuleFor(x => x.Marca).NotEmpty();
            RuleFor(x => x.Modelo).NotEmpty();

        }
    }
}
