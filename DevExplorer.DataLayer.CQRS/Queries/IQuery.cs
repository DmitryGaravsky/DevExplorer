namespace DevExplorer.DataServices {
    using System.Collections.Generic;

    public interface IQuery<T> {
        IEnumerable<T> Execute();
    }
    //
    public interface IQueryCriteria<T> { }
    public interface IQueryFactory<T> {
        IQuery<T> GetQuery(IQueryCriteria<T> criteria);
    }
}