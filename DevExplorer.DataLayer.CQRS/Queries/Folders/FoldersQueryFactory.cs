namespace DevExplorer.DataServices {
    using System.Linq;
    using System.Collections.ObjectModel;
    using DevExplorer.DataModel;

    sealed class FoldersQueryFactory : QueryFactory<Folder> {
        public FoldersQueryFactory() {
            RegisterEntry<IRecentFoldersQueryCriteria>(criteria => new RecentFoldersQuery(criteria));
        }
    }
    //
    public static class FolderQueries {
        public static ReadOnlyCollection<Folder> GetRecentFolders(this IQueryFactory<Folder> queryFactory, int? top = null, bool? includeSpecial = null) {
            var recentFolders = queryFactory
                .GetQuery(new RecentFoldersQueryCriteria() { Top = top, IncludeSpecial = includeSpecial })
                .Execute();
            return new ReadOnlyCollection<Folder>(recentFolders.ToList());
        }
    }
}