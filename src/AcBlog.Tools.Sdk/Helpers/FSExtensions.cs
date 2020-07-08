using System.IO;

namespace AcBlog.Tools.Sdk.Helpers
{
    internal static class FSExtensions
    {
        public static void CopyDirectory(string sourceDirPath, string saveDirPath)
        {
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }
            string[] files = Directory.GetFiles(sourceDirPath);

            foreach (string file in files)
            {
                string pFilePath = Path.Join(saveDirPath, Path.GetFileName(file));
                File.Copy(file, pFilePath, true);
            }

            string[] dirs = Directory.GetDirectories(sourceDirPath);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, Path.Join(saveDirPath, Path.GetFileName(dir)));
            }
        }
    }
}
