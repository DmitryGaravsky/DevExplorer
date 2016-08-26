namespace DevExplorer.DataModel.Services {
    public interface IImagesCache {
        object GetImage(string path);
        object GetLargeImage(string path);
        void Reset();
    }
}