namespace flyingApp
{
    public class PrintCommand : IExecutable
    {
        public PrintCommand() { }

        public void Execute(List<IBaseObject> info)
        {
            string snapshotFileName = $"snapshot_{DateTime.Now:HH_mm_ss}.json";
            FileDataLoader.UploadData(info, snapshotFileName);
            Console.WriteLine($"Snapshot created: {snapshotFileName}");
        }
    }
}
