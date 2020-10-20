using System;
using System.Collections.Generic;


namespace Trader
{
    class GameState {
        public Dictionary<string, SolarSystem> Systems = new Dictionary<string, SolarSystem>();
        public Player Player = new Player();

        public Planet CurrentPlanet {
            get
            {
                var location = this.Player.Location;
                return Systems[location.System].Planets[location.Planet];
            }
        }

        public SolarSystem CurrentSystem {
            get
            {
                var location = this.Player.Location;
                return Systems[location.System];
            }
        }

        public int DistanceTo(string destination)
        {
            return Math.Abs(CurrentSystem.Positions[Player.Location.Planet] - CurrentSystem.Positions[destination]);
        }
    }
}
