using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaCategoriaViewModel : PesquisaViewModel<CategoriaModel, CategoriaModel>
    {
        public PesquisaCategoriaViewModel(IDAL<CategoriaModel> Repositorio) : base(Repositorio)
        {
        }
    }
}
