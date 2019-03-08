using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WinColourLabels.Database
{
    class InMemoryBase : IDatabase
    {
        private Hashtable labels_table;
        private readonly string db_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\WinColourLabels\\Labels.db";

        public InMemoryBase()
        {
            if (File.Exists(db_path))
            {
                labels_table = Hashtable.Synchronized(Deserialize<Hashtable>(File.ReadAllBytes(db_path)));
            }
            else
            {
                labels_table = Hashtable.Synchronized(new Hashtable());
            }

        }
        public FileLabel GetFileLabel(string path) => (FileLabel)(labels_table[path] ?? FileLabel.NOTHING);

        public void SetFileLabel(string path, FileLabel label)
        {
            if (label == FileLabel.NOTHING)
                labels_table.Remove(path);
            else
                labels_table[path] = (byte)label;
        }

        private readonly object file_lock = new object();
        public void SaveBase()
        {
            lock (file_lock) File.WriteAllBytes(db_path, Serialize(labels_table));
        }
        public async void SaveBaseAsync() => await Task.Run(() => SaveBase());


        private static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                binSerializer.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }
        private static T Deserialize<T>(byte[] serializedObj)
        {
            T obj = default(T);
            using (MemoryStream memStream = new MemoryStream(serializedObj))
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                obj = (T)binSerializer.Deserialize(memStream);
            }
            return obj;
        }
    }
}
