using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Pathfinder
{
    public static List<Pos> findAStarPath(Pos start, Pos end, int maxIter = 100000)
    {
        Dictionary<Pos, float> DistanceFromStart = new Dictionary<Pos, float>();
        Dictionary<Pos, float> DistanceToEnd = new Dictionary<Pos, float>();
        Dictionary<Pos, Pos> FastestPath = new Dictionary<Pos, Pos>();
        List<Pos> Searched = new List<Pos>();


        List<Pos> path = new List<Pos>();
        if (start != end)
        {
            // Create the queue of pos to check
            List<Pos> nextStep = new List<Pos>();
            // Add start pos' neighbors to queue
            foreach (Pos p in start.neighbors)
            {
                DistanceFromStart[p] = p.getMoveToCost(start);
                DistanceToEnd[p] = Pos.DistanceMinkowski(p, end);
                FastestPath[p] = start;
                nextStep.Add(p);
            }

            bool pathFound = false;
            int iter = 0;
            while (!pathFound && iter < maxIter)
            {
                // Order queue by distance
                nextStep = nextStep.OrderBy(p => DistanceFromStart[p] + DistanceToEnd[p]).ToList();
                // Pull next pos to search
                Pos thisStep = nextStep[0];
                //Debug.Log("thisStep is at " + thisStep.loc.x + " , " + thisStep.loc.y);
                // Mark pos as searched
                Searched.Add(thisStep);
                if (thisStep.neighbors.Contains(end))
                {
                    pathFound = true;
                    Pos p = end;
                    float newPathCost = p.getMoveToCost(thisStep) + DistanceFromStart[thisStep];
                    if (!DistanceFromStart.ContainsKey(p) || newPathCost < DistanceFromStart[p])
                    {
                        DistanceFromStart[p] = newPathCost;
                        FastestPath[p] = thisStep;
                    }
                    if (DistanceToEnd.ContainsKey(p))
                    {
                        DistanceToEnd[p] = Pos.DistanceMinkowski(p, end);
                    }
                }
                else
                {
                    foreach (Pos p in thisStep.neighbors)
                    {
                        float newPathCost = p.getMoveToCost(thisStep) + DistanceFromStart[thisStep];
                        if (!DistanceFromStart.ContainsKey(p) || newPathCost < DistanceFromStart[p])
                        {
                            DistanceFromStart[p] = newPathCost;
                            FastestPath[p] = thisStep;
                        }
                        if (!DistanceToEnd.ContainsKey(p))
                        {
                            DistanceToEnd[p] = Pos.DistanceMinkowski(p, end);
                        }
                        if (!nextStep.Contains(p) && !Searched.Contains(p))
                        {
                            nextStep.Add(p);
                            //Debug.Log("Added to nextStep Pos at " + p.loc.x + " , " + p.loc.y);
                        }
                    }
                    nextStep.Remove(thisStep);
                    //Debug.Log("Removed from nextStep Pos at " + thisStep.loc.x + " , " + thisStep.loc.y);
                }
                iter++;
            }
            //Debug.Log("Completed with " + iter + " / " + maxIter + " iterations.");

            Pos pathStep = end;
            while (pathStep != start)
            {
                path.Add(pathStep);
                if (FastestPath.ContainsKey(pathStep))
                {
                    pathStep = FastestPath[pathStep];
                }
                else
                {
                    return null;
                }
            }
            path.Add(start);
            path.Reverse();

        }
        return path;
    }
}
