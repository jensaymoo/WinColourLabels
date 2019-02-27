using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WinColourLabels.Database
{
    public enum FileLabel { RED, ORANGE, BLUE, GREEN, PURPLE, NOTHING };

    public static class DatabaseFacade
    {
        private static IDatabase database;

        static DatabaseFacade()
        {
            database = new InMemoryBase();
        }

        public static FileLabel GetFileLabel(string path)
        {
            return database.GetFileLabel(path);
        }
        public static void SetFileLabel(string path, FileLabel label)
        {
            database.SetFileLabel(path, label);
        }

        public static void SaveBaseAsync()
        {
            database.SaveBaseAsync();
        }

    }
}
