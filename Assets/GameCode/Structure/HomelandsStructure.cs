using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsStructure
{
    public HomelandsGame _game;
    public HomelandsLocation _location;
    public Dictionary<eRadius, RadiusRange> _radii;
    float cost = 1f;

    public HomelandsStructure(HomelandsGame game, HomelandsLocation location)
    {
        _game = game;
        _location = location;

        _radii = new Dictionary<eRadius, RadiusRange>();
        _radii[eRadius.Vision] = new RadiusRange(ePath.Euclidian, 5f);
        _radii[eRadius.Control] = new RadiusRange(ePath.Euclidian, 3f);
        _radii[eRadius.Extraction] = new RadiusRange(ePath.Euclidian, 2f);
        _radii[eRadius.Military] = new RadiusRange(ePath.Euclidian, 1f);
    }

    public void Click()
    {
        throw new System.NotImplementedException();
    }
    public StructureGraphicsData Draw()
    {
        return new StructureGraphicsData(Color.white);
    }
    public List<HomelandsLocation> GetLocationsInRadius(eRadius radiusType)
    {
        List<HomelandsLocation> locationsInRadius = new List<HomelandsLocation>();
        if (_radii.ContainsKey(radiusType))
        {
            RadiusRange radiusRange = _radii[radiusType];
            locationsInRadius = GetLocationsInRadius(radiusRange);
        }

        return locationsInRadius;
    }
    public List<HomelandsLocation> GetLocationsInRadius(RadiusRange radiusRange)
    {
        ePath radiusPath = radiusRange._path;
        float radiusValue = radiusRange._range;
        if (radiusPath == ePath.Euclidian)
        {
            return GetLocationsInEuclidian(radiusValue);
        }
        else if (radiusPath == ePath.NodeUniform)
        {
            return GetLocationsInNodeUniform(radiusValue);
        }
        else if (radiusPath == ePath.NodeEuclidian)
        {
            return GetLocationsInNodeEuclidian(radiusValue);
        }
        else if (radiusPath == ePath.NodeWeight)
        {
            return GetLocationsInNodeWeight(radiusValue);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    List<HomelandsLocation> GetLocationsInNodeUniform(float radiusRange)
    {
        throw new System.NotImplementedException();
    }
    List<HomelandsLocation> GetLocationsInNodeEuclidian(float radiusRange)
    {
        throw new System.NotImplementedException();
    }
    List<HomelandsLocation> GetLocationsInNodeWeight(float radiusRange)
    {
        throw new System.NotImplementedException();
    }
    List<HomelandsLocation> GetLocationsInEuclidian(float radiusRange)
    {
        List<HomelandsLocation> locations = new List<HomelandsLocation>();

        Loc l = _location._pos.mapLoc;
        float x = l.x();
        float y = l.y();

        foreach (HomelandsLocation location in _game._locations.Values)
        {
            float x2 = location._pos.mapLoc.x();
            float y2 = location._pos.mapLoc.y();
            if (Mathf.Sqrt(Mathf.Pow(x - x2, 2) + Mathf.Pow(y - y2, 2)) < radiusRange)
            {
                locations.Add(location);
            }
        }

        return locations;
    }

}

