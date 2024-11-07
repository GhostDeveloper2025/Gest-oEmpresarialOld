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

        public override int Id => ObjectoSelecionado?.IdVenda ?? 0; // Garantir que ObjectoSelecionado não seja nulo
        public override string NomeEditarView => nameof(CadastroVendaViewModel);

        public string PesquisaNomeCliente { get; set; }
        public string PesquisaNumeroVenda { get; set; }
        public string PesquisaTipoPagamento { get; set; }

        private readonly RCodigosDAL _repositorioCodigos;

        public PesquisaVendaViewModel(RVendasDAL repositorio, RCodigosDAL repositorioCodigos)
            : base(repositorio)
        {
            _repositorioCodigos = repositorioCodigos;
            TipoPagamentoList = new Dictionary<int, string>
            {
                { 0, null }
            };

            // Carregar os tipos de pagamento de forma assíncrona
            CarregarTiposPagamentosAsync();
        }

        private async void CarregarTiposPagamentosAsync()
        {
            var codigos = await _repositorioCodigos.GetListaTiposPagamentosAsync();
            foreach (var codigo in codigos)
            {
                TipoPagamentoList.Add(codigo.Id, codigo.Nome);
            }

            // Notificar que a lista de tipos de pagamento foi atualizada, caso seja necessário algum binding
            RaisePropertyChanged(nameof(TipoPagamentoList));
        }

        public override DataGridVendaModel GetDataGridModel(VendaModel item)
        {
            return new DataGridVendaModel(item, TipoPagamentoList);
        }

        public override List<VendaModel> GetLista()
        {
            int? idVenda = null;
            if (int.TryParse(PesquisaNumeroVenda, out int idVendaAux))
                idVenda = idVendaAux;

            int? idCodigo = null;
            if (int.TryParse(PesquisaTipoPagamento, out int idCodigoAux))
                idCodigo = idCodigoAux;

            return Repositorio.ListAsync(PesquisaNomeCliente, idCodigo, idVendaAux).Result;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return string.IsNullOrWhiteSpace(PesquisaNomeCliente) == false
                  || string.IsNullOrWhiteSpace(PesquisaNumeroVenda) == false
                  || string.IsNullOrWhiteSpace(PesquisaTipoPagamento) == false;
        }
    }
}

