using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFBulkOp.FinalProjector
{

    public abstract class ModelConvertibleBase<TModel, TEntity> where TModel : ModelConvertibleBase<TModel, TEntity>, new()
    {
        public static TModel FromEntity(TEntity entity)
        {
            var model = new TModel();
            model.FromEntityToThisModel(entity);
            return model;
        }

        protected abstract void FromEntityToThisModel(TEntity entity);

        public abstract TEntity ToEntity();
    }

    public static class EFExtensions
    {
        public static IQueryable<TM> AfterProject<TE, TM>(this IQueryable<TE> query) where TM : ModelConvertibleBase<TM, TE>, new()
        {
            return new QueyableFinalProjection<TE, TM>(query);
        }
    }

    public class QueyableFinalProjection<TS, TD> : IQueryable<TD> where TD : ModelConvertibleBase<TD, TS>, new()
    {
        private readonly IQueryable<TS> _source;
        public QueyableFinalProjection(IQueryable<TS> source) => _source = source;

        public IEnumerator<TD> GetEnumerator()
        {
            return new FinalProjectionEnumerator<TS, TD>(_source.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression
        {
            get
            {
                return _source.Expression;

            }
        }

        public Type ElementType
        {
            get
            {
                return _source.ElementType;

            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _source.Provider;

            }
        }

        public class FinalProjectionEnumerator<TSe, TDe> : IEnumerator<TDe> where TDe : ModelConvertibleBase<TDe, TSe>, new()
        {
            private readonly IEnumerator<TSe> _sourcEnumerator;

            public FinalProjectionEnumerator(IEnumerator<TSe> sourcEnumerator)
            {
                _sourcEnumerator = sourcEnumerator;
            }

            public bool MoveNext()
            {
                return _sourcEnumerator.MoveNext();
            }

            public void Reset()
            {
                _sourcEnumerator.Reset();
            }

            public TDe Current => ModelConvertibleBase<TDe, TSe>.FromEntity(_sourcEnumerator.Current);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _sourcEnumerator.Dispose();
            }
        }
    }



}
