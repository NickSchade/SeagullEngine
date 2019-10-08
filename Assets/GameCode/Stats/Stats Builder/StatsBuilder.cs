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
        Dictionary<Pos, StatsBuild> build = GetPosViewBuild();
        Dictionary<Pos, StatsExtraction> extraction = GetPosViewExtraction();
        Dictionary<Pos, StatsMilitary> military = GetPosViewMilitary();
        foreach (Pos p in _game._locations.Keys)
        {
            StatsVision v = vision.ContainsKey(p) ? vision[p] : new StatsVision(new Dictionary<Player, eVisibility>());
            StatsControl c = control.ContainsKey(p) ? control[p] : new StatsControl(new Dictionary<Player, bool>());
            StatsBuild b = build.ContainsKey(p) ? build[p] : new StatsBuild(new Dictionary<Player, dStructurePlacement>());
            StatsExtraction e = extraction.ContainsKey(p) ? extraction[p] : new StatsExtraction(new Dictionary<Player, float>());
            StatsMilitary m = military.ContainsKey(p) ? military[p] : new StatsMilitary(new Dictionary<Player, float>());
            Stats view = new Stats(p, v, c, b, e, m);
            views.Add(view);
        }
        return views;
    }
    Dictionary<Pos, StatsVision> GetPosViewVision()
    {
        Dictionary<Pos, Dictionary<Player, eVisibility>> visibility = new Dictionary<Pos, Dictionary<Player, eVisibility>>();
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Pos p in _game._locations.Keys)
        {
            visibility[p] = new Dictionary<Player, eVisibility>();
            foreach (Player player in players)
            {
                visibility[p][player] = eVisibility.Unexplored;
            }
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Vision);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    visibility[location._pos][structure._owner] = eVisibility.Visible;
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
        Dictionary<Pos, Dictionary<Player, bool>> control = new Dictionary<Pos, Dictionary<Player, bool>>();
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Pos p in _game._locations.Keys)
        {
            control[p] = new Dictionary<Player, bool>();
            foreach (Player player in players)
            {
                control[p][player] = false;
            }
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
                    control[location._pos][structure._owner] = true;
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

    
    Dictionary<Pos, StatsBuild> GetPosViewBuild()
    {
        Dictionary<Pos, Dictionary<Player, dStructurePlacement>> buildRaw = new Dictionary<Pos, Dictionary<Player, dStructurePlacement>>();
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Pos p in _game._locations.Keys)
        {
            buildRaw[p] = new Dictionary<Player, dStructurePlacement>();
            foreach (Player player in players)
            {
                buildRaw[p][player] = player._buildQueue.GetStructureInQueueAtPos(p);
            }
        }
        Dictionary<Pos, StatsBuild> build = new Dictionary<Pos, StatsBuild>();
        foreach (Pos p in buildRaw.Keys)
        {
            build[p] = new StatsBuild(buildRaw[p]);
        }
        return build;
    }
    Dictionary<Pos, StatsExtraction> GetPosViewExtraction()
    {
        Dictionary<Pos, Dictionary<Player, float>> extractionRaw = new Dictionary<Pos, Dictionary<Player, float>>();
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Pos p in _game._locations.Keys)
        {
            extractionRaw[p] = new Dictionary<Player, float>();
            foreach (Player player in players)
            {
                extractionRaw[p][player] = 0f;
            }
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Extraction);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    extractionRaw[location._pos][structure._owner] += 1f;
                }
            }
        }
        Dictionary<Pos, StatsExtraction> extractionStats = new Dictionary<Pos, StatsExtraction>();
        foreach (Pos p in extractionRaw.Keys)
        {
            extractionStats[p] = new StatsExtraction(extractionRaw[p]);
        }
        return extractionStats;
    }
    Dictionary<Pos, StatsMilitary> GetPosViewMilitary()
    {
        Dictionary<Pos, Dictionary<Player,float>> military = new Dictionary<Pos, Dictionary<Player,float>>();
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Pos p in _game._locations.Keys)
        {
            military[p] = new Dictionary<Player, float>();
            foreach (Player player in players)
            {
                military[p][player] = 0f;
            }
        }
        foreach (Pos p in _game._locations.Keys)
        {
            HomelandsStructure structure = _game._locations[p]._structure;
            if (structure != null)
            {
                List<HomelandsLocation> visibleLocations = structure.GetLocationsInRadius(eRadius.Military);
                foreach (HomelandsLocation location in visibleLocations)
                {
                    military[location._pos][structure._owner] += 1f;
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
