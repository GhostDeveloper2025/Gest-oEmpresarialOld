using AutoCompleteTextBox.Editors;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Utils;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Providers
{
    internal class ProdutoProvider : MarkupExtensionGestaoEmpresarial, ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            var repo = new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario);
            var list = repo.List(filter).Take(50); // Limita a 50 registros
            return list;
        }
    }
}
