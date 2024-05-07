using System;
using System.Collections.Generic;

namespace GestãoEmpresarial.Models
{
    public class DataGridOrdemServicoModel
    {
        public DataGridOrdemServicoModel(OsModel model, Dictionary<int, string> StatusList)
        {
            IdOs = model.IdOs;
            NomeCliente = GetNome(model.Cliente);
            Status = StatusList[model.Status];
            NomeTecnico = GetNome(model.Tecnico);
            //NomeResponsavel = GetNome(model.Responsavel);
            NomeCadastrante = GetNome(model.Cadastrante);
            //Box = model.Box;
            Finalizado = model.Finalizado;
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

        public int IdOs { get; set; }

        //[ColumnHeader("Nome Cliente")]
        public string NomeCliente { get; set; }

        public string Status { get; set; }
        public string NomeTecnico { get; set; }

        //public string NomeResponsavel { get; set; }
        public string NomeCadastrante { get; set; }

        //public string Box { get; set; }
        public bool Finalizado { get; set; }

        public bool Garantia { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataFinalizacao { get; set; }
    }
}