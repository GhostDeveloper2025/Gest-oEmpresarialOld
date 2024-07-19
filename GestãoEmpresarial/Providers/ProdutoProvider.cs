using AutoCompleteTextBox.Editors;
using GestãoEmpresarial.Models;
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
        public List<int> ListaExclusoes { get; private set; } = new List<int>();

        public IEnumerable GetSuggestions(string filter)
        {
            var repo = new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario);
            var list = repo.List(filter).Take(50); // Limita a 50 registros
            List<ProdutoModel> listaAux = new List<ProdutoModel>();
            foreach (var item in list)
            {
                if (ListaExclusoes.Any(a => Equals(a, item.IdProduto)))
                    continue;
                else
                {
                    listaAux.Add(item);
                }
            }
            return listaAux;
        }
    }
}
