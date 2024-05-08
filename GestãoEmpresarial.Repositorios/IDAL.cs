using System.Collections.Generic;

namespace GestãoEmpresarial.Interface
{
    /// <summary>
    ///     Interface (contrato) para classes DAO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDAL<T>
    {
        int Insert(T t);

        void Update(T t);

        void Delete(T t);

        List<T> List(string filtro);

        T GetById(int id);
    }
}
