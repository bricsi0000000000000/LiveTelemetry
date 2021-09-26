using DataModel.Constants;
using System.IO;

namespace DataModel.Extensions
{
    public static class Extension
    {
        public static string MakePath(this string path, string folder)
        {
            return Path.Combine($"{TextManager.ROOT_DIRECTORY}/{folder}", path);
        }
    }
}
