using GestãoEmpresarial.Models;
using GestãoEmpresarial.Providers;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Repositorios.GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    //public class RelatorioHistoricoVendasViewModel
    //{
    //    //private readonly RRelatoriosDAL _relatoriosDAL;

    //    //public RelatorioHistoricoVendasViewModel(RRelatoriosDAL relatoriosDAL)
    //    //{
    //    //    _relatoriosDAL = relatoriosDAL;
    //    //}

    //    ////public ClienteProvider ClienteProviderItem { get; set; }

    //    //private ClienteModel ClienteSelecionadoAux;
    //    //public ClienteModel ClienteSelecionado
    //    //{
    //    //    get { return ClienteSelecionadoAux; }
    //    //    set
    //    //    {
    //    //        ClienteSelecionadoAux = value;
    //    //        /*Raiseprop*/
    //    //    }
    //    //}

    //    //public List<RelatorioHistoricoVendasViewModel> ListaRelatorioProdutoMaisVendido
    //    //{
    //    //    //get
    //    //    //{
    //    //    //    //return _relatoriosDAL.ObterProdutoMaisVendido(ClienteSelecionado.Idcliente);
    //    //    //}
    //    //}
    //}

    public class RelatorioHistoricoVendasViewModel : ObservableObject
    {
        private readonly RRelatorioHistoricoVendasDAL _historicoVendasDAL;

        public RelatorioHistoricoVendasViewModel(RRelatorioHistoricoVendasDAL historicoVendasDAL)
        {
            _historicoVendasDAL = historicoVendasDAL;
            ListaHistoricoVendas = new ObservableCollection<RelatorioHistoricoVendasModel>();
            BuscarHistoricoCommand = new RelayCommand(BuscarHistorico);
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

        public ObservableCollection<RelatorioHistoricoVendasModel> ListaHistoricoVendas { get; }

        public ICommand BuscarHistoricoCommand { get; }

        private void BuscarHistorico()
        {
            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                var produtoId = ProdutoSelecionado?.IdProduto;
                var clienteId = ClienteSelecionado?.Idcliente;
                // var nomeCliente = ClienteSelecionado?.Nome;

                var historicoVendas = _historicoVendasDAL.ObterHistoricoVendas(clienteId, produtoId, DataInicial.Value, DataFinal.Value);

                ListaHistoricoVendas.Clear();
                foreach (var item in historicoVendas)
                {
                    ListaHistoricoVendas.Add(item);
                }
            }
            else
            {
                ListaHistoricoVendas.Clear();// Mantém a lista vazia se as datas não estiverem definidas
            }
        }
    }
}
