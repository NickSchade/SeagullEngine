using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsLocation
{
    public Pos _pos;
    public HomelandsGame _game { get; }

    public HomelandsStructure _structure { get; set; }
    public HomelandsResource _resource { get; set; }
    public HomelandsTerrain _terrain { get; set; }
    public Viewer _viewer { get; set; }
    public Stats _stats;


    public HomelandsLocation(HomelandsGame game, Pos pos, Dictionary<string, float> locationQualities)
    {
        _pos = pos;
        _game = game;
        _viewer = game._viewer;

        _terrain = GetTerrain(locationQualities);
        _resource = GetResource();

    }
    HomelandsResource GetResource()
    {
        if (_terrain._type == eTerrain.Land && Random.Range(0f, 1f) > 0.90f)
        {
            return new HomelandsResource();
        }
        else
        {
            return null;
        }
    }
    HomelandsTerrain GetTerrain(Dictionary<string, float> locationQualities)
    {
        eTerrain type = GetTerrainTypeFromQualities(locationQualities);
        HomelandsTerrain terrain = new HomelandsTerrain(type);
        return terrain;
    }
    eTerrain GetTerrainTypeFromQualities(Dictionary<string, float> locationQualities)
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
        float structureCost = 1f;
        Player currentPlayer = _game._playerSystem.GetPlayer();

        if (_structure == null)
        {
            if (_terrain._type == eTerrain.Land)
            {
                if (_game._stats._numberOfStructures[currentPlayer] <= 0 || _stats._control._controllingPlayers[currentPlayer])
                {
                    if (currentPlayer._resources.CanPay(structureCost))
                    {
                        currentPlayer._resources.Pay(structureCost);
                        _structure = StructureFactory.Make(_game, this, currentPlayer);
                        Debug.Log($"{currentPlayer._name} paid {structureCost} to build a structure");
                    }
                    else
                    {
                        Debug.Log($"{currentPlayer._name} can't Build - Insufficient Resources");
                    }
                }
                else
                {
                    Debug.Log($"{currentPlayer._name} can't Build - Not Controlled");
                }
            }
            else
            {
                Debug.Log($"{currentPlayer._name} can't Build - Unbuildable");
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
        LocationGraphicsData lgd = _viewer.Draw(this, _stats);
        StructureGraphicsData sgd = null;
        ResourceGraphicsData rgd = null;

        bool visibleToPlayer = _stats._vision._visibility[_game._playerSystem.GetPlayer()] == eVisibility.Visible;
        bool visibleBecauseGodMode = _game._viewer._viewType == eView.God;

        if (visibleToPlayer || visibleBecauseGodMode)
        {
            if (_structure != null)
            {
                sgd = _structure.Draw();
            }
            if (_resource != null)
            {
                rgd = _resource.Draw();
            }
        }

        GraphicsData gd = new GraphicsData(_pos, lgd, sgd, rgd);

        return gd;
    }
    
    
}

