using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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

	public abstract class ConfigurationBase
	{
		protected string[] _arguments = Environment.GetCommandLineArgs();

		protected ConfigurationBase()
        {
			// по всем публичным свойствам класса
            var typeInfo = this.GetType();
            var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// взять свойства с атрибутом CommandLineArgument
			var attributedProps = publicProps.Select(x => new { property = x, customAttributes = x.GetCustomAttributes() })
				.Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
				.ToList();

			foreach (var prop in attributedProps)
            {
	            foreach (var attr in prop.customAttributes)
	            {
		            var cmdAttr = (CommandLineArgumentAttribute) attr;
		            var cmdValue = _arguments.FirstOrDefault(x => x.ToUpper().Contains(cmdAttr.Name.ToUpper()));
		            if (String.IsNullOrWhiteSpace(cmdValue)) continue;

		            var match = Regex.Match(cmdAttr.ParseTemplate, "{name}(.*){value}");
		            if (!match.Success || match.Groups.Count <= 0) continue;

		            var splitter = match.Groups[1].Value;
		            var value = Regex.Split(cmdValue, splitter);
		            prop.property.SetValue(this,
			            (value.Length == 2) && !String.IsNullOrWhiteSpace(value[1])
				            ? value[1]
				            : cmdAttr.DefaultValue);

	            }
            }
        }
	}
}
