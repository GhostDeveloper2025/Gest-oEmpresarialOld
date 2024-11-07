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
    public class OrdemServicoValidar : AbstractValidator<OrdemServicoModel>
    {
        //Cada regra é independente (ou seja, cada regra tem de ser verdade)        
        public OrdemServicoValidar()
        {
            RCodigosDAL codigos = new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario);
            //var prontoParaConsertoId = codigos.GetStatusProntoParaConsertoAsync().Result.Id;
            var orcamentoConcluidoId = codigos.GetStatusOrcamentoConcluidoAsync().Result.Id;

            When(x => x.Status == orcamentoConcluidoId, () =>
            {
                RuleFor(x => x.Tecnico).NotEmpty();
                RuleFor(x => x.Responsavel).NotEmpty();
            });

            //When = Quando, portanto quando o estado é "X", o idresponsavel tem de estar preenchido
            //When(x => x.Status == prontoParaConsertoId, () =>
            //{
            //    RuleFor(x => x.IdResponsavel).NotEmpty();
            //});//.WithMessage("O Campo `Responsavel` È Obrigatório. Favor Preencher");

            RuleFor(x => x.Cliente.Idcliente).NotEmpty().When(a => a.Cliente != null);
            RuleFor(x => x.Cliente).NotEmpty();

            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.Ferramenta).NotEmpty();
            RuleFor(x => x.Marca).NotEmpty();
            RuleFor(x => x.Modelo).NotEmpty();
        }
    }
}
