using GestãoEmpresarial.Models;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace GestãoEmpresarial.Views.Layout
{
    /// <summary>
    /// Interação lógica para MenuView.xam
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new MenuViewModel();
            }

        }

        private void OnMenuItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel panel && panel.DataContext is NavigationMenu menu)
            {
                Switcher.Switch(menu); // Método para mudar para a tela desejada
            }
        }
    }
}
