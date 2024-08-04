namespace flyingApp
{
    public class DisplayCommand : IExecutable
    {
        private string _className;
        private string _condition;
        private string[] _fields;
        private Dictionary<string, Func<IBaseObject, object>> getters;

        public DisplayCommand(string className, string[] fields, string condition)
        {
            _className = className;
            _condition = condition;
            _fields = fields;
            getters = Getter.GetGetter(className);
        }

        public void Execute(List<IBaseObject> info)
        {
            var data = FileDataLoader.GetElementByType(_className, info);
            if (info.Count == 0)
            {
                Console.WriteLine("Unknown Object Class");
                return;
            }
            var filteredData = DataFilter.FilterData(_className, data, _condition);
            DisplaySelectedFields(filteredData, _fields);
        }

        private void DisplaySelectedFields(IEnumerable<IBaseObject> data, string[] fields)
        {
            if (fields[0] == "*")
            {
                fields = getters.Keys.ToArray();
            }
            Dictionary<string, int> columnWidths = CalculateColumnWidths(data, fields);
            List<Dictionary<string, string>> rows = GenerateRows(data, fields);
            PrintHeadersAndData(rows, fields, columnWidths);
        }

        private Dictionary<string, int> CalculateColumnWidths(
            IEnumerable<IBaseObject> data,
            string[] fields
        )
        {
            var columnWidths = new Dictionary<string, int>();
            foreach (var field in fields)
            {
                columnWidths[field] = field.Length;
            }

            foreach (var item in data)
            {
                foreach (var field in fields)
                {
                    var value = getters[field](item).ToString();
                    if (value.Length > columnWidths[field])
                    {
                        columnWidths[field] = value.Length;
                    }
                }
            }
            return columnWidths;
        }

        private List<Dictionary<string, string>> GenerateRows(
            IEnumerable<IBaseObject> data,
            string[] fields
        )
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            foreach (var item in data)
            {
                var row = new Dictionary<string, string>();
                foreach (var field in fields)
                {
                    var value = getters[field](item).ToString();
                    row[field] = value;
                }
                rows.Add(row);
            }
            return rows;
        }

        private void PrintHeadersAndData(
            List<Dictionary<string, string>> rows,
            string[] fields,
            Dictionary<string, int> columnWidths
        )
        {
            for (int i = 0; i < fields.Length; i++)
            {
                Console.Write($" {fields[i].PadRight(columnWidths[fields[i]])}");
                if (i < fields.Length - 1)
                    Console.Write(" |");
            }
            Console.WriteLine();

            for (int i = 0; i < fields.Length; i++)
            {
                Console.Write(new string('-', columnWidths[fields[i]] + 2));
                if (i < fields.Length - 1)
                    Console.Write("+");
            }
            Console.WriteLine();

            foreach (var row in rows)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    string field = fields[i];
                    var value = row[field];
                    int padding = columnWidths[field] - value.Length;

                    Console.Write(' ' + new string(' ', padding) + value);

                    if (i < fields.Length - 1)
                        Console.Write(" |");
                }
                Console.WriteLine();
            }
        }
    }
}
