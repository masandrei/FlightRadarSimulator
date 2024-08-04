namespace flyingApp
{
    public class NewsGenerator
    {
        List<INewsProvider> providers;
        List<IReportable> reportables;

        public NewsGenerator(List<INewsProvider> _providers, List<IReportable> _reportables)
        {
            providers = _providers;
            reportables = _reportables;
        }

        public IEnumerable<string> GenerateNextNews()
        {
            foreach (var pr in this.providers)
            {
                foreach (var rep in this.reportables)
                {
                    yield return rep.Accept(pr);
                }
            }
            while (true)
            {
                yield return null;
            }
        }
    }
}
