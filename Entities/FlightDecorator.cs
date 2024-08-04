using NetworkSourceSimulator;

namespace flyingApp
{
    public class FlightDecorator : Flight
    {
        private readonly Flight _fl;
        private readonly object _syncObject = new object();

        public FlightDecorator(Flight flight)
        {
            this._fl = flight;
        }

        public override void Update(PositionUpdateArgs pos)
        {
            lock (_syncObject)
            {
                this._fl.Update(pos);
            }
        }

        public override void Movement()
        {
            lock (_syncObject)
            {
                this._fl.Movement();
            }
        }

        public override FlightGUI ConvertFlight()
        {
            lock (_syncObject)
            {
                return _fl.ConvertFlight();
            }
        }
    }
}
