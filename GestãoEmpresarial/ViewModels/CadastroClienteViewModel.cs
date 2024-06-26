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
    public class CadastroClienteViewModel : CadastroViewModel<ClienteModel, EditarClienteModel>
    {
        public CadastroClienteViewModel(int? id, ClienteValidar validar, IDAL<ClienteModel> repositorio) 
            : base(id, validar, repositorio)
        {
        }
    }
}
