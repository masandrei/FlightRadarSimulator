namespace flyingApp
{
    public class ReportCommand : IExecutable
    {
        private List<INewsProvider> _providers;
        private List<IReportable> _reportables;

        public ReportCommand()
        {
            _providers = new List<INewsProvider>();
            Television abelian = new Television("Abelian Television");
            _providers.Add(abelian);
            Television tensor = new Television("Channel TV-Tensor");
            _providers.Add(tensor);
            Radio quantifier = new Radio("Radion Quantifier");
            _providers.Add(quantifier);
            Radio shmem = new Radio("Shmem radio");
            _providers.Add(shmem);
            NewsPaper cat = new NewsPaper("Categories journal");
            _providers.Add(cat);
            NewsPaper pol = new NewsPaper("Polytechnical Gazette");
            _providers.Add(pol);
            _reportables = new List<IReportable>();
        }

        public void Execute(List<IBaseObject> info)
        {
            foreach (var item in info)
            {
                if (item is IReportable irep)
                {
                    _reportables.Add(irep);
                }
            }
            NewsGenerator ng = new NewsGenerator(_providers, _reportables);
            foreach (string? news in ng.GenerateNextNews())
            {
                if (news == null)
                {
                    break;
                }
                Console.WriteLine(news);
            }
        }
    }
}
