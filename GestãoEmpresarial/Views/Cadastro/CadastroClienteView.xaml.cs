using GestãoEmpresarial.Models;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Interação lógica para CadastroClienteView.xam
    /// </summary>
    public partial class CadastroClienteView : UserControl
    {
        public CadastroClienteView()
        {
            InitializeComponent();
        }
        private async void TxtCep_KeyUp(object sender, KeyEventArgs e)
        {
            string pCep = TxtCep.Text.Replace("-", "");
            int n;
            bool isNumeric = int.TryParse(pCep, out n);

            if (isNumeric && pCep.Length == 8)
            {
                // Exibe a barra de progresso
                MostrarBarraProgresso();

                try
                {
                    // Inicia a tarefa assíncrona para a busca do CEP
                    string json = await Task.Run(() => BuscarCep(pCep));

                    // Processa o JSON na thread da UI
                    ProcessarJson(json);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Erro na requisição HTTP: {ex.Message}");
                }
                catch (SocketException ex)
                {
                    MessageBox.Show($"Erro de conexão de rede: {ex.Message}");
                }
                finally
                {
                    // Esconde a barra de progresso após a conclusão da busca
                    OcultarBarraProgresso();
                }
            }
        }

        private async Task<string> BuscarCep(string cep)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response;

                try
                {
                    response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
                    response.EnsureSuccessStatusCode(); // Lança exceção se o código de status não for de sucesso
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException("Falha Na Conexão Erro na requisição HTTP.", ex);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        private void ProcessarJson(string json)
        {
            if (json == null)
            {
                return;
            }

            JObject jsonObj = JObject.Parse(json);

            if (jsonObj.TryGetValue("erro", out JToken erroToken) && erroToken.Value<bool>())
            {
                MessageBox.Show("CEP não encontrado ou inválido.");
            }
            else
            {
                AtualizarInterfaceUsuario(json);
            }
        }

        private void AtualizarInterfaceUsuario(string json)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var lCep = JsonConvert.DeserializeObject<ViaCepModel>(json);

                TxtUF.Text = lCep.uf;
                TxtBairro.Text = lCep.bairro;
                TxtLocalidade.Text = lCep.localidade;
                TxtLogradouro.Text = lCep.logradouro;
            });
        }

        private void MostrarBarraProgresso()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Mostrar a barra de progresso e definir IsIndeterminate como False
                progressBar.Visibility = Visibility.Visible;
                progressBar.IsIndeterminate = true;
            });
        }

        private void OcultarBarraProgresso()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Ocultar a barra de progresso e definir IsIndeterminate como True
                progressBar.Visibility = Visibility.Collapsed;
                progressBar.IsIndeterminate = false;
            });
        }
    }
}
