using LangdiDb;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace langdiWeb
{
    public interface IRepository<T, PK> where T : class,new()
    {
        IEnumerable<T> Find(ISpecification<T> spec);
        IEnumerable<T> FindAll();
        void Add(T entity);
        void Remove(T entity);
        void SaveChanges();
        /// <summary>
        /// 从数据库中加载
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        T Load(PK primaryKey);
        IQueryable<T> LoadEntitiesPageList<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc);
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntitiesList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        void Update(T entity);
    }
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);
        Expression<Func<T, bool>> Expression { get; }
    }

    public abstract class Specification<T> : ISpecification<T> where T : class
    {
        public bool IsSatisfiedBy(T obj)
        {
            return this.Expression.Compile()(obj);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }

        public static ISpecification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpec<T>(expression);
        }
    }

    internal class ExpressionSpec<T> : Specification<T> where T : class
    {
        private Expression<Func<T, bool>> exp;

        public ExpressionSpec(Expression<Func<T, bool>> expression)
        {
            this.exp = expression;
        }

        public override Expression<Func<T, bool>> Expression
        {
            get { return this.exp; }
        }

    }
    public class Repository<T, PK> : IRepository<T, PK> where T : class,new()
    {

        private static LangdiDbContext _db = null;
        public LangdiDbContext DbContext
        {
            get
            {
                if (_db == null)
                {
                    _db = new LangdiDbContext();
                }
                return _db;
            }
        }
        public void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }
        public void Remove(T entity)
        {
            //DbContext.Set<T>().Add(entity);
            DbContext.Set<T>().Remove(entity);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public T Load(PK primaryKey)
        {
            return DbContext.Set<T>().Find(new object[] { primaryKey });
        }

        public IEnumerable<T> Find(ISpecification<T> spec)
        {
            return DbContext.Set<T>().Where(spec.Expression).ToList();
        }

        public IEnumerable<T> FindAll()
        {
            return DbContext.Set<T>().ToList();
        }
        public IQueryable<T> LoadEntitiesPageList<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc)
        {
            var list = DbContext.Set<T>().Where(whereLambda);
            totalCount = list.Count();
            if (isAsc)//升序
            {
                list = list.OrderBy<T, s>(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else//降序
            {
                list = list.OrderByDescending<T, s>(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return list;
        }
        public IQueryable<T> LoadEntitiesList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where<T>(whereLambda);
        }
        public void Update(T entity)
        {
            DbContext.Entry<T>(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
    }
}