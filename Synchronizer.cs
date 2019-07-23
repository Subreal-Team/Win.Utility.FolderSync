using System;
using System.Diagnostics;
using System.IO;
using SubRealTeam.Common;
using SubRealTeam.Common.Extensions;
using SubRealTeam.Common.Logging;

namespace FolderSync
{
    public class Synchronizer
    {
        public void Run(SyncConfiguration config)
        {
            // TODO add requied attribute processing to ConsoleConfigurationBase
            if (string.IsNullOrEmpty(config.SourceFolder))
            {
                Logger.Warn("Необходимо указать каталог источник 'source'");
                return;
            }

            Guard.IsNotEmpty(config.TargetFolder);
            Guard.IsNotEmpty(config.FileMask);

            var targetFolder = config.TargetFolder.AddTrailingSlash();
            var sourceFolder = config.SourceFolder.AddTrailingSlash();

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            Logger.Info("Получаем список файлов в директориях.");
            string[] targetFiles, sourceFiles;
            try
            {
                targetFiles = Directory.GetFiles(targetFolder, config.FileMask, SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"{targetFolder}");
                return;
            }
            try
            {
                sourceFiles = Directory.GetFiles(sourceFolder, config.FileMask, SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"{sourceFolder}");
                return;
            }

            Logger.Info($"Каталог назначения '{targetFolder}': {targetFiles.Length} файлов.");
            Logger.Info($"Каталог источник '{sourceFolder}': {sourceFiles.Length} файлов.");

            if (sourceFiles.Length > 0)
            {
                Logger.Info("Синхронизация ...");
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
                    if (config.CompareVersion && File.Exists(destFile))
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
