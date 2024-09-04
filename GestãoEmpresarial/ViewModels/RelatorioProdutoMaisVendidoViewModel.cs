using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Repositorios.GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioProdutoMaisVendidoViewModel : ObservableObject
    {
        private readonly RRelatorioProdutoMaisVendidoDAL _relatoriosDAL;
        private DateTime? _dataInicial;
        private DateTime? _dataFinal;
        private List<RelatorioProdutoMaisVendido> _listaRelatorioProdutoMaisVendido;

        public RelatorioProdutoMaisVendidoViewModel(RRelatorioProdutoMaisVendidoDAL relatoriosDAL)
        {
            _relatoriosDAL = relatoriosDAL;
            ListaRelatorioProdutoMaisVendido = new List<RelatorioProdutoMaisVendido>(); // Inicialmente vazio
        }

        public DateTime? DataInicial
        {
            get { return _dataInicial; }
            set
            {
                _dataInicial = value;
                RaisePropertyChanged(nameof(DataInicial));
                ObterDadosRelatorio(); // Atualiza a lista ao mudar a data
            }
        }

        public DateTime? DataFinal
        {
            get { return _dataFinal; }
            set
            {
                _dataFinal = value;
                RaisePropertyChanged(nameof(DataFinal));
                ObterDadosRelatorio(); // Atualiza a lista ao mudar a data
            }
        }

        public List<RelatorioProdutoMaisVendido> ListaRelatorioProdutoMaisVendido
        {
            get { return _listaRelatorioProdutoMaisVendido; }
            private set
            {
                _listaRelatorioProdutoMaisVendido = value;
                RaisePropertyChanged(nameof(ListaRelatorioProdutoMaisVendido));
            }
        }

        private void ObterDadosRelatorio()
        {
            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                ListaRelatorioProdutoMaisVendido = _relatoriosDAL.ObterProdutoMaisVendido(DataInicial, DataFinal);
            }
            else
            {
                ListaRelatorioProdutoMaisVendido = new List<RelatorioProdutoMaisVendido>(); // Mantém a lista vazia se as datas não estiverem definidas
            }
        }
    }
}
