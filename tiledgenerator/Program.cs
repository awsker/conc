using System;
using tile;

namespace BGStageGenerator
{
    public class Program
    {
        private static void Main()
        {
            Console.WriteLine("Generating files...");
            var levelGenerator = new LevelGenerator();
            var levels = levelGenerator.Generate();

            LevelSerializer.Serialize(levels);

            Console.Write("Generation complete. Press any key");
            Console.ReadKey();
        }
    }
}
