using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroProdutoViewModel : CadastroViewModel<ProdutoModel, EditarProdutoModel>
    {
        public CadastroProdutoViewModel(int? id, IDAL<ProdutoModel> Repositorio) : base(id, Repositorio)
        {
        }
    }
}
