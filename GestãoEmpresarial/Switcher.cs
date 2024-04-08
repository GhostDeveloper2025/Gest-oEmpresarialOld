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
        //public static MenuView pageSwitcher;
        //private static Dictionary<ObservableObject, UserControl> Menus;

        ///// <summary>
        ///// É um construtor (metodo) estático, para inicializar as variaveis necessárias.
        ///// </summary>
        //static Switcher()
        //{
        //    Menus = new Dictionary<ObservableObject, UserControl>();
        //}

        //public static void Switch(Type type, object[] paramters)
        //{
        //    pageSwitcher.Navigate(type, paramters);
        //}

        //public static void Switch(ItemMenu newPage)
        //{
        //    pageSwitcher.Navigate(newPage, newPage.Parameters);
        //}
    }
}
