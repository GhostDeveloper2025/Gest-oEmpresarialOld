using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class CadastroClienteViewModel : CadastroViewModel<ClienteModel, EditarClienteModel>
    {
        public CadastroClienteViewModel(int? id, IDAL<ClienteModel> Repositorio) : base(id, Repositorio)
        {
        }
    }
}
