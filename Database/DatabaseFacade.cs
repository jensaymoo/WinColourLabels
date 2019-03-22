using System.Threading.Tasks;
using WinColourLabels.Database.Bases;

namespace WinColourLabels.Database
{
    public static class DatabaseFacade
    {
        private static IDatabase database;

        static DatabaseFacade()
        {
            database = new NTFS();
        }
        /// <summary>
        /// Метод получения метки определенного файла или каталога 
        /// </summary>
        /// <param name="Path">Полный путь к каталогу или файлу для которого необходимо получить метку</param>
        /// <returns></returns>
        public static FileLabel GetLabel(string Path) => database.GetLabel(Path);
        /// <summary>
        /// Метод сохранения метки для определенного файла или каталога 
        /// </summary>
        /// <param name="Path">Полный путь к каталогу или файлу для которого необходимо сохранить метку</param>
        /// <param name="Label">Метка файла или каталога</param>
        public static void SetLabel(string Path, FileLabel Label) => database.SetLabel(Path, Label);
        /// <summary>
        /// Асинхронный метод сохранения метки для определенного файла или каталога 
        /// </summary>
        /// <param name="Path">Полный путь к каталогу или файлу для которого необходимо сохранить метку</param>
        /// <param name="Label">Метка файла или каталога</param>
        public static async void SetLabelAsync(string Path, FileLabel Label) => await Task.Run(() => SetLabel(Path, Label));
        /// <summary>
        /// Метод сохранения метки для нескольких файлов или каталогов 
        /// </summary>
        /// <param name="Paths">Полные пути к каталогам или файлам для которых необходимо сохранить метку</param>
        /// <param name="Label">Метка файла или каталога</param>
        public static void SetLabel(string[] Paths, FileLabel Label) => database.SetLabel(Paths, Label);
        /// <summary>
        /// Асинхронный метод сохранения метки для нескольких файлов или каталогов 
        /// </summary>
        /// <param name="Paths">Полные пути к каталогам или файлам для которых необходимо сохранить метку</param>
        /// <param name="Label">Метка файла или каталога</param>
        public static async void SetLabelAsync(string[] Paths, FileLabel Label) => await Task.Run(() => SetLabel(Paths, Label));

    }
}
