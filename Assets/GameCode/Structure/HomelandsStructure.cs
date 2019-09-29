using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsStructure
{
    public HomelandsGame _game;
    public HomelandsLocation _location;
    public Player _owner;
    public Dictionary<eRadius, RadiusRange> _radii;
    float cost = 1f;

    public HomelandsStructure(HomelandsGame game, HomelandsLocation location, Player owner)
    {
        _game = game;
        _location = location;
        _owner = owner;

        _radii = new Dictionary<eRadius, RadiusRange>();
        _radii[eRadius.Vision] = new RadiusRange(ePath.Euclidian, 5f);
        _radii[eRadius.Control] = new RadiusRange(ePath.NodeEuclidian, 3f);
        _radii[eRadius.Extraction] = new RadiusRange(ePath.NodeEuclidian, 2f);
        _radii[eRadius.Military] = new RadiusRange(ePath.NodeEuclidian, 1f);
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
            locationsInRadius = RadiusFinder.GetLocationsInRadius(_location, _radii[radiusType]);
        }

        return locationsInRadius;
    }
}

