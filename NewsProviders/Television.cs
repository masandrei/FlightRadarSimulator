namespace flyingApp
{
    public class Television : INewsProvider
    {
        private string Name { get; set; }

        public Television(string name)
        {
            Name = name;
        }

        public string NewsAirport(Airport ai)
        {
            return $"{this.Name} - <An image of {ai.Name} airport>";
        }

        public string NewsCargoPlane(CargoPlane cp)
        {
            return $"{this.Name} - <An image of {cp.Serial} cargo plane>";
        }

        public string NewsPassengerPlane(PassengerPlane pp)
        {
            return $"{this.Name} - <An image of {pp.Serial} passenger plane>";
        }
    }
}
