﻿using BoxProblems;
using BoxProblems.Graphing;
using BoxProblems.Solver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BoxRunner
{
    class Program
    {
        private static List<string> GetFilePathsFromFolderRecursively(string folderPath)
        {
            List<string> filepaths = new List<string>();
            filepaths.AddRange(Directory.GetFiles(folderPath));

            foreach (var direcotry in Directory.GetDirectories(folderPath))
            {
                filepaths.AddRange(GetFilePathsFromFolderRecursively(direcotry));
            }

            return filepaths;
        }

        private static string GetLevelPath(string levelFileName)
        {
            List<string> files = GetFilePathsFromFolderRecursively("Levels");
            return files.Single(x => Path.GetFileName(x) == levelFileName);
        }

        private static void ConvertFilesToCorrectFormat(string levelPath, string savePath)
        {
            string[] oldFormat = File.ReadAllLines(levelPath);
            if (Level.IsNewFormatLevel(oldFormat))
            {
                File.WriteAllText(savePath, string.Join('\n', oldFormat));
            }
            else
            {
                string[] newFormat = Level.ConvertToNewFormat(oldFormat, Path.GetFileNameWithoutExtension(levelPath));
                File.WriteAllText(savePath, string.Join('\n', newFormat));
            }
        }

        private static void InteractiveConsole()
        {
            File.WriteAllText(communicatorPath, string.Empty);
            Console.WriteLine("Type your commands here:");
            List<string> history = new List<string>() { "Pull(E,N)", "Push(N,W)", "Pull(S,E)", "Push(E,N)" };
            while (true)
            {
                var s = Console.ReadLine();

                if (s == "save")
                    File.WriteAllLines(savePath, history);
                if (s == "load")
                    File.WriteAllLines(communicatorPath, File.ReadAllLines(savePath));
                else if (s.Contains("LRot"))
                {
                    var a = BoxSwimming.LeftHandBoxSwimming(s.Last());
                    history.AddRange(a);
                    File.WriteAllLines(communicatorPath, a);
                }
                else if (s.Contains("RRot"))
                {
                    var a = BoxSwimming.RightHandBoxSwimming(s.Last());
                    history.AddRange(a);
                    File.WriteAllLines(communicatorPath, a);
                }
                else
                {
                    history.Add(s);
                    File.WriteAllText(communicatorPath, s);
                }
            }
        }

        private static void ServerReceiveInteractiveConsole(ServerCommunicator serverCom)
        {
            //serverCom.SendCommands(new string[4] { "Pull(E,N)", "Push(N,W)", "Pull(S,E)", "Push(E,N)" });
            serverCom.SendCommands(new string[1] { "NoOp" });
            string[] s;
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                s = File.ReadAllLines(communicatorPath);
                if (s.Length == 0) continue;
                if (s[0] == "end") break;

                serverCom.SendCommands(s);
                File.WriteAllText(communicatorPath, string.Empty);
            }
        }

        // Set to suitable folders before enabling Interactive Console.
        const string communicatorPath = @"C:\Meine Items\Coding Ambitions\8. Semester\02285 Box Problems\Box Problem Solver\Communicator.txt";
        const string savePath = @"C:\Meine Items\Coding Ambitions\8. Semester\02285 Box Problems\Box Problem Solver\saved.txt";

        static void Main(string[] args)
        {
            ServerCommunicator.SkipConsoleRead = false;
            bool InteractiveConsoleEnable = false; // WARNING: Set const folder paths above before enabling!

            //string levelPath = "MABahaMAS.lvl";
            //string levelPath = "MAExample.lvl";
            //string levelPath = "friendofDFS.lvl";
            //string levelPath = "SAKarlMarx.lvl";
            //string levelPath = "SAExample.lvl";
            //string levelPath = "SACrunch.lvl";
            //string levelPath = "SAAiMasTers.lvl";
            //string levelPath = "SAExample2.lvl";
            //string levelPath = "MAPullPush.lvl";
            //string levelPath = "MAFiveWalls.lvl";
            //string levelPath = "MAPullPush2.lvl";
            //string levelPath = "SABahaMAS.lvl";
            //string levelPath = "MACorridor.lvl";
            //string levelPath = "SAlabyrinthOfStBertin.lvl";
            //string levelPath = "MAKarlMarx.lvl";'
            //string levelPath = "SAVisualKei.lvl";
            string levelPath = "SALeo.lvl";

            string convertedLevelPath = "temp.lvl";

            ServerCommunicator serverCom = new ServerCommunicator();
            if (args.Length == 0 && !ServerCommunicator.SkipConsoleRead)
            {
                levelPath = GetLevelPath(levelPath);
                ConvertFilesToCorrectFormat(levelPath, convertedLevelPath);

                serverCom.StartServer(convertedLevelPath);

                if (InteractiveConsoleEnable)
                    InteractiveConsole();
            }
            else
            {
                ServerCommunicator.GiveGroupNameToServer();

                Level level;
                if (ServerCommunicator.SkipConsoleRead)
                {
                    levelPath = GetLevelPath(levelPath);
                    ConvertFilesToCorrectFormat(levelPath, convertedLevelPath);
                    level = Level.ReadLevel(File.ReadAllLines(convertedLevelPath));
                }
                else
                {
                    level = ServerCommunicator.GetLevelFromServer();
                }

                if (InteractiveConsoleEnable)
                {
                    ServerReceiveInteractiveConsole(serverCom);
                    return;
                }

                var highLevelCommands = ProblemSolver.SolveLevel(level, TimeSpan.FromHours(1), false);
                var lowLevelCommands = serverCom.NonAsyncSolve(level, highLevelCommands);
                //serverCom.SendCommandsSequentially(lowLevelCommands, level);

                var finalCommands = CommandParallelizer.Parallelize(lowLevelCommands, level);
                serverCom.SendCommands(finalCommands);


                return;
                // Michaelius ENDO
            }
        }
    }
}
