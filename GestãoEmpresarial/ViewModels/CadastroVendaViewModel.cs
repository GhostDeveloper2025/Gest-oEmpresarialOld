using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroVendaViewModel : CadastroViewModel<VendaModel, EditarVendaModel>
    {
        public Dictionary<int, string> TiposPagamento { get; internal set; }

        public CadastroVendaViewModel(int? id, VendaValidar validar, IDAL<VendaModel> repositorio, RCodigosDAL codigosDAL, RItensVendaDAL itensVendaDAL) 
            : base(id, validar, repositorio)
        {

            itensVendaDAL = new RItensVendaDAL(LoginViewModel.colaborador.IdFuncionario);

            if (id.HasValue)
            {
                itensVendaDAL.GetByIdVenda(id.Value);
            }
            TiposPagamento = codigosDAL.GetListaTiposPagamentos().ToDictionary(b => b.Id, a => a.Nome);
        }
    }
}
