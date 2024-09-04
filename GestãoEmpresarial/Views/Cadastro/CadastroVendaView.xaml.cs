using GestãoEmpresarial.Models;
using GestãoEmpresarial.Utils;
using GestãoEmpresarial.ViewModels;
using GestãoEmpresarial.Views.Relatorios;
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

namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Interação lógica para CadastroVendaView.xam
    /// </summary>
    public partial class CadastroVendaView : UserControl
    {
        public CadastroVendaView()
        {
            InitializeComponent();
            Methods.AddColumnToDataGrid(DgItensModel, typeof(ItemVendaModelObservavel));
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            var uc = DI.GetRelatorioView(nameof(RelatorioReciboVendaViewModel));
            Switcher.Imprimir(uc);
        }
    }
}
