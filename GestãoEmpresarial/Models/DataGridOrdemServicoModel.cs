using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GestãoEmpresarial.Models
{
    public class DataGridOrdemServicoModel
    {
        public DataGridOrdemServicoModel(OrdemServicoModel model, Dictionary<int, string> StatusList)
        {
            IdOs = model.IdOs;
            NomeCliente = GetNome(model.Cliente);
            Status = StatusList[model.Status];
            NomeTecnico = GetNome(model.Tecnico);
            //NomeResponsavel = GetNome(model.Responsavel);
            NomeCadastrante = GetNome(model.Cadastrante);
            Box = model.Box;
            Marca = model.Marca;
            Modelo = model.Modelo;
            Ferramenta = model.Ferramenta;
            Finalizado = model.Finalizado;
            TotalOS = model.TotalOS;
            Garantia = model.Garantia;
            DataEntrada = model.DataEntrada;
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
        [DisplayName("N° Os")]
        public int IdOs { get; set; }

        [DisplayName("Cliente")]
        public string NomeCliente { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Ferramenta")]
        public string Ferramenta { get; set; }

        [DisplayName("Marca")]
        public int Marca { get; set; }

        [DisplayName("Modelo")]
        public string Modelo { get; set; }

        [DisplayName("Box")]
        public string Box { get; set; }

        public string NomeTecnico { get; set; }

        [DisplayName("Cadastrante")]
        public string NomeCadastrante { get; set; }

        //public string Box { get; set; }
        public bool Finalizado { get; set; }
        [DisplayName("Total OS")]
        public decimal TotalOS { get; set; }
        public bool Garantia { get; set; }
        [DisplayName("Data Entrada")]
        public DateTime DataEntrada { get; set; }
        [DisplayName("Data Finalização")]
        public DateTime? DataFinalizacao { get; set; }
        



    }
}