using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioProdutoMaisVendidoViewModel : ObservableObject
    {
        private readonly RRelatorioProdutoMaisVendidoDAL _relatoriosDAL;
        private DateTime? _dataInicial;
        private DateTime? _dataFinal;

        public PaginacaoModel<RelatorioProdutoMaisVendido> Paginas { get; set; }

        public RelatorioProdutoMaisVendidoViewModel(RRelatorioProdutoMaisVendidoDAL relatoriosDAL)
        {
            _relatoriosDAL = relatoriosDAL;
            Paginas = new PaginacaoModel<RelatorioProdutoMaisVendido>();
            Paginas.ObterItens = AtualizarRelatorio;
        }
       
        private (List<RelatorioProdutoMaisVendido>, int) AtualizarRelatorio(int pagina, int total)
        {
            if (DataInicial.HasValue && DataFinal.HasValue)
            {
                // Agora o método retorna uma tupla (Itens, TotalRegistros)
                return _relatoriosDAL.ObterProdutoMaisVendidoAsync(DataInicial, DataFinal, pagina, total).GetAwaiter().GetResult();
            }
            else
            {
                return (new List<RelatorioProdutoMaisVendido>(), 0);
            }
        }

        //
        public DateTime? DataInicial
        {
            get { return _dataInicial; }
            set
            {
                _dataInicial = value;
                RaisePropertyChanged(nameof(DataInicial));
                Paginas.AtualizarPagina();
            }
        }

        public DateTime? DataFinal
        {
            get { return _dataFinal; }
            set
            {
                _dataFinal = value;
                RaisePropertyChanged(nameof(DataFinal));
                Paginas.AtualizarPagina();
            }
        }
    }
}
