using GestãoEmpresarial.Models;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        }

        internal void Navigate(TreeviewMenu menu)
        {
            if (menu.GetView != null)
            {
                //TreeviewMenu
                UserControl view = menu.GetView();
                //if (menu.ScreenParameters == null)
                //    view = (UserControl)Activator.CreateInstance(menu.Screen);
                //else
                //    view = (UserControl)Activator.CreateInstance(menu.Screen, menu.ScreenParameters);
                
                fContainer.Children.Clear();
                fContainer.Children.Add(view);
                //só coloca focus depois da thread atual terminar, para ocorrer tem de ser focusable
                Focus(view);
            }
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
