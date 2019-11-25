using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AstarPathfinder
{
    Dictionary<Pos, float> _distanceFromStart;
    Dictionary<Pos, float> _distanceToEnd;
    Dictionary<Pos, Pos> _fastestPath;
    List<Pos> _searched;
    Pos _start;
    Pos _end;
    int _iter;
    int _maxIter;

    public AstarPathfinder(Pos start, Pos end, int maxIter = 100000)
    {
        _distanceFromStart = new Dictionary<Pos, float>();
        _distanceToEnd = new Dictionary<Pos, float>();
        _fastestPath = new Dictionary<Pos, Pos>();
        _searched = new List<Pos>();
        _start = start;
        _end = end;
        _maxIter = maxIter;
    }
    
    public pfPath GetPath()
    {
        List<Pos> path = new List<Pos>();
        if (_start != _end)
        {
            List<Pos> neighborsToSearch = SearchStartNeighbors();

            bool pathFound = false;
            _iter = 0;
            while (!pathFound && _iter < _maxIter)
            {
                Pos neighborToPath = SelectNeighborToPath(neighborsToSearch);
                if (neighborToPath._neighbors.Contains(_end))
                {
                    pathFound = FinishPath(neighborToPath);
                }
                else
                {
                    neighborsToSearch = SearchPathNeighbors(neighborsToSearch, neighborToPath);
                }
                _iter++;
            }
            path = FinalizePath();
        }
        pfPath pfp = new pfPath(_start, _end, path, Pathfinder.GetCost(path), _iter, _maxIter);
        return pfp;
    }

    List<Pos> SearchStartNeighbors()
    {
        // Create the queue of pos to check
        List<Pos> nextStep = new List<Pos>();
        // Add start pos' neighbors to queue
        foreach (Pos p in _start._neighbors)
        {
            _distanceFromStart[p] = Pathfinder.GetMoveCost(p, _start);
            _distanceToEnd[p] = Pathfinder.GetMinkowskiDistance(p, _end);
            _fastestPath[p] = _start;
            nextStep.Add(p);
        }
        return nextStep;
    }
    List<Pos> SearchPathNeighbors(List<Pos> neighborsToSearch, Pos neighborToPath)
    {
        foreach (Pos p in neighborToPath._neighbors)
        {
            UpdatePathCost(p, neighborToPath);
            if (!_distanceToEnd.ContainsKey(p))
            {
                UpdateDistanceToEnd(p);
            }
            if (!neighborsToSearch.Contains(p) && !_searched.Contains(p))
            {
                neighborsToSearch.Add(p);
            }
        }
        neighborsToSearch.Remove(neighborToPath);
        return neighborsToSearch;
    }

    Pos SelectNeighborToPath(List<Pos> neighborsToSearch)
    {
        // Order queue by distance
        neighborsToSearch = neighborsToSearch.OrderBy(p => _distanceFromStart[p] + _distanceToEnd[p]).ToList();
        // Pull next pos to search
        Pos thisStep = neighborsToSearch[0];
        // Mark pos as searched
        _searched.Add(thisStep);
        return thisStep;
    }

    bool FinishPath(Pos neighborToPath)
    {
        UpdatePathCost(_end, neighborToPath);
        UpdateDistanceToEnd(_end);
        return true;
    }
    void UpdatePathCost(Pos p, Pos neighborToPath)
    {
        float newPathCost = Pathfinder.GetMoveCost(p,neighborToPath) + _distanceFromStart[neighborToPath];
        if (!_distanceFromStart.ContainsKey(p) || newPathCost < _distanceFromStart[p])
        {
            _distanceFromStart[p] = newPathCost;
            _fastestPath[p] = neighborToPath;
        }
    }
    void UpdateDistanceToEnd(Pos p)
    {
        _distanceToEnd[p] = Pathfinder.GetMinkowskiDistance(p, _end);
    }

    List<Pos> FinalizePath()
    {
        List<Pos> path = new List<Pos>();
        Pos pathStep = _end;
        while (pathStep != _start)
        {
            path.Add(pathStep);
            if (_fastestPath.ContainsKey(pathStep))
            {
                pathStep = _fastestPath[pathStep];
            }
            else
            {
                return null;
            }
        }
        path.Add(_start);
        path.Reverse();
        return path;
    }
}


