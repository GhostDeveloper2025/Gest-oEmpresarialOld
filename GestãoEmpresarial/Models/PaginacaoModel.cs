using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MicroMvvm;

namespace GestãoEmpresarial.Models
{
    public class PaginacaoModel<T> : ObservableObject
    {
        private int _paginaAtual;
        private int _totalPaginas;
        private int _totalRegistos;

        public ICommand AvancarPaginaCommand { get; }
        public ICommand VoltarPaginaCommand { get; }
        public ICommand IrParaPrimeiraPaginaCommand { get; }
        public ICommand IrParaUltimaPaginaCommand { get; }

        public PaginacaoModel()
        {
            TotalPorPagina = 20;
            _paginaAtual = 1;

            AvancarPaginaCommand = new RelayCommand(AvancarPagina, PodeAvancarPagina);
            VoltarPaginaCommand = new RelayCommand(VoltarPagina, PodeVoltarPagina);
            IrParaPrimeiraPaginaCommand = new RelayCommand(IrParaPrimeiraPagina, PodeIrParaPrimeiraPagina);
            IrParaUltimaPaginaCommand = new RelayCommand(IrParaUltimaPagina, PodeIrParaUltimaPagina);
        }

        public Func<int, int, (List<T>, int)> ObterItens { get; set; }

        public void AtualizarPagina()
        {
            var (itens, totalRegistros) = ObterItens(PaginaAtual, TotalPorPagina);

            if (itens != null)
                ListaPorPagina = new ObservableCollection<T>(itens);
            else
                ListaPorPagina = new ObservableCollection<T>();

            TotalRegistos = totalRegistros;
            TotalPaginas = totalRegistros;
            RaisePropertyChanged(nameof(ListaPorPagina));
        }

        public ObservableCollection<T> ListaPorPagina { get; set; }

        public int TotalPorPagina { get; set; }

        public int TotalRegistos
        {
            get => _totalRegistos;
            set
            {
                _totalRegistos = value;
                RaisePropertyChanged(nameof(TotalRegistos));
            }
        }

        public int PaginaAtual
        {
            get => _paginaAtual;
            set
            {
                _paginaAtual = value;
                RaisePropertyChanged(nameof(PaginaAtual));
            }
        }

        public int TotalPaginas
        {
            get => _totalPaginas;
            private set
            {
                _totalPaginas = (int)Math.Ceiling((double)value / TotalPorPagina); // Calcula o total de páginas
                RaisePropertyChanged(nameof(TotalPaginas));
            }
        }

        public void AvancarPagina()
        {
            if (PaginaAtual < TotalPaginas)
            {
                PaginaAtual++;
                AtualizarPagina();
            }
        }

        public bool PodeAvancarPagina() => PaginaAtual < TotalPaginas;

        public void VoltarPagina()
        {
            if (PaginaAtual > 1)
            {
                PaginaAtual--;
                AtualizarPagina();
            }
        }

        public bool PodeVoltarPagina() => PaginaAtual > 1;

        public void IrParaPrimeiraPagina()
        {
            PaginaAtual = 1;
            AtualizarPagina();
        }
        public bool PodeIrParaPrimeiraPagina() => PaginaAtual > 1;

        public void IrParaUltimaPagina()
        {
            PaginaAtual = TotalPaginas;
            AtualizarPagina();
        }

        public bool PodeIrParaUltimaPagina() => PaginaAtual < TotalPaginas;
    }
}
