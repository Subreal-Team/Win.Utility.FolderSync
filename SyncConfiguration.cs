using SubRealTeam.Common.ConsoleConfiguration;

namespace FolderSync
{
    public class SyncConfiguration : ConsoleConfigurationBase
    {
        /// <summary>
        /// Каталог назначения
        /// </summary>
        [CommandLineArgument("target", defaultValue: ".", Description = "Каталог назначения")]
        public string TargetFolder { get; protected set; }

        /// <summary>
        /// Каталог источник
        /// </summary>
        [CommandLineArgument("source", Description = "Каталог источник")]
        public string SourceFolder { get; protected set; }

        /// <summary>
        /// Маска файлов
        /// </summary>
        [CommandLineArgument("mask", defaultValue: "*.*", Description = "Маска файлов")]
        public string FileMask { get; protected set; }

        /// <summary>
        /// Сравнение по версии
        /// </summary>
        [CommandLineArgument("version", defaultValue: false, Description = "Сравнение по версии")]
        public bool CompareVersion { get; protected set; }
    }
}
