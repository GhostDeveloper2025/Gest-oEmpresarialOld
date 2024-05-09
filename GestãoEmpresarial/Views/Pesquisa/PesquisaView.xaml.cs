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

namespace GestãoEmpresarial.Views.Pesquisa
{
    /// <summary>
    /// Interação lógica para PesquisaView.xam
    /// </summary>
    public partial class PesquisaView : UserControl
    {
        public PesquisaView(IPesquisaViewModel viewModel)
            : this(viewModel, new PesquisaBarraView())
        {
        }

        public PesquisaView(IPesquisaViewModel viewModel, UIElement barraPesquisa)
        {
            InitializeComponent();
            DataContext = viewModel;
            if (barraPesquisa != null)
            {
                fContainer.Children.Add(barraPesquisa);
            }
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
