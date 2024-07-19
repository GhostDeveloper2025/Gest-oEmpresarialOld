using GestãoEmpresarial.Models;
using GestãoEmpresarial.Views;
using GestãoEmpresarial.Views.Layout;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GestãoEmpresarial
{
    /// <summary>
    /// É uma classe static (estatica) para que apenas exista 1 durante a vida da aplicação
    /// Ou seja, o valor colocado é sempre igual em qualquer lado.
    /// </summary>
    internal class Switcher
    {
        public static MainWindow pageSwitcher;

        /// <summary>
        /// É um construtor (metodo) estático, para inicializar as variaveis necessárias.
        /// </summary>
        static Switcher()
        {
        }

        public static void Switch(TreeviewMenu menu)
        {
            pageSwitcher.Navigate(menu);
        }

        public static void Switch(UserControl view)
        {
            pageSwitcher.Navigate(view);
        }
    }
}
