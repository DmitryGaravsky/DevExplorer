namespace DevExplorer.Services {
    using System.Collections.Concurrent;
    using System.Drawing;
    using DevExpress.Utils.Helpers;

    sealed class FileSystemImagesCache : DataModel.Services.IImagesCache {
        public readonly static DataModel.Services.IImagesCache Default = new FileSystemImagesCache();
        FileSystemImagesCache() { }
        //
        ConcurrentDictionary<string, Image> images = new ConcurrentDictionary<string, Image>();
        object DataModel.Services.IImagesCache.GetImage(string path) {
            return images.GetOrAdd(path, p => FileSystemHelper.GetImage(p, IconSizeType.Small, new Size(16, 16)));
        }
        ConcurrentDictionary<string, Image> largeImages = new ConcurrentDictionary<string, Image>();
        object DataModel.Services.IImagesCache.GetLargeImage(string path) {
            return largeImages.GetOrAdd(path, p => FileSystemHelper.GetImage(p, IconSizeType.Large, new Size(64, 64)));
        }
        void DataModel.Services.IImagesCache.Reset() {
            ResetCache(images);
            ResetCache(largeImages);
        }
        void ResetCache(ConcurrentDictionary<string, Image> cache) {
            Image[] imagesToDestoy = new Image[cache.Count];
            cache.Values.CopyTo(imagesToDestoy, 0);
            cache.Clear();
            for(int i = 0; i < imagesToDestoy.Length; i++)
                imagesToDestoy[i].Dispose();
        }
    }
}