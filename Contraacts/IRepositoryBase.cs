using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Contraacts
{
    public interface IRepositoryBase<T>
    {
        //Create
        void Create(T entity);
        //Read
        //trackChanges which is used to determine if the returned queryable should track changes
        //made to the entities returned or not.
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        //Update
        void Update(T entity);
        //Delete
        void Delete(T entity);


    }
}
//Queryable is a.NET framework interface in the System.Linq namespace that provides a set of
//extension methods for querying data from different data sources.
//The Queryable interface allows for constructing queries against various
//data sources(e.g., databases, in-memory collections, etc.) using a common syntax, which is similar
//to SQL-like syntax.The interface supports common operations such as filtering, sorting, grouping, and projection.

//Queryable is a powerful interface that enables LINQ(Language Integrated Query) technology, which
//allows developers to write queries in their programming languages(such as C# or VB.NET) rather than
//writing raw SQL statements. Queries constructed using Queryable are translated by the underlying
//data provider (e.g., Entity Framework) into optimized SQL statements, which are executed against
//the data source to retrieve the desired data.

//The IQueryable<T> interface is an extension of IEnumerable<T> and provides additional query
//capabilities.IQueryable<T> allows for the deferred execution of queries, meaning that the query
//is not executed until the data is actually enumerated or materialized, which helps to optimize
//performance by allowing the data provider to translate the query into the most efficient form.
