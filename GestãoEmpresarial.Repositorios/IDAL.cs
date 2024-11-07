using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Interface
{
    /// <summary>
    /// Interface (contrato) para classes DAO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDAL<T>
    {
        Task<int> InsertAsync(T t);       // Retorna o ID ou número de linhas afetadas
        Task UpdateAsync(T t);            // Atualização assíncrona
        Task DeleteAsync(T t);            // Exclusão assíncrona
        T GetById(int id);

        //Task<T> GetByIdAsync(int id);     // Busca assíncrona por ID
        //Task<List<T>> ListAsync(string filtro);  // Listagem assíncrona com filtro
    }
}
