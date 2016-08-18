using System;

namespace FolderSync
{
    public class CommandLineArgumentAttribute : Attribute
    {
		/// <summary>
		/// Создать аргумент конфигурации командной строки
		/// </summary>
		/// <param name="name">Наименование параметра командной строки</param>
		/// <param name="parseTemplate">Шаблон значения атрибута. default: {name}={value}</param>
		/// <param name="defaultValue">Значение по умолчанию. default: ""</param>
        public CommandLineArgumentAttribute(string name, string parseTemplate = "{name}={value}", string defaultValue = "")
        {
            Name = name;
            ParseTemplate = parseTemplate;
            DefaultValue = DefaultValue;
        }

        /// <summary>
		/// Наименование параметра командной строки
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Шаблон значения атрибута
		/// {name}={value} (по умолчанию).
        ///		Например: /{name}:{value}
        /// </summary>
        public string ParseTemplate { get; private set; }

        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public string DefaultValue { get; private set; }
    }
}