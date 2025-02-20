using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GestãoEmpresarial.Views.Layout
{
    /// <summary>
    /// Interação lógica para LayoutView.xam
    /// </summary>
    public partial class LayoutView : UserControl
    {
        private readonly TextBlock titulo;

        public LayoutView()
        {
            InitializeComponent();
            Switcher.layoutSwitcher = this;
            titulo = new TextBlock()
            {
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }

        internal void Navigate(TreeviewMenu menu)
        {
            BusyIndicator.IsBusy = true;

            //await Task.Delay(System.TimeSpan.FromSeconds(2)); // para utilizar este codigo que tem o tempo definido em 2 segundo tenho que add o async
            if (menu.GetView != null)
            {
                DataContext = menu;
                UserControl view = menu.GetView();
                Navigate(view);
            }
            BusyIndicator.IsBusy = false;
        }

        internal void Navigate(UserControl view)
        {
            BusyIndicator.IsBusy = true;

            fContainer.Children.Clear();
            fContainer.Children.Add(view);

            //só coloca focus depois da thread atual terminar, para ocorrer tem de ser focusable
            Focus(view);
            BusyIndicator.IsBusy = false;
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
