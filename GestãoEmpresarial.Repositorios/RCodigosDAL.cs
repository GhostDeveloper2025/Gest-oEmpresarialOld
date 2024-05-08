using MySql.Data.MySqlClient;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Helpers;
using System.Collections.Generic;
using System.Linq;
using GestãoEmpresarial.Models;

namespace GestãoEmpresarial.Repositorios
{
    public class RCodigosDAL : DatabaseConnection, IDAL<CodigoModel>
    {
        private const string CodigoGrupoStatusOS = "STATUS_OS";

        public RCodigosDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public void Delete(CodigoModel t)
        {
        }

        public int Insert(CodigoModel t)
        {
            return 0;
        }

        public void Update(CodigoModel t)
        {
        }

        public CodigoModel GetById(int id)
        {
            return List(id.ToString()).FirstOrDefault();
        }

        public bool PodeApagarItem(int status)
        {
            if (status == 0)
                return false;
            else
            {
                var lista = List(CodigoGrupoStatusOS, null);
                var statusAtual = lista.Single(a => a.Id == status);
                switch (statusAtual.Nome)
                {
                    case "ORÇAMENTO CONCLUÍDO":
                    case "O.S ABERTA":
                    case "":
                        return true;

                    default:
                        return false;
                }
            }
        }

        public CodigoModel GetStatusAberta()
        {
            return List(CodigoGrupoStatusOS, "O.S ABERTA").First();
        }

        public CodigoModel GetStatusProntoParaConserto()
        {
            return List(CodigoGrupoStatusOS, "PRONTO PARA CONSERTO").First();
        }

        public List<CodigoModel> GetListaStatus()
        {
            return List(CodigoGrupoStatusOS);
        }

        public List<CodigoModel> GetListaMarcasFerramenta()
        {
            return List("MARCAS_F");
        }

        public List<CodigoModel> GetListaTiposPagamentos()
        {
            return List("TIPO_PAGAMENTO");
        }

        // Overload - o metodo tem o mesmo nome e retorno mas tem o numero de parametros diferentes
        public List<CodigoModel> List(string filtro)
        {
            return List(filtro, null);
        }

        public List<CodigoModel> ListaStatusSeguintes(int statusId)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "statusId", statusId);
            List<CodigoModel> list = new List<CodigoModel>();
            using (MySqlDataReader reader = ExecuteReader("usp_sel_status_os", lista.ToArray(), true))
            {
                while (reader.Read())
                {
                    list.Add(new CodigoModel()
                    {
                        Id = reader.GetInt32("IdCodigo"),
                        Nome = DALHelper.GetString(reader, "NomeCodigo"),
                        Descricao = DALHelper.GetString(reader, "DescricaoCodigo"),

                    });
                }
            }
            return list;
        }

        public List<CodigoModel> List(string codigoGrupo, string codigo)
        {
            List<CodigoModel> list = new List<CodigoModel>();
            string query = "SELECT a.Id, a.Nome, a.Descricao FROM config_tb_codigos a "
                + " inner join config_tb_codigos_grupo b ON a.idcodigogrupo = b.id"
                + " WHERE (@codigoGrupo IS NULL OR b.Nome = @codigoGrupo)"
                + " AND (@codigo IS NULL OR a.Nome = @codigo)";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@codigoGrupo", codigoGrupo);
            AddParameter(lista, "@codigo", codigo);
            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                while (reader.Read())
                {
                    list.Add(new CodigoModel()
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        Descricao = DALHelper.GetString(reader, "Descricao"),

                    });
                }
            }
            return list;
        }
    }
}
