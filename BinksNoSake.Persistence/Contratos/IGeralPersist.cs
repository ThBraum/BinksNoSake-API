namespace BinksNoSake.Persistence.Contratos;
public interface IGeralPersist
{
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(T[] entityArray) where T : class;
    void Detach<T>(T entity) where T : class;
    Task<bool> SaveChangesAsync();
}