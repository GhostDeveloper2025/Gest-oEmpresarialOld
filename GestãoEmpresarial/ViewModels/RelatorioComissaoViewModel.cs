using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
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
    public class RelatorioComissaoViewModel : ObservableObject
    {
        private readonly RRelatorioComissaoVendaDAL _relatorioComissaoDAL;

        public RelatorioComissaoViewModel(RRelatorioComissaoVendaDAL relatorioComissaoDAL)
        {
            _relatorioComissaoDAL = relatorioComissaoDAL;
            ListaComissaoVendas = new ObservableCollection<RelatorioComissaoVendasModel>();
            Paginas = new PaginacaoModel<RelatorioComissaoVendasModel>();
            Paginas.ObterItens = BuscarHistorico;
            BuscarHistoricoCommand = new RelayCommand(Paginas.IrParaPrimeiraPagina);
        }
        public ObservableCollection<RelatorioComissaoVendasModel> ListaComissaoVendas { get; }

        public ICommand BuscarHistoricoCommand { get; }
        public PaginacaoModel<RelatorioComissaoVendasModel> Paginas { get; set; }
        // Adicione este método para atualizar os totais
        private void AtualizarTotais()
        {
            RaisePropertyChanged(nameof(TotalVendido));
            RaisePropertyChanged(nameof(TotalComissao));
        }

        private (List<RelatorioComissaoVendasModel>, int) BuscarHistorico(int pagina, int totalporpagina)
        {
            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                var resultado = _relatorioComissaoDAL.ObterComissaoVendasAsync(
                    ColaboradorSelecionado?.IdFuncionario,
                    DataInicial,
                    DataFinal.Value.AddDays(1),
                    pagina,
                    totalporpagina
                ).GetAwaiter().GetResult();

                _totalVendido = resultado.TotalVendido;
                _TotalComissao = resultado.TotalComissao;
                AtualizarTotais();
                return (resultado.Itens, resultado.TotalRegistros);
            }
            else
            {
                AtualizarTotais();
                return (new List<RelatorioComissaoVendasModel>(), 0);
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

        private decimal _TotalComissao;
        public decimal TotalComissao
        {
            get
            {
                return _TotalComissao;
            }
        }

    }
}
