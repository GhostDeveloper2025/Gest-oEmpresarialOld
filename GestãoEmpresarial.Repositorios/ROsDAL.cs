using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class ROsDAL : DatabaseConnection, IDAL<OrdemServicoModel>
    {
        internal readonly RColaboradorDAL rColaboradorDAL;
        internal readonly RClienteDAL rClienteDAL;
        internal readonly RItensOSDAL rItensOSDAL;

        public ROsDAL(int idFuncionario) : base(idFuncionario)
        {
            rColaboradorDAL = new RColaboradorDAL(idFuncionario);
            rItensOSDAL = new RItensOSDAL(idFuncionario);
            rClienteDAL = new RClienteDAL(idFuncionario);
        }

        public async Task<bool> PodeEditarAsync(int statusId)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "statusId", statusId);
            object existe = ExecuteScalar("usp_pode_editar_ordem_servico", lista.ToArray(), true);
            return Convert.ToBoolean(existe);
        }

        public async Task DeleteAsync(OrdemServicoModel t)
        {
            string query = "DELETE FROM tb_os WHERE IdOs = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdOs, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr); // Executa de forma assíncrona
        }

        public async Task<OrdemServicoModel> GetByIdAsync(int id)
        {
            var lista = await GetListaAsync(true, null, null, null, id);
            return lista.FirstOrDefault();
        }

        public async Task<int> InsertAsync(OrdemServicoModel t)
        {
            string query = "INSERT INTO tb_os (DataEntrada, DataFinalizacao, IdCadastrante, IdCliente, Finalizado, IdCodigoStatus, Ferramenta, IdCodigoMarcasF, Modelo, Obs, IdTecnico, IdResponsavel, TotalMaoObra, Box, Garantia, SubTotalProduto, DescontoProduto, TotalProduto, TotalOS) "
            + " VALUES(NOW(), @DataFinalizacao, @IdCadastrante, @IdCliente, @Finalizado, @Status, @Ferramenta, @Marca, @Modelo, @Obs, @IdTecnico, @IdResponsavel, @TotalMaoObra, @Box, @Garantia, @SubTotalProduto, @DescontoProduto, @TotalProduto, @TotalOS);"
            + " SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@DataFinalizacao", t.DataFinalizacao);
            AddParameter(lista, "@IdCadastrante", idFuncionario);
            AddParameter(lista, "@IdCliente", t.Cliente.Idcliente);
            AddParameter(lista, "@Finalizado", t.Finalizado);
            AddParameter(lista, "@Status", t.Status);
            AddParameter(lista, "@Ferramenta", t.Ferramenta);
            AddParameter(lista, "@Marca", t.Marca);
            AddParameter(lista, "@Modelo", t.Modelo);
            AddParameter(lista, "@Obs", t.Obs);
            AddParameter(lista, "@IdTecnico", t.IdTecnico);
            AddParameter(lista, "@IdResponsavel", t.IdResponsavel);
            AddParameter(lista, "@TotalMaoObra", t.TotalMaoObra);
            AddParameter(lista, "@Box", t.Box);
            AddParameter(lista, "@Garantia", t.Garantia);
            AddParameter(lista, "@SubTotalProduto", t.SubTotalProduto);
            AddParameter(lista, "@DescontoProduto", t.TotalDescontoProduto);
            AddParameter(lista, "@TotalProduto", t.TotalProduto);
            AddParameter(lista, "@TotalOS", t.TotalOS);

            object id = ExecuteScalar(query, lista.ToArray()); // Execução assíncrona
            return Convert.ToInt32(id);
        }

        public async Task<List<OrdemServicoModel>> ListAsync(string nomeCliente, int? idStatus, DateTime? dataEntrada, int? idOs)
        {
            return await GetListaAsync(false, nomeCliente, idStatus, dataEntrada, idOs);
        }

        //Este codigo Foi Uma melhoria que o Chat GPT me ajudou a Criar
        /// <summary>
        /// *   Coleta de IDs de colaboradores: A lógica agora coleta todos os IDs de colaboradores antes de entrar no loop,
        ///       evitando múltiplas chamadas assíncronas.
        ///     
        ///  *   Chamada única para obter colaboradores: Uma vez que todos os IDs foram coletados, uma única chamada é feita
        ///        para obter todos os colaboradores correspondentes.
        ///        
        ///  *   Atribuição de colaboradores: Depois de obter a lista de colaboradores, a lógica atribui cada colaborador ao
        ///       respectivo produto.
        ///       
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<OrdemServicoModel>> GetListaAsync(bool comItens, string nomeCliente, int? idStatus, DateTime? dataEntrada, int? idOs)
        {
            var parametros = new List<MySqlParameter>();
            var conditions = new List<string>();
            AddParameterCondition(parametros, conditions, "IdOS", idOs);
            AddParameterCondition(parametros, conditions, "DataEntrada", dataEntrada);
            AddParameterCondition(parametros, conditions, "idcodigostatus", idStatus);

            if (string.IsNullOrWhiteSpace(nomeCliente) == false)
            {
                AddParameter(parametros, "@nomeCliente", nomeCliente);
                conditions.Add("Idcliente IN (SELECT idcliente FROM tb_cliente WHERE nome LIKE CONCAT(@nomeCliente, '%'))");
            }

            string conditionsJoin = string.Join(" OR ", conditions);
            string query = $@"SELECT IdOs, DataEntrada, DataFinalizacao, IdCadastrante, IdCliente, Finalizado, Ferramenta, Modelo, Obs, IdTecnico,
                 IdResponsavel, TotalMaoObra, Box, Garantia, SubTotalProduto, DescontoProduto, TotalProduto, TotalOS, idcodigostatus, idcodigomarcasf
                 FROM tb_os WHERE {conditionsJoin} LIMIT 200";  // Adicionando LIMIT 200 para limitar o número de registros

            List<OrdemServicoModel> lista = new List<OrdemServicoModel>();
            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                // HashSets para armazenar os IDs únicos de clientes, cadastrantes, responsáveis e técnicos
                var idsClientes = new HashSet<int>();
                var idsCadastrantes = new HashSet<int>();
                var idsResponsaveis = new HashSet<int>();
                var idsTecnicos = new HashSet<int>();
                var idsOs = new HashSet<int>();

                // Ler dados e coletar os IDs para consultas em lote
                while (await reader.ReadAsync())
                {
                    var obj = Mapeador.Map(new OrdemServicoModel(), reader);

                    if (comItens)
                    {
                        idsOs.Add(obj.IdOs); // Coletar IDs das ordens de serviço para os itens
                    }

                    idsClientes.Add(obj.IdCliente);
                    idsCadastrantes.Add(obj.IdCadastrante);

                    if (obj.IdResponsavel.HasValue)
                    {
                        idsResponsaveis.Add(obj.IdResponsavel.Value);
                    }

                    if (obj.IdTecnico.HasValue)
                    {
                        idsTecnicos.Add(obj.IdTecnico.Value);
                    }

                    lista.Add(obj);
                }

                // Consultas em lote para buscar os clientes, cadastrantes, responsáveis e técnicos
                var clientes = await rClienteDAL.GetByIdsAsync(idsClientes.ToList());
                var cadastrantes = await rColaboradorDAL.GetByIdsAsync(idsCadastrantes.ToList());
                var responsaveis = await rColaboradorDAL.GetByIdsAsync(idsResponsaveis.ToList());
                var tecnicos = await rColaboradorDAL.GetByIdsAsync(idsTecnicos.ToList());

                // Se comItens for true, buscar os itens das ordens de serviço em uma única chamada
                Dictionary<int, List<ItemOrdemServicoModel>> itensMap = new Dictionary<int, List<ItemOrdemServicoModel>>();
                if (comItens && idsOs.Any())
                {
                    itensMap = await rItensOSDAL.GetByMultipleIdsOsAsync(idsOs.ToList());
                }

                // Atribuir os valores retornados às respectivas ordens de serviço
                foreach (var ordem in lista)
                {
                    ordem.Cliente = clientes.FirstOrDefault(c => c.Idcliente == ordem.IdCliente);
                    ordem.Cadastrante = cadastrantes.FirstOrDefault(c => c.IdFuncionario == ordem.IdCadastrante);
                    ordem.Responsavel = ordem.IdResponsavel.HasValue
                                        ? responsaveis.FirstOrDefault(r => r.IdFuncionario == ordem.IdResponsavel.Value)
                                        : null;
                    ordem.Tecnico = ordem.IdTecnico.HasValue
                                        ? tecnicos.FirstOrDefault(t => t.IdFuncionario == ordem.IdTecnico.Value)
                                        : null;

                    // Atribuir itens se comItens for true
                    if (comItens && itensMap.TryGetValue(ordem.IdOs, out var itensOs))
                    {
                        ordem.ListItensOs = itensOs;
                    }
                }
            }

            return lista;
        }

        public async Task UpdateAsync(OrdemServicoModel t)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "id", t.IdOs);
            AddParameter(lista, "statusId", t.Status);
            AddParameter(lista, "obsv", t.Obs);
            AddParameter(lista, "box", t.Box);
            AddParameter(lista, "Garantia", t.Garantia);
            AddParameter(lista, "idTecnico", t.Tecnico?.IdFuncionario);
            AddParameter(lista, "idResponsavel", t.Responsavel?.IdFuncionario);
            AddParameter(lista, "TotalMaoObra", t.TotalMaoObra);
            AddParameter(lista, "SubTotalProduto", t.SubTotalProduto);
            AddParameter(lista, "DescontoProduto", t.TotalDescontoProduto);
            AddParameter(lista, "TotalProduto", t.TotalProduto);
            AddParameter(lista, "TotalOS", t.TotalOS);

            ExecuteNonQuery("usp_upd_ordem_servico", lista.ToArray(), true); // Execução assíncrona
        }

        public OrdemServicoModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}

