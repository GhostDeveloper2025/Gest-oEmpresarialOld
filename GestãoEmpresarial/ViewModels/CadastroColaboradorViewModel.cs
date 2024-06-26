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
    public class CadastroColaboradorViewModel : CadastroViewModel<ColaboradorModel, EditarColaboradorModel>
    {
        public CadastroColaboradorViewModel(int? id, ColaboradorValidar validar, IDAL<ColaboradorModel> repositorio) 
            : base(id, validar, repositorio)
        {
        }
    }
}
