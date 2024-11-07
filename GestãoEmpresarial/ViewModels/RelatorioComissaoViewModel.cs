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
            BuscarComissaoCommand = new RelayCommand(BuscarComissao);
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

        public decimal TotalVendido { get { return ListaComissaoVendas.Sum(a => a.TotalVenda);  } }
        public decimal TotalComissao { get { return ListaComissaoVendas.Sum(a => a.ValComissao);  } }

        public ObservableCollection<RelatorioComissaoVendasModel> ListaComissaoVendas { get; }

        public ICommand BuscarComissaoCommand { get; }

        private void BuscarComissao()
        {
            var comissaoVendas = _relatorioComissaoDAL.ObterComissaoVendas(ColaboradorSelecionado?.IdFuncionario, DataInicial, DataFinal);

            ListaComissaoVendas.Clear();
            foreach (var item in comissaoVendas)
            {
                ListaComissaoVendas.Add(item);
            }
            RaisePropertyChanged(nameof(TotalComissao));
            RaisePropertyChanged(nameof(TotalVendido));
        }
    }
}
