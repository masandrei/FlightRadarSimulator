namespace flyingApp
{
    public class DeleteCommand : IExecutable
    {
        private string _className;
        private string _conditions;

        public DeleteCommand(string className, string conditions)
        {
            _className = className;
            _conditions = conditions;
        }

        public void Execute(List<IBaseObject> info)
        {
            List<IBaseObject> typedObjects = FileDataLoader.GetElementByType(_className, info);
            if (typedObjects.Count == 0)
            {
                throw new Exception($"No objects of type {_className}");
            }
            var filteredData = DataFilter.FilterData(_className, typedObjects, _conditions);
            int count = 0;
            foreach (var item in filteredData.ToList())
            {
                info.Remove(item);
                count++;
            }
            Console.WriteLine($"Deleted {count} items");
        }
    }
}
