using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
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
        public CadastroVendaViewModel(int? id, VendaValidar validar, IDAL<VendaModel> repositorio) 
            : base(id, validar, repositorio)
        {
        }
    }
}
