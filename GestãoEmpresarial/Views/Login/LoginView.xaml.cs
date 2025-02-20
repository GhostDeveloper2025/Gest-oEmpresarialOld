using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace GestãoEmpresarial.Views.Login
{
    /// <summary>
    /// Interação lógica para LoginView.xam
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = LoginViewModel.Instancia;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)DataContext).Senha = ((PasswordBox)sender).Password;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (LoginButton.Command != null)
                {
                    LoginButton.Command.Execute(null);
                }
            }
        }
        //private void TextBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (sender is TextBox textBox)
        //    {
        //        textBox.Focus();
        //        Keyboard.Focus(textBox);
        //    }
        //}

    }
}
