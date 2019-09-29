using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsLocation
{
    public Pos _pos;
    public HomelandsGame _game { get; }

    public HomelandsStructure _structure { get; set; }
    public Viewer _viewer { get; set; }
    public eTerrain _terrain;
    public Stats _stats;


    public HomelandsLocation(HomelandsGame game, Pos pos, Dictionary<string, float> locationQualities)
    {
        _pos = pos;
        _game = game;
        _viewer = game._viewer;

        _terrain = GetTerrainFromQualities(locationQualities);
    }
    eTerrain GetTerrainFromQualities(Dictionary<string, float> locationQualities)
    {
        float elevation = locationQualities["Elevation"];
        if (elevation < 0.4)
        {
            return eTerrain.Sea;
        }
        else if (elevation < 0.8)
        {
            return eTerrain.Land;
        }
        else
        {
            return eTerrain.Mountain;
        }
    }

    public virtual void TryToMakeStructure()
    {
        if (_structure == null)
        {
            if (_terrain == eTerrain.Land)
            {
                Player currentPlayer = _game._playerSystem.GetPlayer();
                if (_game._stats._numberOfStructures == 0 || _stats._control._controllingPlayers.Contains(currentPlayer))
                {
                    _structure = StructureFactory.Make(_game, this, currentPlayer);
                }
                else
                {
                    Debug.Log("Can't Build - Not Controlled");
                }
            }
            else
            {
                Debug.Log("Can't Build - Unbuildable");
            }
            
        }
        else
        {
            Debug.Log("Can't Build - Occupied");
        }
    }
    public virtual void Click()
    {
        TryToMakeStructure();
    }

    public virtual GraphicsData Draw()
    {
        LocationGraphicsData lgd = GetLocationData(_stats);
        StructureGraphicsData sgd = GetStructureData(_stats);

        GraphicsData gd = new GraphicsData(_pos, lgd, sgd);

        return gd;
    }

    StructureGraphicsData GetStructureData(Stats stats)
    {
        if (_structure == null)
        {
            return null;
        }
        else
        {
            return _structure.Draw();
        }
    }
    LocationGraphicsData GetLocationData(Stats stats)
    {
        return _viewer.Draw(this, stats);
    }
}

