using GestãoEmpresarial.ViewModels;
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

namespace GestãoEmpresarial.Views.Relatorios
{
    /// <summary>
    /// Interação lógica para RelatorioHistoricoVenda.xam
    /// </summary>
    public partial class RelatorioHistoricoVenda : UserControl
    {
        public RelatorioHistoricoVenda(RelatorioHistoricoVendasViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
