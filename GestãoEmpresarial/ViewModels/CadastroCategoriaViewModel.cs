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
    internal class CadastroCategoriaViewModel : CadastroViewModel<CategoriaModel, EditarCategoriaModel>
    {
        public CadastroCategoriaViewModel(int? id, CategoriaValidar categoriaValidar, IDAL<CategoriaModel> Repositorio) 
            : base(id, categoriaValidar, Repositorio)
        {
        }
    }
}
