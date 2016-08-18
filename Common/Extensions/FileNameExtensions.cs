using System.IO;

namespace FolderSync.Common.Extensions
{
	public static class FileNameExtensions
	{
		public static string AddTrailingSlash(this string path)
		{
			if (path.IsEmpty()) return path;

			return !path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path + Path.DirectorySeparatorChar : path;
		}
	}
}
