using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SnapshotTestingGround.Snapshot
{
    public class Reader
    {
        private Dictionary<long, ISnapshotIdentity> _instantiatedObjects;

        public IEnumerable<T> GetCreatedObjects<T>() where T:class
        {
            return _instantiatedObjects.Values.Where(o => o.GetType() == typeof(T)).Select(o => o as T);
        }

        public void Read(string json)
        {
            _instantiatedObjects = new Dictionary<long, ISnapshotIdentity>();
            var o = JObject.Parse(json);

            foreach (var item in o["Snapshot"])
            {
                var typeName = item["ObjectType"].Value<string>();
                var type = Type.GetType(typeName);
                var parameters = item["Value"].ToDictionary(j => (j as JProperty).Name, j => (j as JProperty).Values().Select(v => v.Value<string>()).ToArray());

                var method = typeof(Reader).GetMethod("GetOrCreate").MakeGenericMethod(type);
                var newObject = method.Invoke(this, parameters["SnapshotObjectId"]) as ISnapshotIdentity;
                newObject.Hydrate(parameters, this);
            }
        }
        public T GetOrCreate<T>(string id) where T:class, ISnapshotIdentity, new()
        {
            if (id == null) return null;
            var longId = long.Parse(id);
            if (_instantiatedObjects.ContainsKey(longId)) return _instantiatedObjects[longId] as T;
            var newItem =  new T(); 
            newItem.SnapshotObjectId = longId;
            _instantiatedObjects.Add(newItem.SnapshotObjectId, newItem);
            return newItem;
        }
    }
}