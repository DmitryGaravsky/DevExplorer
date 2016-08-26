namespace DevExplorer.DataServices {
    using System;
    using System.Collections.Generic;

    public abstract class BaseFactory<TKey, TEntry>
        where TKey : class
        where TEntry : class {
        readonly IDictionary<Type, Func<TKey, TEntry>> map = new Dictionary<Type, Func<TKey, TEntry>>();
        protected void RegisterEntry<TParameter>(Func<TParameter, TEntry> getEntry) where TParameter : TKey {
            map.Add(typeof(TParameter), parameter => getEntry((TParameter)parameter));
        }
        protected TEntry GetEntry(TKey key, TEntry defaultEntry) {
            Func<TKey, TEntry> getEntry;
            return map.TryGetValue(GetKeyType(key), out getEntry) ?
                (getEntry(key) ?? defaultEntry) : defaultEntry;
        }
        //
        readonly static Type KeyType = typeof(TKey);
        static Type GetKeyType(TKey key) {
            var keyType = key.GetType();
            var interfaces = keyType.GetInterfaces();
            for(int i = 0; i < interfaces.Length; i++) {
                Type iKeyType = interfaces[(interfaces.Length - 1) - i];
                if(iKeyType != KeyType && KeyType.IsAssignableFrom(iKeyType))
                    return iKeyType;
            }
            return keyType;
        }
    }
}