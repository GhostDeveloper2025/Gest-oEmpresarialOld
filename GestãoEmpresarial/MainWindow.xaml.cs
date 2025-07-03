using GestãoEmpresarial.Models;
using GestãoEmpresarial.Views.Login;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestãoEmpresarial
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Switcher.pageSwitcher = this;
            Navigate(DI.PaginasView[nameof(LoginView)]());
        }

        internal void Navigate(UserControl view)
        {
            PaginaPrincipal.Children.Clear();
            PaginaPrincipal.Children.Add(view);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Exibe uma mensagem de confirmação
            MessageBoxResult result = MessageBox.Show(
                "Hey! Hora de ir embora. Quer mesmo fechar o programa?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Se o usuário escolher "Não", cancela o fechamento da janela
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancela o fechamento
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Usa apenas a área de trabalho disponível (sem cobrir a taskbar)
            var workArea = SystemParameters.WorkArea;

            this.Left = workArea.Left;
            this.Top = workArea.Top;
            this.Width = workArea.Width;
            this.Height = workArea.Height;
        }

    }

}
