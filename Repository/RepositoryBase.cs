using Contraacts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    //RepositoryBase contain all common functionality of child repositorries
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext _RepositoryContext;

        public RepositoryBase(RepositoryContext RepositoryContext)
        {
            _RepositoryContext = RepositoryContext;
        }


        //Create
        public void Create(T entity) => _RepositoryContext.Set<T>().Add(entity);

        //Read
        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _RepositoryContext.Set<T>().AsNoTracking() : _RepositoryContext.Set<T>();


        public IQueryable<T> FindByCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression, bool trackChanges)
        =>
            !trackChanges ? _RepositoryContext.Set<T>().Where(expression).AsNoTracking()
            : _RepositoryContext.Set<T>().Where(expression);

        //Update
        public void Update(T entity) => _RepositoryContext.Set<T>().Update(entity);
        //Delete
        public void Delete(T entity) => _RepositoryContext.Set<T>().Remove(entity);
    }
}

//Set<T> is a method in the DbContext class in Entity Framework that provides access to the entities of
//a particular type T in a database.

//When using Entity Framework, a DbContext instance represents a session with the database, and it can be
//used to query, insert, update, and delete data. The Set<T> method is one of the main entry points for
//performing these operations.

//Set<T> returns an IDbSet<T> instance, which is a collection of entities of the specified type T.
//The IDbSet<T> interface provides a set of methods for querying and modifying the entities in the collection.

//When Set<T> is called on a DbContext instance, Entity Framework will use reflection to generate a
//queryable representation of the database table or view that corresponds to the specified entity type T.
//This means that any LINQ expression that operates on the IDbSet<T> instance returned by Set<T> is
//translated by Entity Framework into an SQL query that is executed against the underlying database.

//In summary, Set<T> is a method in the DbContext class in Entity Framework that returns a collection
//of entities of the specified type T, and allows querying, inserting, updating, and deleting data using
//LINQ and other methods provided by IDbSet<T>.
