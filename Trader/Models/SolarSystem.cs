using System.Collections.Generic;


namespace Trader
{
    class SolarSystem
    {
        public string Name = "";
        public Dictionary<string, int> Positions = new Dictionary<string, int>();
        public Dictionary<string, Planet> Planets = new Dictionary<string, Planet>();
    }
}
