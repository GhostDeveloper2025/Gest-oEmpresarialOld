using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroOrdemServicoViewModel : CadastroViewModel<OsModel, EditarOsModel>
    {
        public CadastroOrdemServicoViewModel(int? id, IDAL<OsModel> Repositorio) : base(id, Repositorio)
        {
        }
    }
}
