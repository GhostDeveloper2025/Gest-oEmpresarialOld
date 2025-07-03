using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wrapper.WhatsGW
{
    public static class WppApi
    {
        private async static Task<HttpResponseMessage?> Executar(Action<HttpRequestMessage> action)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, "https://app.whatsgw.com.br/api/WhatsGw/Send"))
                {
                    action(request);
                    var response = await client.SendAsync(request);
                    return response;
                }
            }
        }

        public async static Task<bool> EnviarMensagem(string numero, string corpo)
        {
            try
            {
                Mensagem msg = new()
                {
                    message_body = corpo,
                    contact_phone_number = numero
                };
                string jsonString = JsonSerializer.Serialize(msg);

                var response = await Executar((request) =>
                {
                    var content = new StringContent(jsonString, null, "application/json");
                    request.Content = content;
                });

                return response?.IsSuccessStatusCode ?? false;
            }
            catch
            {
                return false;
            }
        }

        // NOVO MÉTODO PARA ENVIAR DOCUMENTOS
        public static async Task<(bool Sucesso, string Mensagem)> EnviarDocumentoAsync(string numero, byte[] documentoBytes, string nomeArquivo)
        {
            if (documentoBytes == null || documentoBytes.Length == 0)
                return (false, "Documento está vazio ou nulo.");

            try
            {
                string base64String = Convert.ToBase64String(documentoBytes);

                var msg = new Mensagem
                {
                    message_type = "document",
                    message_body = base64String,
                    message_body_filename = nomeArquivo,
                    contact_phone_number = numero
                };

                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string jsonString = JsonSerializer.Serialize(msg, options);

                Debug.WriteLine($"JSON enviado: {jsonString}");

                var response = await Executar(request =>
                {
                    request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                });

                if (response == null)
                    return (false, "Nenhuma resposta da API.");

                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Resposta da API: {responseContent}");

                if (!response.IsSuccessStatusCode)
                    return (false, $"Erro da API: {response.StatusCode} - {responseContent}");

                return (true, "Documento enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao enviar documento: {ex}");
                return (false, $"Exceção: {ex.Message}");
            }
        }

    }
}
