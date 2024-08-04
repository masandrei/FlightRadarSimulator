namespace flyingApp
{
    public class ConsoleHandler
    {
        public static void Handle(List<IBaseObject> info)
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    string command = Console.ReadLine();
                    if (command == "print")
                    {
                        PrintCommand print = new PrintCommand();
                        print.Execute(info);
                    }
                    else if (command == "report")
                    {
                        ReportCommand report = new ReportCommand();
                        report.Execute(info);
                    }
                    else if (command == "exit")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        try
                        {
                            IExecutable QueryCommand = QueryParser.Parse(
                                command,
                                command.Substring(0, command.IndexOf(" ")).Trim()
                            );
                            QueryCommand.Execute(info);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            });
            thread.Start();
        }
    }
}
