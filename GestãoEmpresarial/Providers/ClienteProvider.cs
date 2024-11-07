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
    public class ClienteProvider : MarkupExtensionGestaoEmpresarial, ISuggestionProvider
    {
        public async Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            var repo = new RClienteDAL(LoginViewModel.colaborador.IdFuncionario);

            try
            {
                // Obtém a lista filtrada de clientes, limitando a 20 registros
                var list = await repo.ListAsync(null, null, null, null, filter);
                var listaLimitada = list.Take(20);

                return listaLimitada;
            }
            catch (Exception ex)
            {
                // Log de erro ou tratamento de exceção, se necessário
                Console.WriteLine($"Erro ao obter sugestões de clientes: {ex.Message}");
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

