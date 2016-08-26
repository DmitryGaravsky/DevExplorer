namespace DevExplorer.DataServices {
    using System.Linq;
    using System.Collections.ObjectModel;
    using DevExplorer.DataModel;

    sealed class FolderItemsQueryFactory : QueryFactory<FolderItem> {
        public FolderItemsQueryFactory() {
            RegisterEntry<IFolderItemsQueryCriteria>(criteria => new FolderItemsQuery(criteria));
        }
    }
    //
    public static class FolderItemsQueries {
        public static ReadOnlyCollection<FolderItem> GetFiles(this IQueryFactory<FolderItem> queryFactory, Folder folder) {
            return GetFiles(queryFactory, (folder != null) ? folder.Path : null);
        }
        public static ReadOnlyCollection<FolderItem> GetFiles(this IQueryFactory<FolderItem> queryFactory, string path) {
            var folderItems = queryFactory
                .GetQuery(new FolderItemsQueryCriteria() { Path = path })
                .Execute();
            return new ReadOnlyCollection<FolderItem>(folderItems.ToList());
        }
    }
}