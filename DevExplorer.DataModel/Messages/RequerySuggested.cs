namespace DevExplorer.DataServices {
    public sealed class RequerySuggested<T> {
        public RequerySuggested()
            : this(default(T)) {
        }
        public RequerySuggested(T entry) {
            Entry = entry;
        }
        public bool IsSingleEntry {
            get { return Entry != null; }
        }
        public T Entry {
            get;
            private set;
        }
    }
}