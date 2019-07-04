using System;

namespace FolderSync
{
	class Program
	{
		static void Main(string[] args)
		{
            var config = new SyncConfiguration();

            PrintHeader();

            if (config.NoParameters)
			{
                Console.WriteLine("Параметры:");
                config.PrintHelp();
                Console.ReadKey();
                return;
			}

			var synchronizer = new Synchronizer();
			synchronizer.Run(config);

            Console.Write("Нажмите любую клавишу ...");
		    Console.ReadKey();
		}

		private static void PrintHeader()
		{
			Console.WriteLine("FolderSync - синхронизация каталогов. По маске, с проверкой даты создания, версии файла.");
		}
	}
}
