namespace flyingApp
{
    public class NewsPaper : INewsProvider
    {
        private string Name { get; set; }

        public NewsPaper(string name)
        {
            Name = name;
        }

        public string NewsAirport(Airport ai)
        {
            return $"{this.Name} - A report from the {ai.Name} airport, {ai.Country}.";
        }

        public string NewsCargoPlane(CargoPlane cp)
        {
            return $"{this.Name} -An interview with the crew of {cp.Serial}.";
        }

        public string NewsPassengerPlane(PassengerPlane pp)
        {
            return $"{this.Name} - Breaking news! {pp.Model} aircraft loses EASA fails certification after inspection of {pp.Serial}.";
        }
    }
}
