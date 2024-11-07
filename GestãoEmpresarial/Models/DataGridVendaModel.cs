using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        NomeCadastrante = GetNome(model.Cadastrante);
        DataVenda = model.DataVenda;
        DataFinalizacao = model.DataFinalizacao;
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

    [DisplayName("Cadastrante")]
    public string NomeCadastrante { get; set; }

    public bool Finalizado { get; set; }

    [DisplayName("Data Venda")]
    public DateTime DataVenda { get; set; }

    [DisplayName("Data Finalização")]
    public DateTime? DataFinalizacao { get; set; }
}
