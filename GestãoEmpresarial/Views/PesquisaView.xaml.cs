using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
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

namespace GestãoEmpresarial.Views
{
    /// <summary>
    /// Interação lógica para PesquisaView.xam
    /// </summary>
    public partial class PesquisaView : UserControl
    {
        public PesquisaView(IPesquisaViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = ((PropertyDescriptor)e.PropertyDescriptor).DisplayName;
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
            else if (e.PropertyType == typeof(System.DateTime?))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            //Switcher.Switch(EditViewType, new[] { DataGridGlobal.SelectedItem });
        }
    }
}
