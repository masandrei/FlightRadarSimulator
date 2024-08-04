using System.Text.RegularExpressions;

namespace flyingApp
{
    public static class QueryParser
    {
        private static string displayRegex =
            @"display\s+([\w,*,\s.]+)\s+from\s+(\w+)(?:\s+where\s+(.+))?";
        private static string deleteRegex = @"delete\s+(\w+)(?:\s+where\s+(.+))?";
        private static string addRegex = @"add (\w+) new \(([^)]+)\)";
        private static string updateRegex = @"update (\w+) set \(([^)]+)\)(?: where (.+))?";

        public static IExecutable Parse(string command, string typeOfQuery)
        {
            string[]? fields = null;
            string? className = null;
            Dictionary<string, string> properties = new Dictionary<string, string>();
            string? conditions = null;
            Match? match = null;
            match = Regex.Match(command, displayRegex);
            if (typeOfQuery == "display" && match.Success)
            {
                className = match.Groups[2].Value.Substring(0, match.Groups[2].Value.Length).Trim();
                fields = match.Groups[1].Value.Split(",").Select(str => str.Trim()).ToArray();
                conditions = match.Groups[3].Value;
                return new DisplayCommand(className, fields, conditions);
            }
            match = Regex.Match(command, updateRegex);
            if (typeOfQuery == "update" && match.Success)
            {
                className = match.Groups[1].Value.Substring(0, match.Groups[1].Value.Length).Trim();
                properties = match
                    .Groups[2]
                    .Value.Split(",")
                    .Select(str => str.Trim().Split("="))
                    .ToDictionary(keyValue => keyValue[0].Trim(), keyValue => keyValue[1].Trim());
                conditions = match.Groups[3].Value;
                return new UpdateCommand(className, properties, conditions);
            }
            match = Regex.Match(command, deleteRegex);
            if (typeOfQuery == "delete" && match.Success)
            {
                className = match.Groups[1].Value.Substring(0, match.Groups[1].Value.Length).Trim();
                conditions = match.Groups[2].Value;
                return new DeleteCommand(className, conditions);
            }
            match = Regex.Match(command, addRegex);
            if (typeOfQuery == "add" && match.Success)
            {
                className = match.Groups[1].Value.Substring(0, match.Groups[1].Value.Length).Trim();
                properties = match
                    .Groups[2]
                    .Value.Split(",")
                    .Select(str => str.Trim().Split("="))
                    .ToDictionary(keyValue => keyValue[0].Trim(), keyValue => keyValue[1].Trim());
                return new AddCommand(className, properties);
            }
            throw new Exception($"Unknown command {typeOfQuery}");
        }
    }
}
