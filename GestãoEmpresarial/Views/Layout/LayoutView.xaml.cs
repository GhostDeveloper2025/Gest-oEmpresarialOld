using GestãoEmpresarial.Models;
using GestãoEmpresarial.Themes;
using GestãoEmpresarial.ViewModels;
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

            DataContext = new LayoutViewModel();
        }


        internal void Navigate(NavigationMenu menu)
        {
            if (menu.GetView != null)
            {
                if (DataContext is LayoutViewModel vm)
                {
                    vm.CurrentView = menu.GetView();
                    vm.Header = menu.Header; 
                    vm.Icon = menu.Icon;
                }
            }
        }

        internal void Navigate(UserControl view)
        {
            if (DataContext is LayoutViewModel vm)
            {
                vm.CurrentView = view;
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
