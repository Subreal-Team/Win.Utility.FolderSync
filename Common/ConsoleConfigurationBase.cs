using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FolderSync.Common.Logging;

namespace FolderSync.Common
{
	public abstract class ConsoleConfigurationBase
	{
		protected string[] _arguments = Environment.GetCommandLineArgs();

		protected ConsoleConfigurationBase()
		{
			NotValidParameters = false;
			NotValidParamtersMessages = new List<string>();

			// �� ���� ��������� ��������� ������
			var typeInfo = this.GetType();
			var publicProps = typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// ����� �������� � ��������� CommandLineArgument
			var attributedProps = publicProps.Select(x => new { property = x, customAttributes = x.GetCustomAttributes() })
				.Where(x => x.customAttributes.Any(y => y.GetType() == typeof(CommandLineArgumentAttribute)))
				.ToList();

			foreach (var prop in attributedProps)
			{
				foreach (var attr in prop.customAttributes)
				{
					var cmdAttr = (CommandLineArgumentAttribute) attr;
					var cmdValue = _arguments.FirstOrDefault(x => x.ToUpper().Contains(cmdAttr.Name.ToUpper()));
					// �������� �� ������, ����� �������� �� ���������
					if (String.IsNullOrWhiteSpace(cmdValue))
					{
						prop.property.SetValue(this, cmdAttr.DefaultValue);
						continue;
					}

					var match = Regex.Match(cmdAttr.ParseTemplate, "{name}(.*){value}");
					// ������� ����� ��������
					if (!match.Success || match.Groups.Count <= 0)
					{
						NotValidParameters = true;
						var errorMessage = String.Format("������� ����� �������� {0}, ������: {1}", cmdAttr.Name, cmdAttr.ParseTemplate);
						NotValidParamtersMessages.Add(errorMessage);
						Logger.Instance.Error(errorMessage);
						continue;
					}

					var splitter = match.Groups[1].Value;
					var value = Regex.Split(cmdValue, splitter);
					prop.property.SetValue(this,
						(value.Length == 2) && !String.IsNullOrWhiteSpace(value[1])
							? value[1]
							: cmdAttr.DefaultValue);

				}
			}
		}

		/// <summary>
		/// ���� - ��������� �� �������
		/// </summary>
		public bool NoParameters { get { return _arguments.Length == 1; } }

		/// <summary>
		/// ���� ������ ��������� ����������
		/// </summary>
		public bool NotValidParameters { get; private set; }

		/// <summary>
		/// ��������� �� ������ ���������� ����������
		/// </summary>
		public List<string> NotValidParamtersMessages { get; private set; }
	}
}