using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Trader.Models;

namespace Trader.IO
{
    class Misc
    {
        public static GameState LoadGame(string[] args)
        {
            string text;
            if (args.Length == 2)
            {
                var fileName = args[1];
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("File does not exist.");
                    return null;
                }

                Console.WriteLine($"Loading from {fileName}...");
                text = File.ReadAllText(fileName);
            }
            else
            {
                var fileName = "save.json";
                if (!File.Exists(fileName))
                {
                    fileName = "universe.json";
                }
                Console.WriteLine($"Loading from {fileName}...");
                text = File.ReadAllText(fileName);
            }

            //var traceWriter = new Newtonsoft.Json.Serialization.MemoryTraceWriter();
            //var gameState = JsonConvert.DeserializeObject<GameState>(text, new Newtonsoft.Json.JsonSerializerSettings
            //{
            //    TraceWriter = traceWriter
            //});
            //Console.WriteLine(traceWriter.ToString());
            var gameState = JsonConvert.DeserializeObject<GameState>(text);
            return gameState;
        }

        public static void SaveGame(GameState gameState)
        {
            if (File.Exists("save.json"))
            {
                Console.WriteLine("Backing up save...");
                File.Move("save.json", "save.bak", true);
            }
            Console.WriteLine("Saving...");
            File.WriteAllText("save.json", JsonConvert.SerializeObject(gameState));
        }

        public static List<HighScoreEntry> LoadHighScores()
        {
            List<HighScoreEntry> highScores;
            if (File.Exists("highscores.json"))
            {
                var highScoresText = File.ReadAllText("highscores.json");
                highScores = JsonConvert.DeserializeObject<List<HighScoreEntry>>(highScoresText);
                File.Move("highscores.json", "highscores.bak", true);
            }
            else
            {
                highScores = new List<HighScoreEntry>();
            }
            return highScores;
        }

        public static void SaveHighScores(HighScoreEntry[] sortedHighScores)
        {
            var sortedHighScoresText = JsonConvert.SerializeObject(sortedHighScores);
            File.WriteAllText("highscores.json", sortedHighScoresText);
        }
    }
}
