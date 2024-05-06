using System;
using GestãoEmpresarial.Models.Atributos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    //    public class ItensVendaModel : ObservableObject
    //    {
    //        public int IdVenda { get; set; }
    //        public int IdItensVenda { get; set; }
    //        private int _quantidade;


    //        //public string NomeProduto
    //        //{
    //        //    get { return _produto?.Nome; }
    //        //}

    //        //[ColumnHeader("Código")]
    //        //public string CodigoProduto
    //        //{
    //        //    get { return _produto?.CodProduto; }
    //        //}
    //        [ColumnHeader("Quantidade")]
    //        public int Quantidade
    //        {
    //            get { return _quantidade; }
    //            set
    //            {
    //                _quantidade = value;
    //                RaisePropertyChanged(nameof(Quantidade));
    //                RaisePropertyChanged(nameof(TotalItem));
    //            }
    //        }
    //        private decimal _valUnitario;
    //        [ColumnHeader("Valor Unitario")]
    //        public decimal ValUnitario
    //        {
    //            get { return _valUnitario; }
    //            set
    //            {
    //                _valUnitario = value;
    //                RaisePropertyChanged(nameof(ValUnitario));
    //                RaisePropertyChanged(nameof(TotalItem));
    //            }
    //        }

    //        public decimal _Desconto;
    //        [ColumnHeader("Desconto")]
    //        public decimal Desconto
    //        {
    //            get { return _Desconto; }
    //            set
    //            {
    //                _Desconto = value;
    //                RaisePropertyChanged(nameof(Desconto));
    //                RaisePropertyChanged(nameof(TotalItem));
    //            }
    //        }
    //        public decimal CustoTotal { get { return Quantidade * ValUnitario; } } // => ValTotal
    //        public decimal DescontoValor { get { return Desconto <= 0 ? 0 : CustoTotal * Desconto / 100; } }

    //        [ColumnHeader("Total Item")]
    //        //public decimal TotalItem { get { return CustoTotal - DescontoValor; } }
    //        public decimal TotalItem { get { return Math.Round(CustoTotal - DescontoValor, 2); } }

    //        private ProdutoModel _produto;
    //        public ProdutoModel Produto
    //        {
    //            get { return _produto; }
    //            set
    //            {
    //                _produto = value;
    //                if (_produto != null)
    //                    ValUnitario = _produto.ValorVenda;
    //                RaisePropertyChanged(nameof(Produto));
    //            }
    //        }
    //    }
}
