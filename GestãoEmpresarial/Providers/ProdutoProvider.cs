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

//public interface ISuggestionProvider
//{
//    Task<IEnumerable> GetSuggestions(string filter); // Certifique-se de que a assinatura é esta
//}

namespace GestãoEmpresarial.Providers
{
    internal class ProdutoProvider : MarkupExtensionGestaoEmpresarial, ISuggestionProvider
    {
        public List<int> ListaExclusoes { get; private set; } = new List<int>();

        // Novo pesquisar exclusivo provider
        public async Task<IEnumerable<ProdutoModel>> GetSuggestionsAsync(string filter)
        {
            var repo = DI.GetRepositorio<RProdutoDAL>();

            try
            {
                // Usando o método específico para autocomplete, que busca por nome e código
                var list = await repo.ListForAutoCompleteAsync(filter);

                // Limita a 50 registros e filtra a lista de exclusões
                var listaFiltrada = list
                    .Where(item => !ListaExclusoes.Contains(item.IdProduto))
                    .Take(50)
                    .ToList();

                return listaFiltrada;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter sugestões de produtos: {ex.Message}");
                return Enumerable.Empty<ProdutoModel>(); // Retorna uma lista vazia em caso de erro
            }
        }

        // Método síncrono exigido pela interface ISuggestionProvider
        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            // Como ISuggestionProvider requer um método síncrono, usamos o `Result` para aguardar a execução do método assíncrono
            return GetSuggestionsAsync(filter).Result;
        }
    }
}



