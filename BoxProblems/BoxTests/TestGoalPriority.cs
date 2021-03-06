﻿using BoxProblems;
using BoxProblems.Graphing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BoxTests
{
    [TestClass]
    public class TestGoalPriority
    {
        [TestMethod]
        public void TestGoalPrioritySACrunch()
        {
            string levelString = @"
++++++++
+0+  cb+
+ A+ ad+
+ +B+  +
+  DC+ +
+ ++++ +
+      +
++++++++";

            string levelPriorityString = @"
++++++++
+ +  11+
+  + 22+
+ + +  +
+    + +
+ ++++ +
+      +
++++++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority2()
        {
            string levelString = @"
+++++++++++
+++++a+++++
+B c e b C+
+++ +d+ +++
+++  +  +++
++++ d ++++
+++++0+++++
+++++A+++++
++++D D++++
+++++E+++++
+++++++++++";

            string levelPriorityString = @"
+++++++++++
+++++1+++++
+  3 2 3  +
+++ +1+ +++
+++  +  +++
++++ 4 ++++
+++++ +++++
+++++ +++++
++++   ++++
+++++ +++++
+++++++++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority3()
        {
            string levelString = @"
+++++
++B++
++ ++
++ ++
++ ++
+Aba+
+++++";

            string levelPriorityString = @"
+++++
++ ++
++ ++
++ ++
++ ++
+ 21+
+++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority4()
        {
            string levelString = @"
+++++++++++
+a   c   b+
+         +
+A   C   B+
+++++++++++";
            string levelPriorityString = @"
+++++++++++
+1   1   1+
+         +
+         +
+++++++++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriorty5()
        {
            string levelString = @"
++++
+a++
+b++
+c++
+ ++
+ A+
+ B+
+ C+
++++";
            string levelPriorityString = @"
++++
+1++
+2++
+3++
+ ++
+  +
+  +
+  +
++++";
            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority6()
        {
            string levelString = @"
+++++++++
++++A++++
++++b++++
+Ba e cD+
++++d++++
++++C++++
++++E++++
+++++++++";
            string levelPriorityString = @"
+++++++++
++++ ++++
++++1++++
+ 1 2 1 +
++++2++++
++++ ++++
++++ ++++
+++++++++";
            VerifyPriority(levelString, levelPriorityString);

        }

        [TestMethod]
        public void TestGoalPriority7()
        {
            string levelString = @"
+++++++++++++
+ABCDEFGHIJK+
+           +
+abcdefghijk+
+++++++++++++";

            string levelPriorityString = @"
+++++++++++++
+           +
+           +
+11111111111+
+++++++++++++";
            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority8()
        {
            string levelString = @"
+++++
+E+++
+CBD+
++e++
++d++
++c++
+Aba+
+++++";

            string levelPriorityString = @"
+++++
+ +++
+   +
++5++
++4++
++3++
+ 21+
+++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriority9()
        {
            string levelString = @"
++++++++++
+AAAAAAAA+
+  AAAA  +
+   aa   +
+  aaaa  +
+ aaaaaa +
++++++++++";

            string levelPriorityString = @"
++++++++++
+        +
+        +
+   33   +
+  3223  +
+ 321123 +
++++++++++";
            VerifyPriority(levelString, levelPriorityString);
        }

        [TestMethod]
        public void TestGoalPriorityWithAgentGoal()
        {
            string levelString = @"
#domain
hospital
#levelname
Test
#colors
green: 0, A, B, C, D
#initial
++++++++
+0+    +
+ A+   +
+ +B+  +
+  DC+ +
+ ++++ +
+      +
++++++++
#goal
++++++++
+ + 0cb+
+  + ad+
+ + +  +
+    + +
+ ++++ +
+      +
++++++++
#end";

            string levelPriorityString = @"
++++++++
+ + 411+
+  + 32+
+ + +  +
+    + +
+ ++++ +
+      +
++++++++";

            VerifyPriority(levelString, levelPriorityString);
        }

        private static void VerifyPriority(string levelString, string levelPriorityString)
        {
            levelPriorityString = levelPriorityString.Substring(2, levelPriorityString.Length - 2);

            Level level = TestTools.StringToLevel(levelString);
            GraphSearchData gsData = new GraphSearchData(level);
            GoalGraph goalGraph = new GoalGraph(gsData, level.InitialState, level);
            var actualGoalPriority = new GoalPriority(level, goalGraph, new CancellationToken());
            var actualPriorityString = actualGoalPriority.ToLevelString(level);

            string expectedLevel = TestTools.RemoveInvisibleCharacters(levelPriorityString);
            string actualLevel = TestTools.RemoveInvisibleCharacters(actualPriorityString);
            Assert.IsTrue(expectedLevel == actualLevel, $"{Environment.NewLine}Expected:{Environment.NewLine}{levelPriorityString}{Environment.NewLine}{Environment.NewLine}Actual:{Environment.NewLine}{actualPriorityString}");
        }
    }
}
