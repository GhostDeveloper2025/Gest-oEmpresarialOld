using AutoCompleteTextBox.Editors;
using GestãoEmpresarial.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Providers
{
    internal class ClienteProvider : MarkupExtensionGestaoEmpresarial, ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            return null;
            //var repo = new RClienteDAL(LoginViewModel.colaborador.IdFuncionario);
            //var list = repo.List(filter).Take(20); // Limita a 20 registros
            //return list;
        }
    }
}
