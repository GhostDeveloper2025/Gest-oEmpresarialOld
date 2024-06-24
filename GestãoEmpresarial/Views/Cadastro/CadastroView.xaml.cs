using GestãoEmpresarial.ViewModels;
using GestãoEmpresarial.Views.Pesquisa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Interação lógica para CadastroView.xam
    /// </summary>
    public partial class CadastroView : UserControl
    {
        public CadastroView(ICadastroViewModel viewModel, UIElement view)
        {
            InitializeComponent();
            DataContext = viewModel;
            fContainer.Children.Clear();
            fContainer.Children.Add(view);
        }
    }
}
