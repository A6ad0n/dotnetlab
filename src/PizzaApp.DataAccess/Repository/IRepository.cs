using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Repository;

public interface IRepository<T> where T: IBaseEntity
{
    IQueryable<T> GetAll();
    Task<IQueryable<T>> GetAllAsync();
    
    T? GetById(int id);
    Task<T?> GetByIdAsync(int id);

    T Save(T entity);
    Task<T> SaveAsync(T entity);

    bool Delete(int id);
    Task<bool> DeleteAsync(int id);
}