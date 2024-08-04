namespace flyingApp
{
    public static class Getter
    {
        public static Dictionary<string, Func<IBaseObject, object>>? GetGetter(string type)
        {
            if (ModelGetter.TryGetValue(type, out var dict))
            {
                return dict;
            }
            else
                return null;
        }

        private static Dictionary<string, Func<IBaseObject, object>> FlightProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", flight => ((Flight)flight).ObjectID },
                { "OriginID", flight => ((Flight)flight).OriginID.ObjectID },
                { "TargetID", flight => ((Flight)flight).TargetID.ObjectID },
                { "TakeoffTime", flight => ((Flight)flight).TakeoffTime },
                { "LandingTime", flight => ((Flight)flight).LandingTime },
                { "WorldPosition.Lat", flight => ((Flight)flight).Latitude },
                { "WorldPosition.Long", flight => ((Flight)flight).Longitude },
                { "AMSL", flight => ((Flight)flight).AMSL },
                { "PlaneID", flight => ((Flight)flight).PlaneID }
            };
        private static Dictionary<string, Func<IBaseObject, object>> AirportProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", airport => ((Airport)airport).ObjectID },
                { "Name", airport => ((Airport)airport).Name },
                { "Code", airport => ((Airport)airport).Code },
                { "WorldPosition.Lat", airport => ((Airport)airport).Latitude },
                { "WorldPosition.Long", airport => ((Airport)airport).Longitude },
                { "AMSL", airport => ((Airport)airport).AMSL },
                { "Country", airport => ((Airport)airport).Country }
            };
        private static Dictionary<string, Func<IBaseObject, object>> CargoProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", cargo => ((Cargo)cargo).ObjectID },
                { "Wight", cargo => ((Cargo)cargo).Weight },
                { "Code", cargo => ((Cargo)cargo).Code },
                { "Description", cargo => ((Cargo)cargo).Description }
            };
        private static Dictionary<string, Func<IBaseObject, object>> CargoPlaneProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", cp => ((CargoPlane)cp).ObjectID },
                { "Serial", cp => ((CargoPlane)cp).Serial },
                { "Country", cp => ((CargoPlane)cp).Country },
                { "Model", cp => ((CargoPlane)cp).Model },
                { "MaxWeight", cp => ((CargoPlane)cp).MaxWeight }
            };
        private static Dictionary<string, Func<IBaseObject, object>> CrewProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", crew => ((Crew)crew).ObjectID },
                { "Name", crew => ((Crew)crew).Name },
                { "Age", crew => ((Crew)crew).Age },
                { "Phone", crew => ((Crew)crew).Phone },
                { "Email", crew => ((Crew)crew).Email },
                { "Practice", crew => ((Crew)crew).Practice },
                { "Role", crew => ((Crew)crew).Role }
            };
        private static Dictionary<string, Func<IBaseObject, object>> PassengerProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", passenger => ((Passenger)passenger).ObjectID },
                { "Name", passenger => ((Passenger)passenger).Name },
                { "Age", passenger => ((Passenger)passenger).Age },
                { "Phone", passenger => ((Passenger)passenger).Phone },
                { "Email", passenger => ((Passenger)passenger).Email },
                { "Class", passenger => ((Passenger)passenger).Class },
                { "Miles", passenger => ((Passenger)passenger).Miles }
            };
        private static Dictionary<string, Func<IBaseObject, object>> PassengerPlaneProperties =
            new Dictionary<string, Func<IBaseObject, object>>()
            {
                { "ObjectID", pp => ((PassengerPlane)pp).ObjectID },
                { "Serial", pp => ((PassengerPlane)pp).Serial },
                { "Country", pp => ((PassengerPlane)pp).Country },
                { "Model", pp => ((PassengerPlane)pp).Model },
                { "FirstClassSize", pp => ((PassengerPlane)pp).FirstClassSize },
                { "BusinessClassSize", pp => ((PassengerPlane)pp).BusinessClassSize },
                { "EconomClassSize", pp => ((PassengerPlane)pp).EconomClassSize }
            };

        private static Dictionary<
            string,
            Dictionary<string, Func<IBaseObject, object>>
        > ModelGetter = new Dictionary<string, Dictionary<string, Func<IBaseObject, object>>>()
        {
            { "Flight", FlightProperties },
            { "Airport", AirportProperties },
            { "Cargo", CargoProperties },
            { "CargoPlane", CargoPlaneProperties },
            { "Crew", CrewProperties },
            { "Passenger", PassengerProperties },
            { "PassengerPlane", PassengerPlaneProperties }
        };
    }
}
