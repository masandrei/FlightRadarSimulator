using System;
using System.Globalization;

namespace flyingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            List<IBaseObject> info = FileDataLoader.DownloadData("example_data.ftr.txt");
            FileDataLoader.UploadData(info, "example_data.json");
            List<IBaseObject> newInfo = new List<IBaseObject>();
            NetworkDataLoader ndl = new NetworkDataLoader();
            ConsoleHandler ch = new ConsoleHandler();
            //ndl.DownloadData(newInfo);
            ndl.UpdateData(info);
            ConsoleHandler.Handle(info);
            FlightRadar.CreateRadar(info);
        }
    }
}
