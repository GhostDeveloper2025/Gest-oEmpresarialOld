using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class DataGridVendaModel
{
    public DataGridVendaModel(VendaModel model, Dictionary<int, string> TipoPagamentoList)
    {
        IdVenda = model.IdVenda;
        NomeCliente = GetNome(model.Cliente);

        // Verificar se o TipoPagamento existe no dicionário, se não, definir como "Não informado"
        if (!TipoPagamentoList.TryGetValue(model.IdCodigoTipoPagamento, out string tipoPagamento))
        {
            tipoPagamento = "Não informado";
        }
        TipoPagamento = tipoPagamento;

        NomeVendedor = GetNome(model.Vendedor);
        DataVenda = model.DataVenda;
        DataFinalizacao = model.DataFinalizacao;
        //TotalVenda = model.CustoVenda;
    }

    private string GetNome(ColaboradorModel model)
    {
        return model?.Nome;
    }

    private string GetNome(ClienteModel model)
    {
        return model?.Nome;
    }
   
    [DisplayName("N° Venda")]
    public int IdVenda { get; set; }

    [DisplayName("Cliente")]
    public string NomeCliente { get; set; }

    public string TipoPagamento { get; set; }

    [DisplayName("Vendedor")]
    // public string NomeCadastrante { get; set; }
    public string NomeVendedor { get; set; }

    public bool Finalizado { get; set; }

    [DisplayName("Data Venda")]
    public DateTime DataVenda { get; set; }

    [DisplayName("Data Finalização")]
    public DateTime? DataFinalizacao { get; set; }

    //[DisplayName("Total de Venda")]
    //public decimal? TotalVenda { get; set; }
}
