namespace flyingApp
{
    public interface IReportable
    {
        string Accept(INewsProvider prov);
    }
}
