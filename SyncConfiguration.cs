using SubrealTeam.Common.ConsoleConfiguration;

namespace FolderSync
{
	public class SyncConfiguration : ConsoleConfigurationBase
    {
		/// <summary>
		/// Каталог назначения
		/// </summary>
        [CommandLineArgument("target", defaultValue: ".")]
        public string TargetFolder { get; protected set; }

		/// <summary>
		/// Каталог источник
		/// </summary>
		[CommandLineArgument("source")]
		public string SourceFolder { get; protected set; }

		/// <summary>
		/// Маска файлов
		/// </summary>
		[CommandLineArgument("mask", defaultValue: "*.*")]
		public string FileMask { get; protected set; }

		/// <summary>
		/// Сравнение по версии
		/// </summary>
		[CommandLineArgument("version", defaultValue: "false")]
		public string CompareVersion { get; protected set; }

		/// <summary>
		/// Флаг сравнение по версии
		/// </summary>
		public bool IsCompareVersion => CompareVersion.ToLower() == "true";
    }
}
