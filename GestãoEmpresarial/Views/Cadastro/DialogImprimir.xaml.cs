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
using System.Windows.Shapes;

namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Lógica interna para DialogImprimir.xaml
    /// </summary>
    public partial class DialogImprimir : Window
    {
        public DialogImprimir(ICadastroViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void FecharJanela_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
