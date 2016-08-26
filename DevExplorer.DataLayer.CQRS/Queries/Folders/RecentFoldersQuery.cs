namespace DevExplorer.DataServices {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using DevExplorer.DataModel;

    public interface IRecentFoldersQueryCriteria : IQueryCriteria<Folder> {
        int? Top { get; }
        bool? IncludeSpecial { get; }
    }
    //
    sealed class RecentFoldersQueryCriteria : IRecentFoldersQueryCriteria {
        public const int DefaultTop = 8;
        public int? Top {
            get;
            set;
        }
        public bool? IncludeSpecial {
            get;
            set;
        }
    }
    sealed class RecentFoldersQuery : IQuery<Folder> {
        readonly IRecentFoldersQueryCriteria criteria;
        public RecentFoldersQuery(IRecentFoldersQueryCriteria criteria) {
            this.criteria = criteria;
        }
        public IEnumerable<Folder> Execute() {
            int top = criteria.Top.GetValueOrDefault(RecentFoldersQueryCriteria.DefaultTop);
            bool includeSpecial = criteria.IncludeSpecial.GetValueOrDefault(true);
            Func<Folder, bool> skipSpecial = f => includeSpecial | !f.IsSpecial();
            return GetFolders()
                .Where(skipSpecial)
                .Take(top);
        }
        readonly static Environment.SpecialFolder[] specialFolders = new Environment.SpecialFolder[] { 
            Environment.SpecialFolder.Desktop,
            Environment.SpecialFolder.ProgramFiles
        };
        IEnumerable<Folder> GetFolders() {
            yield return NewFolder.Instance;
            foreach(var item in specialFolders) {
                Folder special;
                if(item.TryGetSpecialFolder(out special))
                    yield return special;
            }
        }
    }
}