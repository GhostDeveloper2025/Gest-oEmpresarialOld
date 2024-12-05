using GestãoEmpresarial.Models;
using GestãoEmpresarial.Views;
using GestãoEmpresarial.Views.Layout;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestãoEmpresarial
{
    /// <summary>
    /// É uma classe static (estatica) para que apenas exista 1 durante a vida da aplicação
    /// Ou seja, o valor colocado é sempre igual em qualquer lado.
    /// </summary>
    internal class Switcher
    {
        public static MainWindow pageSwitcher;
        public static LayoutView layoutSwitcher;

        /// <summary>
        /// É um construtor (metodo) estático, para inicializar as variaveis necessárias.
        /// </summary>
        static Switcher()
        {
        }

        public static void Switch(TreeviewMenu menu)
        {
            layoutSwitcher.Navigate(menu);
        }

        public static void Switch(UserControl view)
        {
            layoutSwitcher.Navigate(view);
            Focus(view);
        }

        public static void SwitchPagina(UserControl view)
        {
            pageSwitcher.Navigate(view);
            Focus(view);
        }

        public static void Imprimir(UserControl uc)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.ShowDialog();
            printDlg.PrintVisual(uc, "User Control Printing.");
        }

        public static void Focus(UIElement element)
        {
            if (!element.Focus())
            {
                element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate ()
                {
                    if (element.Focusable == true)
                    {
                        element.Focus();
                        Keyboard.Focus(element);
                    }
                }));
            }
        }
    }
}
