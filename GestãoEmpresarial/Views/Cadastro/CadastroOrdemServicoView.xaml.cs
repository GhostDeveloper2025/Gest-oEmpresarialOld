using GestãoEmpresarial.Models;
using GestãoEmpresarial.Utils;
using GestãoEmpresarial.ViewModels;
using GestãoEmpresarial.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Interação lógica para CadastroOrdemServicoView.xam
    /// </summary>
    public partial class CadastroOrdemServicoView : UserControl
    {
        public CadastroOrdemServicoView()
        {
            InitializeComponent();
            Methods.AddColumnToDataGrid(DgItensOsModel, typeof(ItensOrdemServicoModelObservavel));
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            int? id = ((CadastroOrdemServicoViewModel)DataContext).Id;
            var func = DI.CadastrosViews[nameof(RelatorioReciboOrdemServicoViewModel)];
            if (id.HasValue)
                Switcher.Imprimir(func(id));
        }
    }
}
