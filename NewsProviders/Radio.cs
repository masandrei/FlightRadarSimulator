namespace flyingApp
{
    public class Radio : INewsProvider
    {
        private string Name { get; set; }

        public Radio(string name)
        {
            Name = name;
        }

        public string NewsAirport(Airport ai)
        {
            return $"Reporting for {this.Name}, Ladies and Gentlemen, we are at the {ai.Name} airport";
        }

        public string NewsCargoPlane(CargoPlane cp)
        {
            return $"Reporting for {this.Name}, Ladies and Gentlemen, we are seeing the {cp.Serial} aircraft fly above us.";
        }

        public string NewsPassengerPlane(PassengerPlane pp)
        {
            return $"Reporting for {this.Name}, Ladies and Gentlemen, we’ve just witnessed {pp.Serial} takeoff.";
        }
    }
}
