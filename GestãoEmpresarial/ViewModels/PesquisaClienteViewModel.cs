using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System.Collections.Generic;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaClienteViewModel : PesquisaViewModel<ClienteModel, DataGridClienteModel, RClienteDAL>
    {
        public PesquisaClienteViewModel(RClienteDAL Repositorio) : base(Repositorio)
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

        public override int Id => ObjectoSelecionado.Idcliente;
        public override string NomeEditarView => nameof(CadastroClienteViewModel);

        public string PesquisaCPF { get; set; }
        public string PesquisaCNPJ { get; set; }
        public string PesquisaCelular { get; set; }
        public string PesquisaNome { get; set; }

        public override DataGridClienteModel GetDataGridModel(ClienteModel item)
        {
            return new DataGridClienteModel(item);
        }

        public override List<ClienteModel> GetLista()
        {
            // Remove a formatação do CPF e do CNPJ antes da busca
            string pesquisaCpfSemFormatacao = PesquisaCPF?.Replace(".", "").Replace("-", "");
            string pesquisaCnpjSemFormatacao = PesquisaCNPJ?.Replace(".", "").Replace("/", "").Replace("-", "");
            var lista = Repositorio.ListAsync(null, pesquisaCpfSemFormatacao, pesquisaCnpjSemFormatacao, PesquisaCelular, PesquisaNome).Result;

            PesquisaCelular = null;
            RaisePropertyChanged(nameof(PesquisaCelular));

            PesquisaCPF = null;
            RaisePropertyChanged(nameof(PesquisaCPF));

            PesquisaCNPJ = null;
            RaisePropertyChanged(nameof(PesquisaCNPJ));

            PesquisaNome = null;
            RaisePropertyChanged(nameof(PesquisaNome));

            return lista;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return string.IsNullOrWhiteSpace(PesquisaCelular) == false
                || string.IsNullOrWhiteSpace(PesquisaCPF) == false
                || string.IsNullOrWhiteSpace(PesquisaCNPJ) == false
                || string.IsNullOrWhiteSpace(PesquisaNome) == false;
        }
    }
}