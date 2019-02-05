using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using Newtonsoft.Json;

namespace CodeProvider.Infrastructure
{
	public static class ExcelReader
	{
		public static string GetCodes(Stream stream, int sheetNumber, int firstRow, int codeColumn, int descriptionColumn)
		{
			var codes = new List<CodeDescription>();
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				SelectSheet(reader, sheetNumber);
				SelectRecordDefinitionFirstRow(reader, firstRow);
				while (reader.Read())
				{
					var code = reader.GetValue(codeColumn)?.ToString()?.Trim();
					var description = reader.GetValue(descriptionColumn)?.ToString()?.Trim();
					if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(description))
						continue;
					codes.Add(new CodeDescription { Code = code, Description = description });
				}
			}

			return JsonConvert.SerializeObject(codes, Formatting.Indented);
		}

		public static void SelectSheet(IExcelDataReader reader, int sheetNumber)
		{
			for (var i = 1; i < sheetNumber; i++)
				reader.NextResult();
		}

		public static void SelectRecordDefinitionFirstRow(IExcelDataReader reader, int rowNumber)
		{
			for (var i = 0; i < rowNumber; i++)
				reader.Read();
		}
	}
}