namespace flyingApp
{
    public static class DataFilter
    {
        public static IEnumerable<IBaseObject> FilterData(
            string className,
            IEnumerable<IBaseObject> data,
            string conditions
        )
        {
            if (string.IsNullOrEmpty(conditions))
                return data;

            var orConditions = conditions.Split(
                new[] { " or " },
                StringSplitOptions.RemoveEmptyEntries
            );
            var results = new List<IBaseObject>();

            foreach (var orCondition in orConditions)
            {
                var filteredData = data;
                var andConditions = orCondition.Split(
                    new[] { " and " },
                    StringSplitOptions.RemoveEmptyEntries
                );

                foreach (var condition in andConditions)
                {
                    var parts = ParseCondition(condition);
                    if (parts == null)
                        continue;

                    filteredData = filteredData
                        .Where(item =>
                            EvaluateCondition(
                                className,
                                item,
                                parts.Item1,
                                parts.Item2,
                                parts.Item3
                            )
                        )
                        .ToList();
                }

                results.AddRange(filteredData);
            }

            return results.Distinct();
        }

        private static Tuple<string, string, string> ParseCondition(string condition)
        {
            string[] operators = { ">=", "<=", "!=", "=", "<", ">" }; // Order matters to avoid partial matches
            foreach (var op in operators)
            {
                var index = condition.IndexOf(op);
                if (index != -1)
                {
                    var field = condition.Substring(0, index).Trim();
                    var value = condition.Substring(index + op.Length).Trim();
                    return Tuple.Create(field, op, value);
                }
            }
            return null;
        }

        private static bool CompareValues(string itemValue, string conditionValue, string ops)
        {
            switch (ops)
            {
                case "=":
                    return itemValue == conditionValue;
                case "!=":
                    return itemValue != conditionValue;
                case "<":
                    return Convert.ToDouble(itemValue) < Convert.ToDouble(conditionValue);
                case ">":
                    return Convert.ToDouble(itemValue) > Convert.ToDouble(conditionValue);
                case "<=":
                    return Convert.ToDouble(itemValue) <= Convert.ToDouble(conditionValue);
                case ">=":
                    return Convert.ToDouble(itemValue) >= Convert.ToDouble(conditionValue);
                default:
                    return false;
            }
        }

        private static bool EvaluateCondition(
            string className,
            IBaseObject item,
            string field,
            string op,
            string value
        )
        {
            string itemValueAsString;
            var itemProps = Getter.GetGetter(className);

            if (field == "WorldPosition.Lat" || field == "WorldPosition.Long")
            {
                var worldPos = itemProps[field];
                itemValueAsString = worldPos(item).ToString();
            }
            else
            {
                var itemValue = itemProps[field](item);
                itemValueAsString = itemValue?.ToString();
            }

            return CompareValues(itemValueAsString, value, op);
        }
    }
}
