using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;

public interface IHomelandsPosStatsBuilder
{
    List<HomelandsPosStats> GetPosViews();
}

public class HomelandsPosStatsBuilderBasic : IHomelandsPosStatsBuilder
{
    HomelandsGame _game;
    public HomelandsPosStatsBuilderBasic(HomelandsGame game)
    {
        _game = game;
    }
    public List<HomelandsPosStats> GetPosViews()
    {
        List<HomelandsPosStats> views = new List<HomelandsPosStats>();
        Dictionary<Pos, HomelandsPosStatsVision> vision = GetPosViewVision();
        Dictionary<Pos, HomelandsPosStatsControl> control = GetPosViewControl();
        Dictionary<Pos, HomelandsPosStatsExtraction> extraction = GetPosViewExtraction();
        Dictionary<Pos, HomelandsPosStatsMilitary> military = GetPosViewMilitary();
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsPosStatsVision v = vision.ContainsKey(p) ? vision[p] : new HomelandsPosStatsVision(eVisibility.Unexplored);
            HomelandsPosStatsControl c = control.ContainsKey(p) ? control[p] : new HomelandsPosStatsControl(false);
            HomelandsPosStatsExtraction e = extraction.ContainsKey(p) ? extraction[p] : new HomelandsPosStatsExtraction(0f);
            HomelandsPosStatsMilitary m = military.ContainsKey(p) ? military[p] : new HomelandsPosStatsMilitary(0f);
            HomelandsPosStats view = new HomelandsPosStats(p, v, c, e, m);
            views.Add(view);
        }
        return views;
    }
    Dictionary<Pos, HomelandsPosStatsVision> GetPosViewVision()
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

        Dictionary<Pos, HomelandsPosStatsVision> visionStats = new Dictionary<Pos, HomelandsPosStatsVision>();
        foreach (Pos p in visibility.Keys)
        {
            visionStats[p] = new HomelandsPosStatsVision(visibility[p]);
        }
        return visionStats;
    }
    Dictionary<Pos, HomelandsPosStatsControl> GetPosViewControl()
    {
        Dictionary<Pos, bool> control = new Dictionary<Pos, bool>();
        foreach (Pos p in _game._locations.Keys)
        {
            control[p] = false;
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Control);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    control[location._pos] = true;
                }
            }
        }

        Dictionary<Pos, HomelandsPosStatsControl> controlStats = new Dictionary<Pos, HomelandsPosStatsControl>();
        foreach (Pos p in control.Keys)
        {
            controlStats[p] = new HomelandsPosStatsControl(control[p]);
        }
        return controlStats;
    }
    Dictionary<Pos, HomelandsPosStatsExtraction> GetPosViewExtraction()
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
        Dictionary<Pos, HomelandsPosStatsExtraction> extractionStats = new Dictionary<Pos, HomelandsPosStatsExtraction>();
        foreach (Pos p in extraction.Keys)
        {
            extractionStats[p] = new HomelandsPosStatsExtraction(extraction[p]);
        }
        return extractionStats;
    }
    Dictionary<Pos, HomelandsPosStatsMilitary> GetPosViewMilitary()
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
        Dictionary<Pos, HomelandsPosStatsMilitary> militaryStats = new Dictionary<Pos, HomelandsPosStatsMilitary>();
        foreach (Pos p in military.Keys)
        {
            militaryStats[p] = new HomelandsPosStatsMilitary(military[p]);
        }
        return militaryStats;
    }
}
