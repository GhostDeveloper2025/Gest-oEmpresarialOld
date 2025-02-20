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
    internal class PesquisaVendaViewModel : PesquisaViewModel<VendaModel, DataGridVendaModel, RVendasDAL>
    {
        public Dictionary<int, string> TipoPagamentoList { get; internal set; }

        public override int Id => ObjectoSelecionado?.IdVenda ?? 0;
        public override string NomeEditarView => nameof(CadastroVendaViewModel);

        public string PesquisaNomeCliente { get; set; }
        public string PesquisaNumeroVenda { get; set; }
        public string PesquisaTipoPagamento { get; set; }

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
        public PesquisaVendaViewModel(RVendasDAL repositorio, RCodigosDAL repositorioCodigos)
            : base(repositorio)
        {
            _repositorioCodigos = repositorioCodigos;
            TipoPagamentoList = new Dictionary<int, string> { { 0, null } };
            CarregarTiposPagamentosAsync();
        }

        private async void CarregarTiposPagamentosAsync()
        {
            var codigos = await _repositorioCodigos.GetListaTiposPagamentosAsync();
            foreach (var codigo in codigos)
            {
                TipoPagamentoList.Add(codigo.Id, codigo.Nome);
            }

            RaisePropertyChanged(nameof(TipoPagamentoList));
        }

        public override DataGridVendaModel GetDataGridModel(VendaModel item)
        {
            return new DataGridVendaModel(item, TipoPagamentoList);
        }

        public override List<VendaModel> GetLista()
        {
            int? idVenda = int.TryParse(PesquisaNumeroVenda, out int idVendaAux) ? idVendaAux : (int?)null;
            int? idTipoPagamento = int.TryParse(PesquisaTipoPagamento, out int idTipoPagamentoAux) ? idTipoPagamentoAux : (int?)null;

            var lista = Repositorio.ListAsync(PesquisaNomeCliente, idTipoPagamento, idVenda).Result;

            PesquisaNomeCliente = null;
            RaisePropertyChanged(nameof(PesquisaNomeCliente));

            PesquisaNumeroVenda = null;
            RaisePropertyChanged(nameof(PesquisaNumeroVenda));

            PesquisaTipoPagamento = null;
            RaisePropertyChanged(nameof(PesquisaTipoPagamento));

            return lista;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return !string.IsNullOrWhiteSpace(PesquisaNomeCliente)
                   || !string.IsNullOrWhiteSpace(PesquisaNumeroVenda)
                   || !string.IsNullOrWhiteSpace(PesquisaTipoPagamento);
        }
    }
}

