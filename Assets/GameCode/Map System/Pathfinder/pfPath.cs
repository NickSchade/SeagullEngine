using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pfPath
{
    public Pos _start;
    public Pos _end;
    public List<Pos> _path;
    public float _moveCost;
    public int _iterations;
    public int _maxIterations;
    public pfPath(Pos s, Pos e, List<Pos> p, float pathcost, int iter, int maxIter)
    {
        _start = s;
        _end = e;
        _path = p;
        _moveCost = pathcost;
        _iterations = iter;
        _maxIterations = maxIter;
    }
}
