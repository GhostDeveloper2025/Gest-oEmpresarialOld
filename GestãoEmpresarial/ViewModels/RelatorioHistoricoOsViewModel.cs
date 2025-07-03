using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System.Windows.Input;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioHistoricoOsViewModel : ObservableObject
    {
        private readonly RRelatorioHistoricoOsDAL _historicoOsDAL;
        private readonly RCodigosDAL _repositorioCodigos;

        public Dictionary<int, string> StatusList { get; private set; }
        public string PesquisaStatus { get; set; }
        public ICommand BuscarHistoricoCommand { get; set; }
        
        public PaginacaoModel<RelatorioHistoricoOsModel> Paginas { get; set; }
        public decimal TotalVendido { get; set; }
        
        public RelatorioHistoricoOsViewModel(RRelatorioHistoricoOsDAL historicoOsDAL, RCodigosDAL repositorioCodigos)
        {
            _historicoOsDAL = historicoOsDAL;
            _repositorioCodigos = repositorioCodigos;
            StatusList = new Dictionary<int, string>();
            CarregarStatusAsync();

            Paginas = new PaginacaoModel<RelatorioHistoricoOsModel>();
           // Paginas.TotalPorPagina = 20;
            Paginas.ObterItens = BuscarHistorico;

            BuscarHistoricoCommand = new RelayCommand(Paginas.IrParaPrimeiraPagina);
        }

        private async void CarregarStatusAsync()
        {
            int? idStatus = null;
            if (int.TryParse(PesquisaStatus, out int idStatusAux))
                idStatus = idStatusAux;

            var codigos = await _repositorioCodigos.GetListaStatusAsync();
            StatusList = codigos.ToDictionary(codigo => codigo.Id, codigo => codigo.Nome);
            RaisePropertyChanged(nameof(StatusList));
        }

        private (List<RelatorioHistoricoOsModel>, int) BuscarHistorico(int pagina, int totalporpagina)
        {
            var clienteId = ClienteSelecionado?.Idcliente;
            var tecnicoId = ColaboradorSelecionado?.IdFuncionario;
            int? status = StatusSelecionado > 0 ? StatusSelecionado : (int?)null;

            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                var (itens, totalR, totalV) =  _historicoOsDAL.ObterHistoricoOsAsync(
                    clienteId, 
                    tecnicoId, 
                    DataInicial.Value, 
                    DataFinal.Value.AddDays(1), 
                    status, 
                    pagina, 
                    totalporpagina).GetAwaiter().GetResult();
                TotalVendido = totalV;
                RaisePropertyChanged(nameof(TotalVendido));
                return (itens, totalR);
            }
            else
            {
                TotalVendido = 0;
                RaisePropertyChanged(nameof(TotalVendido));
                return (null, 0);
            }

        }

        private ClienteModel _clienteSelecionado;
        public ClienteModel ClienteSelecionado
        {
            get => _clienteSelecionado;
            set
            {
                if (_clienteSelecionado != value)
                {
                    _clienteSelecionado = value;
                    RaisePropertyChanged(nameof(ClienteSelecionado));
                }
            }
        }

        private ColaboradorModel _colaboradorSelecionado;
        public ColaboradorModel ColaboradorSelecionado
        {
            get { return _colaboradorSelecionado; }
            set
            {
                if (_colaboradorSelecionado != value)
                {
                    _colaboradorSelecionado = value;
                    RaisePropertyChanged(nameof(ColaboradorSelecionado));
                }
            }
        }

        private ProdutoModel _produtoSelecionado;
        public ProdutoModel ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                if (_produtoSelecionado != value)
                {
                    _produtoSelecionado = value;
                    RaisePropertyChanged(nameof(ProdutoSelecionado));
                }
            }
        }

        private DateTime? _dataInicial;
        public DateTime? DataInicial
        {
            get => _dataInicial;
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
            get => _dataFinal;
            set
            {
                if (_dataFinal != value)
                {
                    _dataFinal = value;
                    RaisePropertyChanged(nameof(DataFinal));
                }
            }
        }

        private int _statusSelecionado;
        public int StatusSelecionado
        {
            get => _statusSelecionado;
            set
            {
                if (_statusSelecionado != value)
                {
                    _statusSelecionado = value;
                    RaisePropertyChanged(nameof(StatusSelecionado));
                }
            }
        }
    }
}
