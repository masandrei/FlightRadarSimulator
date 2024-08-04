namespace flyingApp
{
    public class AddCommand : IExecutable
    {
        private string _className;
        private Dictionary<string, string> _properties;

        public AddCommand(string className, Dictionary<string, string> properties)
        {
            _className = className;
            _properties = properties;
        }

        public void Execute(List<IBaseObject> info)
        {
            IBaseObject obj = Factory.getDefaultElement(_className);
            if (obj != null)
            {
                Dictionary<string, Action<IBaseObject, string>> setter = Setter.GetSetter(
                    _className
                );
                if (setter == null)
                {
                    throw new Exception($"Unknown type {_className}");
                }
                foreach (var prop in _properties.Keys)
                {
                    if (setter.TryGetValue(prop, out var func))
                    {
                        func(obj, _properties[prop]);
                    }
                    else
                    {
                        throw new Exception(
                            $"Property {prop} is not defined for class {_className}"
                        );
                    }
                }
                info.Add(obj);
                Console.WriteLine($"{_className} has been successfully added to the List");
            }
            else
            {
                throw new Exception($"Unknown class {_className}");
            }
        }
    }
}
