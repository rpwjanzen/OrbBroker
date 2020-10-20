using System.Collections.Generic;
using System.Linq;
using Trader.IO;
using Trader.Models;
using Console = System.Console;
using Math = System.Math;


namespace Trader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Trader!");

            var gameState = Misc.LoadGame(args);
            if (gameState == null)
            {
                Console.WriteLine("Unable to load universe.");
                return;
            }

            Console.WriteLine($"You are captain of the {gameState.Player.ShipName}.");

            var quit = false;
            while (!quit)
            {
                Console.WriteLine();
                Console.WriteLine($"Turn: {gameState.Player.Turn}/5000");
                Console.WriteLine("-----");
                var usedCapacity = gameState.Player.Cargo.Goods.Sum(kvp => kvp.Value);
                Console.WriteLine($"Location: {gameState.CurrentPlanet.Name}, {gameState.CurrentSystem.Name}");
                Console.WriteLine($"Cash: {gameState.Player.Cash}");
                Console.WriteLine($"Fuel: {gameState.Player.Fuel}/{gameState.Player.FuelCapacity}");
                Console.WriteLine($"Cargo: {usedCapacity}/{gameState.Player.Cargo.Capacity}");
                Console.WriteLine("-----");
                var printedGoods = new List<string>();
                var market = gameState.CurrentPlanet.Market;
                ConsoleExtensions.WriteTable(
                    market.ToArray(),
                    new[] { "Good", "Ship", "Planet","Rate" },
                    (g) => {
                        printedGoods.Add(g.Key);
                        gameState.Player.Cargo.Goods.TryGetValue(g.Key, out var amountOnShip);
                        return new[] { g.Key, amountOnShip.ToString(), g.Value.Amount.ToString(), g.Value.Rate.ToString() };
                    }
                );

                Console.WriteLine("-----");
                var cargo = gameState.Player.Cargo;
                var unprintedGoods = cargo.Goods.Where(kvp => !printedGoods.Contains(kvp.Key)).ToArray();
                if (unprintedGoods.Length > 0) {
                    foreach (var kvp in unprintedGoods)
                    {
                        var good = kvp.Key;
                        var shipAmount = kvp.Value;
                        Console.WriteLine($"{good}\t{shipAmount}");
                    }
                }
                
                Console.WriteLine(" (b)uy good amount");
                Console.WriteLine(" (s)ell good amount");
                Console.WriteLine(" (t)ravel planet");
                Console.WriteLine(" sa(v)e");
                Console.WriteLine(" (q)uit");
                var line = Console.ReadLine();
                if (line.StartsWith("q"))
                {
                    Console.WriteLine($"Your score is {gameState.Player.Cash}");
                    Console.WriteLine();
                    Console.WriteLine("High Scores");
                    DisplayHighScores(gameState);
                    quit = true;
                    return;
                }

                var save = line.StartsWith("v");
                if (save)
                {
                    Misc.SaveGame(gameState);
                    continue;
                }

                var parts = line.Split(" ");
                var travel = parts[0].StartsWith("t");
                if (travel)
                {
                    TravelToPlanet(parts, gameState);
                    continue;
                }

                if (parts.Length < 3)
                {
                    continue;
                }

                var goodName = parts[1];
                if (!market.TryGetValue(goodName, out var listing))
                {
                    ConsoleExtensions.WithColor($"{goodName} is not traded at {gameState.Player.Location.Planet}.", System.ConsoleColor.Yellow);
                    continue;
                }

                if (!int.TryParse(parts[2], out var amount))
                {
                    ConsoleExtensions.WithColor($"Amount must be a number.", System.ConsoleColor.Yellow);
                    continue;
                }
                if (amount <= 0)
                {
                    ConsoleExtensions.WithColor($"Amount must be positive.", System.ConsoleColor.Yellow);
                    continue;
                }

                var doBuy = parts[0].StartsWith("b");
                if (doBuy)
                {
                    BuyGood(goodName, listing, gameState, amount);
                }

                var doSell = parts[0].StartsWith("s");
                if (doSell)
                {
                    SellGood(goodName, listing, gameState, amount);
                }
            }

            Console.WriteLine("Goodbye");
            Console.ReadKey();
        }

        private static void DisplayHighScores(GameState gameState)
        {
            var highScores = Misc.LoadHighScores();
            highScores.Add(new HighScoreEntry()
            {
                ShipName = gameState.Player.ShipName,
                Score = gameState.Player.Cash,
                Turn = gameState.Player.Turn
            });
            var sortedHighScores = PrintHighScores(highScores);
            Misc.SaveHighScores(sortedHighScores);
        }

        private static HighScoreEntry[] PrintHighScores(List<HighScoreEntry> highScores)
        {
            var sortedHighScores = highScores.OrderBy(x => x.Score)
                .ThenBy(x => x.Turn)
                .ThenBy(x => x.ShipName)
                .Take(10)
                .ToArray();

            ConsoleExtensions.WriteTable(
                sortedHighScores,
                new[] { "Ship", "Score", "Turns" },
                (s) => new[] { s.ShipName, s.Score.ToString(), s.Turn.ToString() }
            );
            return sortedHighScores;
        }

        static void SellGood(string goodName, Listing listing, GameState gameState, int amount)
        {
            gameState.Player.Cargo.Goods.TryGetValue(goodName, out var amountOnShip);
            // cannot sell more than amount on ship
            var amountToSell = Math.Min(amountOnShip, amount);

            gameState.Player.Cash += amountToSell * listing.Rate;
            gameState.Player.Cargo.Goods.AddOrUpdate(goodName, 0, x => x - amountToSell);
            listing.Amount += amountToSell;
        }

        static void BuyGood(string goodName, Listing listing, GameState gameState, int amount)
        {
            var planetAmount = listing.Amount;
            // planet must have some to buy
            if (planetAmount == 0)
            {
                ConsoleExtensions.WithColor($"{goodName} is not for sale at {gameState.Player.Location.Planet}.", System.ConsoleColor.Yellow);
                return;
            }

            // cannot but more than amount on planet
            var amountToBuy = Math.Min(amount, planetAmount);
            var cost = amountToBuy * listing.Rate;
            if (gameState.Player.Cash < cost)
            {
                ConsoleExtensions.WithColor($"You do not have enough cash for the purchase.", System.ConsoleColor.Yellow);
                return;
            }

            if (goodName != "fuel")
            {
                var usedCapacity = gameState.Player.Cargo.Goods.Sum(kvp => kvp.Value);
                if (usedCapacity + amountToBuy > gameState.Player.Cargo.Capacity)
                {
                    ConsoleExtensions.WithColor($"You do not have enough cargo capacity for the purchase.", System.ConsoleColor.Yellow);
                    return;
                }
            
                gameState.Player.Cargo.Goods.AddOrUpdate(goodName, amountToBuy, x => x + amountToBuy);
            } else
            {
                var availableCapacity = gameState.Player.FuelCapacity - gameState.Player.Fuel;
                amountToBuy = Math.Min(amountToBuy, availableCapacity);
                gameState.Player.Fuel += amountToBuy;
            }
            gameState.Player.Cash -= cost;
            listing.Amount -= amountToBuy;
        }

        static void TravelToPlanet(string[] parts, GameState gameState)
        {
            string destination = "";
            if (parts.Length == 2)
            {
                destination = parts[1];
            }
            
            if (destination == "" || !gameState.CurrentSystem.Planets.ContainsKey(destination))
            {
                Console.WriteLine("-----");
                Console.WriteLine("Planets");
                foreach (var entry in gameState.CurrentSystem.Planets.Values)
                {
                    Console.WriteLine($" {entry.Name}");
                }

                return;
            }

            if (gameState.Player.Location.Planet == destination)
            {
                // already on planet
                return;
            }

            var fuelCost = 3;
            if (gameState.Player.Fuel < fuelCost)
            {
                ConsoleExtensions.WithColor($"You do not have enough fuel to travel to {destination}.", System.ConsoleColor.Yellow);
            }
            gameState.Player.Location = new Location
            {
                System = gameState.Player.Location.System,
                Planet = destination
            };
            gameState.Player.Fuel -= fuelCost;

            // update markets, production, consumption
            foreach (var planet in gameState.CurrentSystem.Planets.Values)
            {
                foreach (var listing in planet.Market.Values)
                {
                    listing.Amount += listing.Production;
                    listing.Amount -= listing.Consumption;
                    listing.Amount = Math.Max(0, listing.Amount);
                    listing.Amount = Math.Min(200, listing.Amount);
                }
            }

            gameState.Player.Turn++;
        }
    }
}
