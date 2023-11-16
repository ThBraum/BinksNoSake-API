using BinksNoSake.Domain.Models;
using BinksNoSake.Persistence.Contratos;

namespace BinksNoSake.Persistence.Persistence;
public class GeralPersist : IGeralPersist
{
    private readonly BinksNoSakeContext _context;
    public GeralPersist(BinksNoSakeContext context)
    {
        _context = context;

    }

    public void Add<T>(T entity) where T : class
    {
        _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Remove(entity);
    }

    public void DeleteRange<T>(T[] entityArray) where T : class
    {
        _context.RemoveRange(entityArray);
    }

    public void Update<T>(T entity) where T : class
    {
        _context.Update(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            if (_context.ChangeTracker.HasChanges())
            {
                return (await _context.SaveChangesAsync()) > 0;
            }

            return false;
        }
        catch (Exception e)
        {
            throw new Exception (e.Message); 
        }
    }


}