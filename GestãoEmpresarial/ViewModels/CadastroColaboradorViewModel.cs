using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class CadastroColaboradorViewModel : CadastroViewModel<ColaboradorModel, EditarColaboradorModel>
    {
        public CadastroColaboradorViewModel(int? id, ColaboradorValidar validar, IDAL<ColaboradorModel> repositorio) 
            : base(id, validar, repositorio)
        {
        }

        // Campo privado que armazena o estado de foco do TextBox.
        // Inicializa como true para que o TextBox receba foco assim que a View for carregada.
        private bool _isTextBoxFocused = true;

        // Propriedade pública que permite o binding com a View (XAML). Neste Caso Para adicionar um foco no TextBox
        // Essa propriedade é observada pela View, e qualquer mudança nela reflete no controle associado.
        public bool IsTextBoxFocused
        {
            get => _isTextBoxFocused;
            set
            {
                _isTextBoxFocused = value;
                RaisePropertyChanged(nameof(IsTextBoxFocused));
            }
        }
    }
}
