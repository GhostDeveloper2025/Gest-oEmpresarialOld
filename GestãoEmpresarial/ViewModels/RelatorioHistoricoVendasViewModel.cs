using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioHistoricoVendasViewModel : ObservableObject
    {
        private readonly RRelatorioHistoricoVendasDAL _historicoVendasDAL;

        public RelatorioHistoricoVendasViewModel(RRelatorioHistoricoVendasDAL historicoVendasDAL)
        {
            _historicoVendasDAL = historicoVendasDAL;
            Paginas = new PaginacaoModel<RelatorioHistoricoVendasModel>();
            Paginas.ObterItens = BuscarHistorico;
            BuscarHistoricoCommand = new RelayCommand(Paginas.IrParaPrimeiraPagina);
        }
        public ICommand BuscarHistoricoCommand { get; }
        public PaginacaoModel<RelatorioHistoricoVendasModel> Paginas { get; set; }

        private (List<RelatorioHistoricoVendasModel>, int) BuscarHistorico(int pagina, int totalporpagina)
        {
            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                var produtoId = ProdutoSelecionado?.IdProduto;
                var clienteId = ClienteSelecionado?.Idcliente;

                var resultado = _historicoVendasDAL.ObterHistoricoVendasAsync(
                    clienteId,
                    produtoId,
                    DataInicial,
                    DataFinal.Value.AddDays(1),
                    pagina,
                    totalporpagina
                ).GetAwaiter().GetResult();

                _totalVendido = resultado.TotalVendido;
                //Atualiza os totais sempre que a página muda
                RaisePropertyChanged(nameof(TotalVendido));
                return (resultado.Itens, resultado.TotalRegistros);
            }
            else
            {
                return (new List<RelatorioHistoricoVendasModel>(), 0);
            }
        }

        private ClienteModel _clienteSelecionado;
        public ClienteModel ClienteSelecionado
        {
            get { return _clienteSelecionado; }
            set
            {
                if (_clienteSelecionado != value)
                {
                    _clienteSelecionado = value;
                    RaisePropertyChanged(nameof(ClienteSelecionado));
                }
            }
        }

        private ProdutoModel _produtoSelecionado;
        public ProdutoModel ProdutoSelecionado
        {
            get { return _produtoSelecionado; }
            set
            {
                if (_produtoSelecionado != value)
                {
                    _produtoSelecionado = value;
                }
            }
        }

        private DateTime? _dataInicial;
        public DateTime? DataInicial
        {
            get { return _dataInicial; }
            set
            {
                if (_dataInicial != value)
                {
                    _dataInicial = value;
                    RaisePropertyChanged(nameof(DataInicial));
                }
            }
        }

        private DateTime? _dataFinal;
        public DateTime? DataFinal
        {
            get { return _dataFinal; }
            set
            {
                if (_dataFinal != value)
                {
                    _dataFinal = value;
                    RaisePropertyChanged(nameof(DataFinal));
                }
            }
        }

        private decimal _totalVendido;
        public decimal TotalVendido
        {
            get
            {
                return _totalVendido;
            }
        }

    }
}
