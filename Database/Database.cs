using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WinColourLabels
{
    public enum FileLabel { RED, ORANGE, BLUE, GREEN, PURPLE, NOTHING };

    public class Database
    {
        private static Database instance;
        private Hashtable tagstable = new Hashtable();


        private Database()
        { }

        public static Database GetInstance()
        {
            if (instance == null)
            {
                instance = new Database();

                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                    "\\WinColourLabels\\Labels.db"))
                {
                    instance.tagstable = Deserialize<Hashtable>(File.ReadAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                                                    "\\WinColourLabels\\Labels.db"));
                }
            }
            return instance;
        }

        public FileLabel GetFileLabel(string path)
        {
            if (tagstable.Contains(path))
                return (FileLabel)tagstable[path];
            else return FileLabel.NOTHING;
        }
        public void SetFileLabel(string path, FileLabel label)
        {
            if(label == FileLabel.NOTHING)
            {
                tagstable.Remove(path);
            }
            else
            {
                tagstable[path] = (int)label;
            }
        }

        private object dbfile_lock = new object();
        public void SaveBase()
        {
            lock(dbfile_lock)
                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                "\\WinColourLabels\\Labels.db", Serialize(tagstable));
        }
        public async void SaveBaseAsync()
        {
            await Task.Run(() => SaveBase());
        }
        public static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                binSerializer.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }
        public static T Deserialize<T>(byte[] serializedObj)
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
