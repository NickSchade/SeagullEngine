using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Pathfinder
{
    public static float GetMinkowskiDistance(Pos start, Pos end, float minkowski = 2)
    {
        float pSum = 0f;
        for (int i = 0; i < start.mapLoc.coordinates.Length; i++)
        {
            pSum += Mathf.Pow(Mathf.Abs(start.mapLoc.coordinates[i] - end.mapLoc.coordinates[i]), minkowski);
        }
        return Mathf.Pow(pSum, (1 / minkowski));
    }

    public static float GetMoveCost(Pos start, Pos end)
    {
        if (start.neighbors.Contains(end))
        {
            float distance = GetMinkowskiDistance(start, end);
            return distance;
        }
        else
        {
            return float.PositiveInfinity;
        }
    }

    public static float GetCost(List<Pos> path)
    {
        float d = 0f;
        for (int i = 1; i < path.Count; i++)
        {
            d += GetMoveCost(path[i], path[i - 1]);
        }
        return d;
    }

    public static List<Pos> GetPosInRadius(List<Pos> allPos, Pos p, RadiusRange radius)
    {
        if (radius._path == ePath.Euclidian)
        {
            return GetPosInEuclidianRadius(allPos, p, radius._range);
        }
        else if (radius._path == ePath.NodeEuclidian)
        {
            return GetPosInPathRange(p, radius);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }

    public static List<Pos> GetPosInPathRange(Pos p, RadiusRange radiusRange)
    {
        BreadthFirstSearchRadius bfsr = new BreadthFirstSearchRadius(p, radiusRange);
        List<Pos> posInRadius = bfsr.GetPosInRadius();
        return posInRadius;
    }

    public static List<Pos> GetPosInEuclidianRadius(List<Pos> allPos, Pos p, float radiusRange)
    {
        List<Pos> posInRadius = new List<Pos>();

        foreach (Pos p2 in allPos)
        {
            float euclidDistance = GetMinkowskiDistance(p, p2);
            if (euclidDistance < radiusRange)
            {
                posInRadius.Add(p2);
            }
        }

        return posInRadius;
    }
}