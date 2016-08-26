namespace DevExplorer.DataServices {
    using System.Collections.Generic;

    public abstract class QueryFactory<T> : BaseFactory<IQueryCriteria<T>, IQuery<T>>, IQueryFactory<T> {
        IQuery<T> IQueryFactory<T>.GetQuery(IQueryCriteria<T> criteria) {
            return GetEntry(criteria, Empty.Instance);
        }
        sealed class Empty : IQuery<T> {
            readonly static T[] EmptyResult = new T[] { };
            internal readonly static IQuery<T> Instance = new Empty();
            Empty() { }
            IEnumerable<T> IQuery<T>.Execute() {
                return EmptyResult;
            }
        }
    }
}