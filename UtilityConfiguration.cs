namespace FolderSync
{
	public class UtilityConfiguration : ConfigurationBase
    {
		/// <summary>
		/// Каталог назначения
		/// </summary>
        [CommandLineArgument("TargetFolder", defaultValue: ".")]
        public string TargetFolder { get; protected set; }

		/// <summary>
		/// Каталог источник
		/// </summary>
		[CommandLineArgument("SourceFolder")]
		public string SourceFolder { get; protected set; }
    }
}
