using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess;

public interface IRepository<T> where T: BaseEntity
{
    IQueryable<T> GetAll();
    T? GetById(int id);
    T? GetById(Guid id);
    T Save(T entity);
    bool Delete(T entity);
    bool Delete(Guid id);
    bool Delete(int id);
}