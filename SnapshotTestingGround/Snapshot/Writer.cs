using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SnapshotTestingGround.Snapshot
{
    public class Writer
    {
        private JsonWriter _writer;
        private StringBuilder _builder;
        private long _latestObjectId;
        public long SnapshotIdentity { get; set; }

        public Writer(long snapshotIdentity)
        {
            SnapshotIdentity = snapshotIdentity;
            _latestObjectId = 0;
            _builder = new StringBuilder();
            var swriter = new StringWriter(_builder);
            _writer = new JsonTextWriter(swriter);
            _writer.WriteStartObject();
            _writer.WritePropertyName("Snapshot");
            _writer.WriteStartArray();
        }

        public void WriteClassStart(string identifier)
        {
            _writer.WriteStartObject();
            _writer.WritePropertyName("ObjectType");
            _writer.WriteValue(identifier);
            _writer.WritePropertyName("Value");
            _writer.WriteStartObject();
        }

        public void WriteClassEnd()
        {
            _writer.WriteEndObject();
            _writer.WriteEndObject();
        }

        public void WriteProperty<T>(string name, T property)
        {
            _writer.WritePropertyName(name);
            _writer.WriteValue(property);
        }

        public void WriteArray(string name, IEnumerable<long> identifiers)
        {
            _writer.WritePropertyName(name);
            _writer.WriteStartArray();
            foreach (var id in identifiers)
            {
                _writer.WriteValue(id);
            }
            _writer.WriteEndArray();
        }

        public void WriteArray(string name, IEnumerable<string> values)
        {
            _writer.WritePropertyName(name);
            _writer.WriteStartArray();
            foreach (var item in values)
            {
                _writer.WriteValue(item);
            }
            _writer.WriteEndArray();
        }

        public long GetNextObjectId()
        {
            return ++_latestObjectId;
        }

        public string GetJson()
        {
            _writer.WriteEndArray();
            _writer.WriteEndObject();
            return _builder.ToString();
        }
    }
}