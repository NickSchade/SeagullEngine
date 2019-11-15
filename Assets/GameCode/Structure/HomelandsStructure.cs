using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsStructure
{
    public HomelandsGame _game;
    public HomelandsLocation _location;
    public Player _owner;
    public Dictionary<eRadius, RadiusRange> _radii;

    float _cost;
    float _hitPoints;
    
    public HomelandsStructure(HomelandsGame game, dStructurePlacement placementData)
    {
        _game = game;
        _location = placementData.location;
        _owner = placementData.creator;

        _cost = placementData.data.cost;
        _hitPoints = placementData.data.hitPoints;
        _radii = placementData.data.radii;
    }
    public string Describe()
    {
        List<string> stats = new List<string>();

        stats.Add("H:" + _hitPoints);
        stats.Add("C:" + _cost);
        foreach (eRadius rad in _radii.Keys)
        {
            stats.Add(rad.ToString()[0] + ":" + _radii[rad]._range.ToString());
        }

        string desc = string.Join(";", stats);

        desc = _owner._name;
        return desc;
    }
    public void TakeDamage(float damageAmount)
    {
        if (damageAmount >= _hitPoints)
        {
            DestroyThis();
        }
    }
    public void DestroyThis()
    {
        Debug.Log("Destroyed This Structure");
        _location._structure = null;
    }
    public StructureGraphicsData Draw()
    {
        return new StructureGraphicsData(_owner._color);
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

