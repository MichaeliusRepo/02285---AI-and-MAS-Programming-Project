﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BoxProblems.Graphing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[assembly: InternalsVisibleTo("BoxTests")]
namespace BoxProblems
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += (_, __) => ReleaseResources();

            //string oldFormatPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels", "Old_Format");
            //string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Levels", "Old_To_New_Format");

            //if (!Directory.Exists(savePath))
            //{
            //    Directory.CreateDirectory(savePath);
            //}

            //ConvertFiles(oldFormatPath, "", savePath);
            //Console.WriteLine("Converting done");
            //Console.Read();
            //return;

            //if (args.Length == 0)
            //{
            //    string strategy = "-astar";
            //    string level = @"Levels\Old_Format\real_levels\MAKarlMarx.lvl";
            //    string asd = $"/c start powershell.exe java -jar server.jar -l {level} -c 'dotnet BoxProblems.dll {strategy}' -g 150 -t 300";
            //    System.Diagnostics.Process.Start("cmd.exe", asd);
            //}

            //string levelString = @"+++++++++++
            //+++++++++++
            //+++++a+++++
            //+B c e b C+
            //+++ +d+ +++
            //+++  +  +++
            //++++ d ++++
            //+++++0+++++
            //+++++A+++++
            //++++D D++++
            //+++++E+++++
            //+++++++++++";

            ////Level level = Level.ReadLevel(File.ReadAllLines("Levels/New_Format/SplitExample2.lvl"));
            //Level level = Level.ReadOldFormatLevel(levelString.Replace("\r", "").Split('\n'), "asdas");// File.ReadAllLines("Levels/Old_Format/initial_levels/SAtowersOfSaigon10.lvl"), "asdas");

            //Level wholeLevel = Level.ReadOldFormatLevel(File.ReadAllLines("Levels/Old_Format/real_levels/MAKarlMarx.lvl"), "asdas");
            //Level wholeLevel = Level.ReadLevel(File.ReadAllLines("Levels/Old_Format/initial_levels/SAsoko3_64.lvl"));
            ////Level wholeLevel = Level.ReadLevel(File.ReadAllLines("Levels/New_Format/SplitExample1.lvl"));

            //var solution = ProblemSolver.SolveLevel(wholeLevel);

            //for (int i = 0; i < solution.First().solutionGraphs.Count; i++)
            //{
            //    State state = solution.First().solutionGraphs[i].CreatedFromThisState;
            //    Console.WriteLine(wholeLevel.StateToString(state));
            //    Console.WriteLine();
            //    //GraphShower.ShowGraph(solution.First().solutionGraphs[i]);
            //    //Thread.Sleep(5000);
            //}




            //var goalGraphs = LevelSplitter.SplitLevel(wholeLevel).Select(x => new GoalGraph(x.InitialState, x))
            //                                                     .ToArray();
            //BoxConflictGraph sdf = new BoxConflictGraph(wholeLevel.InitialState, wholeLevel);
            //sdf.AddFreeNodes(wholeLevel, wholeLevel.InitialState.Entities.Single(x => x.Type == 'B').Pos, wholeLevel.Goals.Single(x => x.Type == 'B').Pos);
            //GraphShower.ShowGraph(sdf);
            //GraphShower.ShowSimplifiedGraphs(sdf);

            //List<Level> levels = LevelSplitter.SplitLevel(wholeLevel);
            //foreach (var level in levels)
            //{
            //    Console.WriteLine(level);
            //    GoalGraph graph = new GoalGraph(level.InitialState, level);
            //    //GraphShower.ShowGraph(graph);
            //    var priority = new GoalPriority(level, graph);
            //    Console.WriteLine(priority);
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
            Console.Read();
        }

        private static void ReleaseResources()
        {
            GraphShower.Shutdown();
        }
    }
}
