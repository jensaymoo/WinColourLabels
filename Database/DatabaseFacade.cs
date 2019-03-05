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
        /// <summary>
        /// Метод получения метки определенного файла
        /// </summary>
        /// <param name="Path">Полный путь к каталогу или файлу для которого необходимо получить метку</param>
        /// <returns></returns>
        public static FileLabel GetFileLabel(string Path)
        {
            return database.GetFileLabel(Path);
        }
        /// <summary>
        /// Метод сохранения метки для определенного файла или каталога 
        /// </summary>
        /// <param name="Path">Полный путь к каталогу или файлу для которого необходимо сохранить метку</param>
        /// <param name="Label">Метка файла</param>
        public static void SetFileLabel(string Path, FileLabel Label)
        {
            database.SetFileLabel(Path, Label);
        }

        /// <summary>
        /// Метод записи меток файлов и сохранения базы данных
        /// </summary>
        /// <param name="Paths">Полные пути к каталогам или файлам для которых необходимо сохранить метку</param>
        /// <param name="Label">Метка файлов</param>
        public static void SetFilesLabelAndSaveBase(string[] Paths, FileLabel Label)
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                SetFileLabel(Paths[i], Label);
            }
            database.SaveBase();
        }
        /// <summary>
        /// Асинхронный метод записи меток файлов и сохранения базы данных
        /// </summary>
        /// <param name="Paths">Полные пути к каталогам или файлам для которых необходимо сохранить метку</param>
        /// <param name="Label">Метка файлов</param>
        public static async void SetFilesLabelAndSaveAsync(string[] Paths, FileLabel Label)
        {
            await Task.Run(() => SetFilesLabelAndSaveBase(Paths, Label));
        }
        /// <summary>
        /// Асинхронный метод сохранения базы данных
        /// </summary>
        public static void SaveBaseAsync()
        {
            database.SaveBaseAsync();
        }

    }
}
