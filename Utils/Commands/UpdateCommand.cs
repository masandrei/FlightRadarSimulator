namespace flyingApp
{
    public class UpdateCommand : IExecutable
    {
        private string _className;
        private string _condition;
        private Dictionary<string, string> _properties;

        public UpdateCommand(
            string className,
            Dictionary<string, string> properties,
            string conditions
        )
        {
            _className = className;
            _properties = properties;
            _condition = conditions;
        }

        public void Execute(List<IBaseObject> info)
        {
            Dictionary<string, Action<IBaseObject, string>> setter = Setter.GetSetter(_className);
            List<IBaseObject> typedObjects = FileDataLoader.GetElementByType(_className, info);
            if (setter == null)
            {
                throw new Exception($"Unknown type {_className}");
            }
            if (typedObjects.Count == 0)
            {
                throw new Exception($"No objects of type {_className}");
            }
            var filteredData = DataFilter.FilterData(_className, typedObjects, _condition);
            foreach (var item in filteredData)
            {
                foreach (var key in _properties.Keys)
                {
                    if (setter.TryGetValue(key, out var prop))
                    {
                        prop(item, _properties[key]);
                    }
                    else
                    {
                        throw new Exception($"Property {key} is not defined for {_className}");
                    }
                }
            }
        }
    }
}
