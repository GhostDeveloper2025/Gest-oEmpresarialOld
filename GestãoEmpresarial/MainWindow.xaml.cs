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
            //BusyIndicator.IsBusy = true;
            PaginaPrincipal.Children.Clear();
            PaginaPrincipal.Children.Add(view);
            //só coloca focus depois da thread atual terminar, para ocorrer tem de ser focusable
            //Focus(view);
            //BusyIndicator.IsBusy = false;
        }
    }
}
