using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct Loc
{
    public float[] coordinates;
    public Loc(float _x, float _y, float _z = 0)
    {
        coordinates = new float[] { _x, _y, _z };
    }
    public Loc(float[] _coordinates)
    {
        coordinates = _coordinates;
    }
    public Loc(string _locKey)
    {
        coordinates = _locKey.Split(',').Select(x => float.Parse(x)).ToArray();
    }
    public string key()
    {
        //return coordinates.Select(a => a.ToString()).Aggregate((i, j) => i + "," + j);
        string[] k = coordinates.Select(x => x.ToString()).ToArray();
        return string.Join(",", k);
    }
    public float x()
    {
        try
        {
            return coordinates[0];
        }
        catch
        {
            return 0;
        }
    }
    public float y()
    {
        try
        {
            return coordinates[1];
        }
        catch
        {
            return 0;
        }
    }
    public float z()
    {
        try
        {
            return coordinates[2];
        }
        catch
        {
            return 0;
        }
    }
    public static Loc SquareToCube(Loc squareLoc)
    {
        float x = squareLoc.y() - (squareLoc.x() - (squareLoc.x() % 2f)) / 2f;
        float z = squareLoc.x();
        float y = -x - z;
        return new Loc(x, y, z);
    }
    public static Loc CubeToSquare(Loc cubeLoc)
    {
        float x = cubeLoc.z();
        float y = cubeLoc.x() + (cubeLoc.z() - (cubeLoc.z() % 2f)) / 2;
        return new Loc(x, y);
    }

}
