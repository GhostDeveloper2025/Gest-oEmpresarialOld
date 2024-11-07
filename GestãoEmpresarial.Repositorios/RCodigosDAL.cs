using MySql.Data.MySqlClient;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Helpers;
using System.Collections.Generic;
using System.Linq;
using GestãoEmpresarial.Models;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RCodigosDAL : DatabaseConnection, IDAL<CodigoModel>
    {
        private const string CodigoGrupoStatusOS = "STATUS_OS";

        public RCodigosDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public async Task DeleteAsync(CodigoModel t)
        {
            // Não faz nada por enquanto, mas mantém a assinatura assíncrona
            await Task.CompletedTask;
        }

        public async Task<int> InsertAsync(CodigoModel t)
        {
            // Retorna 0, como você mencionou
            await Task.CompletedTask;
            return 0;
        }

        public async Task UpdateAsync(CodigoModel t)
        {
            // Não faz nada por enquanto, mas mantém a assinatura assíncrona
            await Task.CompletedTask;
        }

        public async Task<CodigoModel> GetByIdAsync(int id)
        {
            var list = await ListAsync(id.ToString());
            return list.FirstOrDefault();
        }

        public async Task<bool> PodeApagarItemAsync(int status)
        {
            if (status == 0)
                return false;

            var lista = await ListAsync(CodigoGrupoStatusOS, null);
            var statusAtual = lista.SingleOrDefault(a => a.Id == status);

            if (statusAtual == null)
                return false;

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

        public async Task<CodigoModel> GetStatusOrcamentoConcluidoAsync()
        {
            var list = await ListAsync(CodigoGrupoStatusOS, "ORÇAMENTO CONCLUÍDO");
            return list.First();
        }
       
        public async Task<CodigoModel> GetStatusAbertaAsync()
        {
            var list = await ListAsync(CodigoGrupoStatusOS, "O.S ABERTA");
            return list.First();
        }


        //public async Task<CodigoModel> GetStatusProntoParaConsertoAsync()
        //{
        //    var list = await ListAsync(CodigoGrupoStatusOS, "PRONTO PARA CONSERTO");
        //    return list.FirstOrDefault(); // Usar FirstOrDefault para evitar exceções se a lista estiver vazia
        //}


        public async Task<List<CodigoModel>> GetListaStatusAsync()
        {
            return await ListAsync(CodigoGrupoStatusOS);
        }


        public async Task<List<CodigoModel>> GetListaMarcasFerramentaAsync()
        {
            return await ListAsync("MARCAS_F");
        }


        public async Task<List<CodigoModel>> GetListaTiposPagamentosAsync()
        {
            return await ListAsync("TIPO_PAGAMENTO");
        }


        public async Task<List<CodigoModel>> ListAsync(string filtro)
        {
            return await ListAsync(filtro, null); // Chama o método assíncrono existente
        }

        public async Task<List<CodigoModel>> ListaStatusSeguintesAsync(int statusId)
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

        private async Task<List<CodigoModel>> ListAsync(string codigoGrupo, string codigo = null)
        {
            List<CodigoModel> list = new List<CodigoModel>();
            string query = "SELECT a.Id, a.Nome, a.Descricao FROM config_tb_codigos a "
                + " INNER JOIN config_tb_codigos_grupo b ON a.idcodigogrupo = b.id"
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

        public CodigoModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}

