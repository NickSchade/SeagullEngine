using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeighborBuilderHex : NeighborBuilder
{
    MapInfo mapInfo;
    public NeighborBuilderHex(MapInfo mi)
    {
        mapInfo = mi;
    }
    public override void SetNeighbors(Pos p)
    {
        p.neighbors = new List<Pos>();

        List<int[]> hexNeighbors = new List<int[]>();
        if (p.gridLoc.y() % 2 == 0)
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


        float x = p.gridLoc.x();
        float y = p.gridLoc.y();
        for (int k = 0; k < hexNeighbors.Count; k++)
        {
            int i = hexNeighbors[k][0];
            int j = hexNeighbors[k][1];
            float X = mapInfo.wrapEW ? (mapInfo.xDim + x + i) % mapInfo.xDim : x + i;
            float Y = mapInfo.wrapNS ? (mapInfo.yDim + y + j) % mapInfo.yDim : y + j;

            Loc l2 = new Loc(X, Y);
            if (mapInfo.pathMap.ContainsKey(l2.key()))
            {
                Pos potentialNeighbor = mapInfo.pathMap[l2.key()];
                if (mapInfo.locations[p]._terrain._type == mapInfo.locations[potentialNeighbor]._terrain._type)
                {
                    p.neighbors.Add(potentialNeighbor);
                }
            }
        }
    }
}
