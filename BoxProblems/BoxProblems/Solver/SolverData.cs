﻿using BoxProblems.Graphing;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BoxProblems.Solver
{
    public static partial class ProblemSolver
    {
        private class SolverData
        {
            public readonly Dictionary<Point, int> FreePath = new Dictionary<Point, int>();
            public readonly Dictionary<Point, int> RoutesUsed = new Dictionary<Point, int>();
            public List<BoxConflictGraph> SolutionGraphs = new List<BoxConflictGraph>();
            public readonly HashSet<Entity> RemovedEntities = new HashSet<Entity>();
            public readonly GraphSearchData gsData;
            public readonly Level Level;
            public readonly CancellationToken CancelToken;
            public BoxConflictGraph CurrentConflicts;
            public State CurrentState;
            public int Counter = 0;

            public SolverData(Level level, CancellationToken cancelToken)
            {
                this.Level = level;
                this.CancelToken = cancelToken;
                this.CurrentState = level.InitialState;
                this.gsData = new GraphSearchData(level);
            }

            public void AddToFreePath(Point pos)
            {
                if (FreePath.TryGetValue(pos, out int value))
                {
                    FreePath[pos] = value + 1;
                }
                else
                {
                    FreePath.Add(pos, 1);
                }
            }

            public void AddToRoutesUsed(Point[] path)
            {
                foreach (var pos in path)
                {
                    AddToRoutesUsed(pos);
                }
            }

            public void AddToRoutesUsed(Point pos)
            {
                if (RoutesUsed.TryGetValue(pos, out int value))
                {
                    RoutesUsed[pos] = value + 1;
                }
                else
                {
                    RoutesUsed.Add(pos, 1);
                }
            }

            public void RemoveFromFreePath(Point pos)
            {
                int value = FreePath[pos];
                if (value == 1)
                {
                    FreePath.Remove(pos);
                }
                else
                {
                    FreePath[pos] = value - 1;
                }
            }

            public void RemoveFromRoutesUsed(Point[] path)
            {
                foreach (var pos in path)
                {
                    RemoveFromRoutesUsed(pos);
                }
            }

            public void RemoveFromRoutesUsed(Point pos)
            {
                int value = RoutesUsed[pos];
                if (value == 1)
                {
                    RoutesUsed.Remove(pos);
                }
                else
                {
                    RoutesUsed[pos] = value - 1;
                }
            }

            public Entity GetEntity(int index)
            { 
                return CurrentState.Entities[index];
            }

            public int GetEntityIndex(Entity entity)
            {
                return Array.IndexOf(CurrentState.Entities, entity);
            }
        }
    }
}
