using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pos
{
    public Loc gridLoc;
    public Loc mapLoc;
    public List<Pos> neighbors;

    public Pos(Loc _loc, eTileShape _tileShape)
    {
        gridLoc = _loc;
        setMapLoc(_tileShape);
        neighbors = new List<Pos>();
    }
    public void setMapLoc(eTileShape _tileShape)
    {
        switch (_tileShape)
        {
            case eTileShape.Square:
                mapLoc = new Loc(gridLoc.coordinates);
                break;
            case eTileShape.Hex:
                float x = Mathf.Sqrt(3) * (gridLoc.x() - 0.5f * (gridLoc.y() % 2f)) / 1.9f;
                float y = (3 / 2) * gridLoc.y() / 1.3f;
                mapLoc = new Loc(x, y);
                break;
        }
    }

    public static float DistanceMinkowski(Pos p1, Pos p2, float d = 2)
    {
        float pSum = 0f;
        for (int i = 0; i < p1.mapLoc.coordinates.Length; i++)
        {
            pSum += Mathf.Pow(Mathf.Abs(p1.mapLoc.coordinates[i] - p2.mapLoc.coordinates[i]), d);
        }
        return Mathf.Pow(pSum, (1 / d));
    }

    public float getMoveToCost(Pos moveFrom)
    {
        if (neighbors.Contains(moveFrom))
        {
            float distance = DistanceMinkowski(this, moveFrom);
            return distance;
        }
        else
        {
            return float.PositiveInfinity;
        }
    }
    public static float DistancePath(Pos p1, Pos p2)
    {
        List<Pos> path = Pathfinder.findAStarPath(p1, p2);
        float d = 0f;
        for (int i = 1; i < path.Count; i++)
        {
            d += path[i].getMoveToCost(path[i - 1]);
        }
        return d;
    }
    public List<Pos> findPath(Pos pathTarget)
    {
        List<Pos> path = Pathfinder.findAStarPath(this, pathTarget);
        return path;
    }
    public string getName()
    {
        return "[" + gridLoc.x() + "," + gridLoc.y() + "]";
    }
    public string listNeighbors()
    {
        string[] neighborsList = neighbors.Select(n => n.getName()).ToArray();
        return string.Join(",", neighborsList);
    }


    //public void SetNeighbors(MarinusMap map, TileShape tileShape)
    //{
    //    if (tileShape == TileShape.SQUARE)
    //    {
    //        SetNeighborsSquare(map);
    //    }
    //    else
    //    {
    //        SetNeighborsHex(map);
    //    }
    //}
    //private void SetNeighborsSquare(MarinusMap map)
    //{
    //    SetNeighborsSquare(map.pathMap, map.xDim, map.yDim, map.wrapEastWest, map.wrapNorthSouth);
    //}
    //private void SetNeighborsHex(MarinusMap map)
    //{
    //    SetNeighborsHex(map.pathMap, map.xDim, map.yDim, map.wrapEastWest, map.wrapNorthSouth);
    //}
    private void SetNeighborsHex(Dictionary<string, Pos> pathMap, int xDim, int yDim, bool wrapEastWest, bool wrapNorthSouth)
    {
        neighbors = new List<Pos>();

        List<int[]> hexNeighbors = new List<int[]>();
        if (gridLoc.y() % 2 == 0)
        {
            hexNeighbors.Add(new int[] { 1, 0 });
            hexNeighbors.Add(new int[] { 1, -1 });
            hexNeighbors.Add(new int[] { 0, -1 });
            hexNeighbors.Add(new int[] { -1, 0 });
            hexNeighbors.Add(new int[] { 0, 1 });
            hexNeighbors.Add(new int[] { 1, 1 });
        }
        else
        {

            hexNeighbors.Add(new int[] { 1, 0 });
            hexNeighbors.Add(new int[] { -1, -1 });
            hexNeighbors.Add(new int[] { 0, -1 });
            hexNeighbors.Add(new int[] { -1, 0 });
            hexNeighbors.Add(new int[] { 0, 1 });
            hexNeighbors.Add(new int[] { -1, 1 });
        }


        float x = gridLoc.x();
        float y = gridLoc.y();
        for (int k = 0; k < hexNeighbors.Count; k++)
        {
            int i = hexNeighbors[k][0];
            int j = hexNeighbors[k][1];
            float X = wrapEastWest ? (xDim + x + i) % yDim : x + i;
            float Y = wrapNorthSouth ? (yDim + y + j) % yDim : y + j;

            Loc l2 = new Loc(X, Y);
            if (pathMap.ContainsKey(l2.key()))
            {
                neighbors.Add(pathMap[l2.key()]);
            }
            else
            {
                Debug.Log("Map doesn't contain " + l2.x() + "," + l2.y());
            }
        }
    }
}

