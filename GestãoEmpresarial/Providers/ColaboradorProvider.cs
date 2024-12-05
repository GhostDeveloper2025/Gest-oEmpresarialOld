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
    internal abstract class ColaboradorProvider : MarkupExtensionGestaoEmpresarial, ISuggestionProvider
    {
        protected abstract string TipoColaborador { get; }

        public async Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            var repo = DI.GetRepositorio<RColaboradorDAL>();

            try
            {
                // Obtém a lista filtrada de colaboradores e limita a 20 registros
                var list = await repo.ListAsync(filter);
                var listaLimitada = list.Where(a => a.Cargo.IndexOf(TipoColaborador, StringComparison.OrdinalIgnoreCase) >= 0).Take(20);
                return listaLimitada;
            }
            catch (Exception ex)
            {
                // Log de erro ou tratamento de exceção, se necessário
                Console.WriteLine($"Erro ao obter sugestões de colaboradores: {ex.Message}");
                return Enumerable.Empty<object>(); // Retorna uma lista vazia em caso de erro
            }
        }

        //// Implementação síncrona exigida pela interface ISuggestionProvider
        //Task<IEnumerable> ISuggestionProvider.GetSuggestions(string filter)
        //{
        //    // Chama o método assíncrono e retorna o resultado
        //    return GetSuggestionsAsync(filter);
        //}

        // Método síncrono exigido pela interface ISuggestionProvider
        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            // Como ISuggestionProvider requer um método síncrono, usamos o `Result` para aguardar a execução do método assíncrono
            return GetSuggestionsAsync(filter).Result;
        }
    }
}

