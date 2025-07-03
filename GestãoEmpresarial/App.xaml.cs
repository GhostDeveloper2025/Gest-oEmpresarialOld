using System.Threading;
using System.Windows;
using GestãoEmpresarial.Services;
using GestãoEmpresarial.Themes;

namespace GestãoEmpresarial
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Agora, carregamos e aplicamos o tema aqui
            AplicarTemaInicial();
            bool isNewInstance;
            mutex = new Mutex(true, "GestaoEmpresarial", out isNewInstance);

            if (!isNewInstance)
            {
                MessageBox.Show("Hey, só um toque: o programa já está em execução!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);

        }

        private void AplicarTemaInicial()
        {
            // 1. Instanciamos o serviço de configuração
            var configService = new ConfiguracaoService();

            // 2. Carregamos a preferência do tema ("Dark" ou "Light")
            string temaSalvo = configService.CarregarTema();

            // 3. Convertemos a string para o nosso enum de tema
            ThemesController.ThemeTypes tipoDeTema = temaSalvo == "Dark"
                ? ThemesController.ThemeTypes.Dark
                : ThemesController.ThemeTypes.Light;

            // 4. Aplicamos o tema globalmente usando o nosso controlador
            ThemesController.SetTheme(tipoDeTema);
        }
    }
}
