using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaOrdemServicoViewModel : PesquisaViewModel<OrdemServicoModel, DataGridOrdemServicoModel, ROsDAL>
    {
        public Dictionary<int, string> StatusList { get; internal set; }

        public override int Id => ObjectoSelecionado?.IdOs ?? 0; // Verifica se ObjectoSelecionado não é nulo
        public override string NomeEditarView => nameof(CadastroOrdemServicoViewModel);

        public string PesquisaNomeCliente { get; set; }
        public string PesquisaNumeroOS { get; set; }
        public string PesquisaStatus { get; set; }

        private readonly RCodigosDAL _repositorioCodigos;

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

        public PesquisaOrdemServicoViewModel(ROsDAL repositorio, RCodigosDAL repositorioCodigos)
            : base(repositorio)
        {
            _repositorioCodigos = repositorioCodigos;
            StatusList = new Dictionary<int, string>
            {
                { 0, null }
            };

            // Carregar os status de forma assíncrona
            CarregarStatusAsync();
        }

        private async void CarregarStatusAsync()
        {
            var codigos = await _repositorioCodigos.GetListaStatusAsync();
            foreach (var codigo in codigos)
            {
                StatusList.Add(codigo.Id, codigo.Nome);
            }

            // Notificar que a lista de status foi atualizada
            RaisePropertyChanged(nameof(StatusList));
        }

        public override DataGridOrdemServicoModel GetDataGridModel(OrdemServicoModel item)
        {
            return new DataGridOrdemServicoModel(item, StatusList);
        }
        public override List<OrdemServicoModel> GetLista()
        {
            int? idStatus = null;
            if (int.TryParse(PesquisaStatus, out int idStatusAux))
                idStatus = idStatusAux;

            int? idOs = null;
            if (int.TryParse(PesquisaNumeroOS, out int idOsAux))
                idOs = idOsAux;

            // Executa a consulta com os filtros aplicados
            var lista = Repositorio.ListAsync(PesquisaNomeCliente, idStatus, null, idOs).Result;

            // Limpa os campos de pesquisa após a consulta
            PesquisaNomeCliente = null;
            RaisePropertyChanged(nameof(PesquisaNomeCliente));

            PesquisaNumeroOS = null;
            RaisePropertyChanged(nameof(PesquisaNumeroOS));

            PesquisaStatus = null;
            RaisePropertyChanged(nameof(PesquisaStatus));

            return lista;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return !string.IsNullOrWhiteSpace(PesquisaNomeCliente)
                || !string.IsNullOrWhiteSpace(PesquisaNumeroOS)
                || !string.IsNullOrWhiteSpace(PesquisaStatus);
        }

    }
}

