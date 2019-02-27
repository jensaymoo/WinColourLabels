using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WinColourLabels.Database
{
    class InMemoryBase : IDatabase
    {
        private Hashtable labelstable = new Hashtable();

        public InMemoryBase()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\WinColourLabels\\Labels.db"))
            {
                labelstable = Deserialize<Hashtable>(File.ReadAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                                                "\\WinColourLabels\\Labels.db"));
            }
        }
        public FileLabel GetFileLabel(string path)
        {
            object curlabel = labelstable[path];

            if (curlabel != null)
                return (FileLabel)curlabel;
            else return FileLabel.NOTHING;
        }
        public void SetFileLabel(string path, FileLabel label)
        {
            if (label == FileLabel.NOTHING)
            {
                labelstable.Remove(path);
            }
            else
            {
                labelstable[path] = (int)label;
            }
        }

        private readonly object dbfile_lock = new object();
        public void SaveBase()
        {
            lock (dbfile_lock)
                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                "\\WinColourLabels\\Labels.db", Serialize(labelstable));
        }
        public async void SaveBaseAsync()
        {
            await Task.Run(() => SaveBase());
        }


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
