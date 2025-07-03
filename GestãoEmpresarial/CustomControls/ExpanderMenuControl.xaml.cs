using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Utils;

namespace GestãoEmpresarial.CustomControls
{
    /// <summary>
    /// Interação lógica para ExpanderMenuControl.xam
    /// </summary>
    public partial class ExpanderMenuControl : UserControl
    {
        public ExpanderMenuControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MenuStateManager.RegisterMenu(ExpanderMenu);
        }

        private async void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewMenu.SelectedItem is NavigationMenu selectedItem && selectedItem.GetView != null)
            {
                Switcher.Switch(selectedItem);
                ListViewMenu.SelectedItem = null;

                // Adiciona um pequeno delay ANTES de fechar o menu
                await Task.Delay(600); // 500 milissegundos (meio segundo)
                ExpanderMenu.IsExpanded = false;
            }
        }

        private async void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NavigationMenu item && item.GetView != null && !HasChildItems(item))
            {
                Switcher.Switch(item);

                // Pequeno delay para dar tempo de abrir a view
                await Task.Delay(600);
                ExpanderMenu.IsExpanded = false;
            }
        }


        private bool HasChildItems(NavigationMenu item)
        {
            return item.Items != null && item.Items.Count > 0;
        }
    }
}
