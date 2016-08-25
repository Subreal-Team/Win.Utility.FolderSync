using System;
using System.Diagnostics;
using System.IO;
using SubrealTeam.Common;
using SubrealTeam.Common.Extensions;
using SubrealTeam.Common.Logging;

namespace FolderSync
{
	public class Synchronizer
	{
		public void Run(SyncConfiguration config)
		{
			Guard.IsNotEmpty(config.TargetFolder);
			Guard.IsNotEmpty(config.SourceFolder);
			Guard.IsNotEmpty(config.FileMask);

			var targetFolder = config.TargetFolder.AddTrailingSlash();
			var sourceFolder = config.SourceFolder.AddTrailingSlash();

			if (!Directory.Exists(targetFolder))
				Directory.CreateDirectory(targetFolder);

			// Получаем список файлов в директории
			var targetFiles = Directory.GetFiles(targetFolder, config.FileMask, SearchOption.AllDirectories);
			var sourceFiles = Directory.GetFiles(sourceFolder, config.FileMask, SearchOption.AllDirectories);

			Logger.Instance.Debug($"Каталог назначения '{targetFolder}': {targetFiles.Length} файлов.");
			Logger.Instance.Debug($"Каталог источник '{sourceFolder}': {sourceFiles.Length} файлов.");

			if (sourceFiles.Length > 0)
			{
				Logger.Instance.Debug("Синхронизация ...");
                Console.ForegroundColor = ConsoleColor.Yellow;
			    var i = 1;
				foreach (string sourceFile in sourceFiles)
				{
                    var sourceFileName = Path.GetFileName(sourceFile) ?? "";
                    Guard.IsNotEmpty(sourceFileName);

                    Console.Write($"[{i:D4}/{(sourceFiles.Length + 1):D4}] {sourceFileName}");

					var targetDir = targetFolder;
					var sourceDir = Path.GetDirectoryName(sourceFile).AddTrailingSlash();
					// есть подкаталоги
					if (sourceDir != sourceFolder)
					{
						targetDir = sourceDir.Replace(sourceFolder, targetFolder);
						if (!Directory.Exists(targetDir))
							Directory.CreateDirectory(targetDir);
					}

					var destFile = Path.Combine(targetDir, sourceFileName);

					var isNew = true;
					// проверка версии (todo работает долго с файлом по сети)
					if (config.IsCompareVersion && File.Exists(destFile))
					{
						var destVersion = FileVersionInfo.GetVersionInfo(destFile);
						var sourceVersion = FileVersionInfo.GetVersionInfo(sourceFile);
						isNew = destVersion.FileVersion != sourceVersion.FileVersion;
						if (!isNew)
						    Console.Write($" | пропуск, версия совпадает {destVersion.FileVersion}");
					}
					
					if (isNew) 
						File.Copy(sourceFile, destFile, true);

				    i++;
				    ConsoleClearLine();
				}

                Console.WriteLine($"Синхронизовано {i} файлов из {sourceFiles.Length + 1}.");
                Console.ResetColor();
			}

            
		}

	    private static void ConsoleClearLine()
	    {
	        var buffer = new char[Console.WindowWidth];
            Console.CursorLeft = 0;
            Console.Write(buffer);
            Console.CursorTop = Console.CursorTop - 1;
        }
	}
}
