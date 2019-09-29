using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class BreadthFirstSearchRadius
{
    Pos _start;
    float _range;
    ePath _type;

    Dictionary<Pos, Pos> _fastestPath;
    Dictionary<Pos, float> _distanceFromStart;
    List<Pos> _searched;
    int _iter;
    int _maxIter;

    public BreadthFirstSearchRadius(Pos start, RadiusRange radius, int maxIter = 100000)
    {
        _start = start;
        _range = radius._range;
        _type = radius._path;
        _maxIter = maxIter;

        _fastestPath = new Dictionary<Pos, Pos>();
        _distanceFromStart = new Dictionary<Pos, float>();
        _searched = new List<Pos>();
    }

    public List<Pos> GetPosInRadius()
    {
        List<Pos> posInRadius = new List<Pos>();
        if (_range > 0f)
        {
            posInRadius.Add(_start);
            _searched.Add(_start);
            List<Pos> neighborsToSearch = SearchStartNeighbors();
            bool pathFound = false;
            _iter = 0;
            while (!pathFound && _iter < _maxIter)
            {
                Pos neighborToPath = SelectNeighborToPath(neighborsToSearch);
                if (_distanceFromStart[neighborToPath] > _range)
                {
                    pathFound = true;
                }
                else
                {
                    posInRadius.Add(neighborToPath);
                    neighborsToSearch = SearchPathNeighbors(neighborsToSearch, neighborToPath);
                }
                _iter++;
            }
        }
        return posInRadius;
    }
    void UpdatePathCost(Pos p, Pos neighborToPath)
    {
        float newPathCost = Pathfinder.GetMoveCost(p, neighborToPath) + _distanceFromStart[neighborToPath];
        if (!_distanceFromStart.ContainsKey(p) || newPathCost < _distanceFromStart[p])
        {
            _distanceFromStart[p] = newPathCost;
            _fastestPath[p] = neighborToPath;
        }
    }
    List<Pos> SearchPathNeighbors(List<Pos> neighborsToSearch, Pos neighborToPath)
    {
        foreach (Pos p in neighborToPath.neighbors)
        {
            UpdatePathCost(p, neighborToPath);
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
        neighborsToSearch = neighborsToSearch.OrderBy(p => _distanceFromStart[p]).ToList();
        // Pull next pos to search
        Pos thisStep = neighborsToSearch[0];
        // Mark pos as searched
        _searched.Add(thisStep);
        return thisStep;
    }

    List<Pos> SearchStartNeighbors()
    {
        // Create the queue of pos to check
        List<Pos> nextStep = new List<Pos>();
        // Add start pos' neighbors to queue
        foreach (Pos p in _start.neighbors)
        {
            _distanceFromStart[p] = Pathfinder.GetMoveCost(p, _start);
            _fastestPath[p] = _start;
            nextStep.Add(p);
        }
        return nextStep;
    }
}
