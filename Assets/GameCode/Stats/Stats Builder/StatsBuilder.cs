using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;

public interface IStatsBuilder
{
    List<Stats> GetPosViews();
}

public class StatsBuilderBasic : IStatsBuilder
{
    HomelandsGame _game;
    public StatsBuilderBasic(HomelandsGame game)
    {
        _game = game;
    }
    public List<Stats> GetPosViews()
    {
        List<Stats> views = new List<Stats>();
        Dictionary<Pos, StatsVision> vision = GetPosViewVision();
        Dictionary<Pos, StatsControl> control = GetPosViewControl();
        Dictionary<Pos, StatsExtraction> extraction = GetPosViewExtraction();
        Dictionary<Pos, StatsMilitary> military = GetPosViewMilitary();
        foreach (Pos p in _game._locations.Keys)
        {
            StatsVision v = vision.ContainsKey(p) ? vision[p] : new StatsVision(eVisibility.Unexplored);
            StatsControl c = control.ContainsKey(p) ? control[p] : new StatsControl(new List<Player>());
            StatsExtraction e = extraction.ContainsKey(p) ? extraction[p] : new StatsExtraction(0f);
            StatsMilitary m = military.ContainsKey(p) ? military[p] : new StatsMilitary(0f);
            Stats view = new Stats(p, v, c, e, m);
            views.Add(view);
        }
        return views;
    }
    Dictionary<Pos, StatsVision> GetPosViewVision()
    {
        Dictionary<Pos, eVisibility> visibility = new Dictionary<Pos, eVisibility>();
        foreach (Pos p in _game._locations.Keys)
        {
            visibility[p] = eVisibility.Unexplored;
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Vision);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    visibility[location._pos] = eVisibility.Visible;
                }
            }
        }

        Dictionary<Pos, StatsVision> visionStats = new Dictionary<Pos, StatsVision>();
        foreach (Pos p in visibility.Keys)
        {
            visionStats[p] = new StatsVision(visibility[p]);
        }
        return visionStats;
    }
    Dictionary<Pos, StatsControl> GetPosViewControl()
    {
        Dictionary<Pos, List<Player>> control = new Dictionary<Pos, List<Player>>();
        foreach (Pos p in _game._locations.Keys)
        {
            control[p] = new List<Player>();
        }
        Player currentPlayer = _game._playerSystem.GetPlayer();
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Control);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    control[location._pos].Add(currentPlayer);
                }
            }
        }

        Dictionary<Pos, StatsControl> controlStats = new Dictionary<Pos, StatsControl>();
        foreach (Pos p in control.Keys)
        {
            controlStats[p] = new StatsControl(control[p]);
        }
        return controlStats;
    }
    Dictionary<Pos, StatsExtraction> GetPosViewExtraction()
    {
        Dictionary<Pos, float> extraction = new Dictionary<Pos, float>();
        foreach (Pos p in _game._locations.Keys)
        {
            extraction[p] = 0f;
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Extraction);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    extraction[location._pos] += 1f;
                }
            }
        }
        Dictionary<Pos, StatsExtraction> extractionStats = new Dictionary<Pos, StatsExtraction>();
        foreach (Pos p in extraction.Keys)
        {
            extractionStats[p] = new StatsExtraction(extraction[p]);
        }
        return extractionStats;
    }
    Dictionary<Pos, StatsMilitary> GetPosViewMilitary()
    {
        Dictionary<Pos, float> military = new Dictionary<Pos, float>();
        foreach (Pos p in _game._locations.Keys)
        {
            military[p] = 0f;
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Military);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    military[location._pos] += 1f;
                }
            }
        }
        Dictionary<Pos, StatsMilitary> militaryStats = new Dictionary<Pos, StatsMilitary>();
        foreach (Pos p in military.Keys)
        {
            militaryStats[p] = new StatsMilitary(military[p]);
        }
        return militaryStats;
    }
}
