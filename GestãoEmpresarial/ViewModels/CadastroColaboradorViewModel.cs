using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class CadastroColaboradorViewModel : CadastroViewModel<ColaboradorModel, EditarColaboradorModel>
    {
        public CadastroColaboradorViewModel(int? id , IDAL<ColaboradorModel> Repositorio) : base(id, Repositorio)
        {
        }
    }
}
