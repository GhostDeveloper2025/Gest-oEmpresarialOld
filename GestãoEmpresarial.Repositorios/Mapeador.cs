using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    internal class Mapeador
    {
        internal static ColaboradorModel Map(ColaboradorModel model, MySqlDataReader reader)
        {
            model.DataCadastro = DALHelper.GetDateTime(reader, "DataCadastro").GetValueOrDefault();
            model.IdFuncionario = reader.GetInt32("IdFuncionario");
            model.Nome = DALHelper.GetString(reader, "Nome");
            model.CPF = DALHelper.GetString(reader, "CPF");
            model.Telefone = DALHelper.GetString(reader, "Telefone");
            model.Email = DALHelper.GetString(reader, "Email");
            model.Senha = DALHelper.GetString(reader, "Senha");
            model.Cargo = DALHelper.GetString(reader, "Cargo");
            model.Comissao = DALHelper.GetDecimal(reader, "Comissao");
            model.Ativo = DALHelper.GetBool(reader, "Ativo");
            return model;
        }

        internal static VendaModel Map(VendaModel model, MySqlDataReader reader)
        {
            model.IdVenda = reader.GetInt32("IdVenda");
            model.DataVenda = reader.GetDateTime("DataVenda");
            model.DataFinalizacao = DALHelper.GetDateTime(reader, "DataFinalizacao");
            model.IdCodigoTipoPagamento = reader.GetInt32("IdCodigoTipoPagamento");
            model.Situacao = DALHelper.GetBool(reader, "Situacao");
            model.ValorFrete = DALHelper.GetDecimal(reader, "ValorFrete");
            model.IdCliente = DALHelper.GetInt32(reader, "IdCliente").Value;
            model.IdFuncionario = DALHelper.GetInt32(reader, "IdFuncionario").Value;

            model.CustoVenda = DALHelper.GetDecimalNullable(reader, "CustoVenda");

            return model;
        }

        internal static OrdemServicoModel Map(OrdemServicoModel model, MySqlDataReader reader)
        {
            model.IdOs = reader.GetInt32("IdOs");
            model.DataEntrada = reader.GetDateTime("DataEntrada");
            model.DataFinalizacao = DALHelper.GetDateTime(reader, "DataFinalizacao");
            model.Ferramenta = DALHelper.GetString(reader, "Ferramenta");
            model.Modelo = DALHelper.GetString(reader, "Modelo");
            model.Obs = DALHelper.GetString(reader, "Obs");
            model.TotalDescontoProduto = DALHelper.GetDecimal(reader, "DescontoProduto");
            model.SubTotalProduto = DALHelper.GetDecimal(reader, "SubTotalProduto");
            model.TotalProduto = DALHelper.GetDecimal(reader, "TotalProduto");
            model.TotalOS = DALHelper.GetDecimal(reader, "TotalOS");
            model.TotalMaoObra = DALHelper.GetDecimal(reader, "TotalMaoObra");
            model.Box = DALHelper.GetString(reader, "Box");
            model.Finalizado = DALHelper.GetBool(reader, "Finalizado");
            model.Garantia = DALHelper.GetBool(reader, "Garantia");
            model.Status = reader.GetInt32("idcodigostatus");
            model.Marca = DALHelper.GetInt32(reader, "idcodigomarcasf").Value;
            model.IdCliente = DALHelper.GetInt32(reader, "IdCliente").Value;
            model.IdCadastrante = DALHelper.GetInt32(reader, "IdCadastrante").Value;

            model.IdResponsavel = DALHelper.GetInt32(reader, "IdResponsavel");
            model.IdTecnico = DALHelper.GetInt32(reader, "IdTecnico");

            return model;
        }

    }
}
