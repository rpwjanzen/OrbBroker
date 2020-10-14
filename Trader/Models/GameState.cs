using System.Collections.Generic;


namespace Trader
{
    class GameState {
        public Dictionary<string, SolarSystem> Systems;
        public Player Player;

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
    }
}
