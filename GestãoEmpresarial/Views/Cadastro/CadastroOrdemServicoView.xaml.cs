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
using System.Windows.Input;

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
    }
}
