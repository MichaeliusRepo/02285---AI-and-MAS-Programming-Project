﻿using BoxProblems.Solver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BoxProblems
{
    public class ServerCommunicator
    {
        const string strategy = "-astar";

        //const string levelPath = "MAKarlMarx.lvl";
        //public static string levelPath = @"Levels\New_Format\SABahaMAS.lvl";

        //public static string levelPath = @"Levels\New_Format\MAExample.lvl";
        //public static string levelPath = @"Levels\New_Format\SAExample.lvl";
        public static string levelPath = @"Levels\New_Format\SACrunch.lvl";
        //public static string levelPath = @"Levels\New_Format\SAExample2.lvl";
        //public static string levelPath = @"Levels\New_Format\MAPullPush.lvl";
        //public static string levelPath = @"Levels\New_Format\MAFiveWalls.lvl";
        //public static string levelPath = @"Levels\New_Format\MAPullPush2.lvl";
        //public static string levelPath = @"Levels\New_Format\SABahaMAS.lvl";
        //public static string levelPath = @"Levels\New_Format\MACorridor.lvl";
        //public static string levelPath = @"Levels\New_Format\SAlabyrinthOfStBertin.lvl"; //MABahaMAS.lvl";
        //public static string levelPath = @"Levels\New_Format\MAKarlMarx.lvl";



        public static bool SkipConsoleRead = false;

        public ServerCommunicator() { }
        public ServerCommunicator(List<List<HighlevelMove>> highlevelMoves)
        {

            List<HighlevelMove> allMoves = new List<HighlevelMove>();

            foreach (List<HighlevelMove> list in highlevelMoves)
                allMoves.AddRange(list);

            NaiveSolver.plan = allMoves;
        }

        public void Run(string[] args)
        {
            if (args.Length == 0)
                System.Diagnostics.Process.Start("cmd.exe", $"/c start powershell.exe java -jar server.jar -l {levelPath} -c 'dotnet BoxRunner.dll {strategy}' -g 150 -t 300");
            else
            {
                PrintMap(); // Michaelius: With the new solver, everything messes up if I don't print this. DON'T ASK, I DON'T KNOW WHY

                // Pick one!
                NonAsyncSolve();
                //AsyncSolve();
            }
        }

        public void NonAsyncSolve()
        {
            //var solver = new NaiveSolver(Level.ReadLevel(File.ReadAllLines(levelPath)));
            var solver = new LessNaiveSolver(Level.ReadLevel(File.ReadAllLines(levelPath)), NaiveSolver.plan);
            solver.Solve(); // A most convenient function.
        }

        public void AsyncSolve()
        {
            Level level = Level.ReadLevel(File.ReadAllLines(levelPath));
            List<Level> levels = LevelSplitter.SplitLevel(level);
            NaiveSolver.totalAgentCount = level.AgentCount;

            // This is the most disgusting data structure I've ever had the honour of writing.
            var allResults = new ConcurrentBag<List<string[]>>();

            Parallel.ForEach(levels, (currentLevel) =>
            {
                var solver = new NaiveSolver(currentLevel);
                allResults.Add(solver.AsyncSolve());
            });

            AssembleCommands(level.AgentCount, allResults.ToList());
        }

        // Iterates over each solved level, picks out first command, assembles those commands and sends to server. Repeat until fully solved.
        public void AssembleCommands(int agentCount, List<List<string[]>> results)
        {
            var commands = new string[agentCount];
            while (results.Count != 0)
            {
                for (int i = 0; i < commands.Length; ++i) commands[i] = NoOp(); // Default
                for (int i = 0; i < results.Count; ++i)
                {
                    var result = results[i];
                    if (result.Count == 0)
                    {
                        results.Remove(result);
                        i--;
                    }
                    else
                    {
                        for (int j = 0; j < agentCount; ++j)
                            if (result[0][j] != null)
                                commands[j] = result[0][j];
                        result.RemoveAt(0);
                    }
                }
                if (results.Count != 0)
                    Command(commands);
            }
        }

        public void PrintMap()
        {
            Console.Error.WriteLine("C# Client initialized.");
            Console.WriteLine(); // Input to trigger Java client to respond.

            string line;
            while ((line = Console.ReadLine()) != "#end")
                Console.Error.WriteLine(line); // Print map input.
            Console.Error.WriteLine(line + "\n End of map file. \n");
        }

        public void ExampleCommands()
        {
            for (int i = 0; i < 7; ++i)
                Command(new string[] { Move(Direction.W), Move(Direction.E) }); // Accepts string arrays, functions with enum
            for (int i = 0; i < 2; ++i)
                Command(new List<string> { Move('S'), Move('N') }); // Accepts string lists, functions with chars
            for (int i = 0; i < 7; ++i)
                Command("Move(E);Move(W)"); // Accepts direct string commands.

            Command("NoOp;" + NoOp());
        }

        public static string Command(string command)
        {
            Console.WriteLine(command);
            if (SkipConsoleRead) return string.Empty;
            string response = Console.ReadLine();

            Console.Error.WriteLine("COMMAND: " + command + "\nRESPONSE: " + response);
            return response;
        }

        internal static string Command(string[] commands) { return Command(String.Join(';', commands)); }
        internal static string Command(List<string> commands) { return Command(String.Join(';', commands)); }

        internal static string NoOp() { return "NoOp"; }
        internal static string Move(Direction agentDirection) { return "Move(" + agentDirection.ToString() + ")"; }
        internal static string Push(Direction agentDirection, Direction boxDirection) { return "Push(" + agentDirection.ToString() + "," + boxDirection.ToString() + ")"; }
        internal static string Pull(Direction agentDirection, Direction boxDirection) { return "Pull(" + agentDirection.ToString() + "," + boxDirection.ToString() + ")"; }

        internal static string Move(char agentDirection) { return "Move(" + agentDirection + ")"; }
        internal static string Push(char agentDirection, char boxDirection) { return "Push(" + agentDirection + "," + boxDirection + ")"; }
        internal static string Pull(char agentDirection, char boxDirection) { return "Pull(" + agentDirection + "," + boxDirection + ")"; }

    }
}