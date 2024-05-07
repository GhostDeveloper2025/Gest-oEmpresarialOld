using System;
using System.Collections.Generic;

namespace GestãoEmpresarial.Models
{
    public class DataGridVendaModel
    {
        public DataGridVendaModel(VendaModel model, Dictionary<int, string> TipoPagamentoList)
        {
            IdVenda = model.IdVenda;
            NomeCliente = GetNome(model.Cliente);
            TipoPagamento = TipoPagamentoList[model.IdCodigoTipoPagamento];
            NomeCadastrante = GetNome(model.Cadastrante);
            DataVenda = model.DataVenda;
            DataFinalizacao = model.DataFinalizacao;
        }

        private string GetNome(ColaboradorModel model)
        {
            if (model == null) return null;
            else return model.Nome;
        }
        private string GetNome(ClienteModel model)
        {
            if (model == null) return null;
            else return model.Nome;
        }

        public int IdVenda { get; set; }
        //public int IdCodigoTipoPagamento { get; set; }
        public string NomeCliente { get; set; }
        public string TipoPagamento { get; set; }
        public string NomeCadastrante { get; set; }
        public bool Finalizado { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataFinalizacao { get; set; }
    }
}
