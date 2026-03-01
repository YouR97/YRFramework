using System.IO;

namespace YRFramework.Editor
{
    public partial class TextureImportProcessor
    {
        private string CheckName(string fileName)
        {
            if (fileName.Contains("ReflectionProbe-"))
            {
                var index = fileName.LastIndexOf("-");
                fileName = fileName.Remove(index, fileName.Length - index);
                fileName += "_";
            }

            return fileName;
        }

        private string GetFileName(string assetPath)
        {
            return Path.GetFileNameWithoutExtension(assetPath);
        }
    }
}