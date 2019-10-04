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


    public HomelandsLocation(HomelandsGame game, Pos pos, HomelandsTerrain terrain, HomelandsResource resource)
    {
        _pos = pos;
        _game = game;
        _viewer = game._viewer;

        _terrain = terrain;
        _resource = resource;

    }
    HomelandsResource GetResource()
    {
        return _terrain._type == eTerrain.Land && Random.Range(0f, 1f) > 0.90f ? new HomelandsResource() : null;
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

    StructureData GetBasicStructure()
    {
        Dictionary<eRadius, RadiusRange> radii = new Dictionary<eRadius, RadiusRange>();
        radii[eRadius.Vision] = new RadiusRange(ePath.Euclidian, 5f);
        radii[eRadius.Control] = new RadiusRange(ePath.NodeEuclidian, 3f);
        radii[eRadius.Extraction] = new RadiusRange(ePath.NodeEuclidian, 2f);
        radii[eRadius.Military] = new RadiusRange(ePath.NodeEuclidian, 1f);
        StructureData sd = new StructureData(1f, 3f, radii);
        return sd;
    }
    

    public virtual StructurePlacementData TryToMakeStructure()
    {
        StructureData structureToBuild = GetBasicStructure();
        StructurePlacementData sd = null;
        Player currentPlayer = _game._playerSystem.GetPlayer();

        if (_structure == null)
        {
            if (_terrain._type == eTerrain.Land)
            {
                if (_game._stats._numberOfStructures[currentPlayer] <= 0 || _stats._control._controllingPlayers[currentPlayer])
                {
                    sd = currentPlayer._buildQueue.GetStructureToPlace(structureToBuild, this);
                    currentPlayer._buildQueue.AddStructureToBuildQueue(sd);
                    Debug.Log($"{currentPlayer._name} added a structure to the build queue");
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
        return sd;
    }
    public virtual StructurePlacementData Click()
    {
        StructurePlacementData sd = TryToMakeStructure();
        return sd;
    }

    public virtual GraphicsData Draw()
    {
        LocationGraphicsData lgd = _viewer.Draw(this, _stats);
        StructureGraphicsData sgd = null;
        ResourceGraphicsData rgd = null;

        Player currentPlayer = _game._playerSystem.GetPlayer();

        bool visibleToPlayer = _stats._vision._visibility[currentPlayer] == eVisibility.Visible;
        bool visibleBecauseGodMode = _game._viewer._viewType == eView.God;

        if (visibleToPlayer || visibleBecauseGodMode)
        {
            if (_structure != null)
            {
                sgd = _structure.Draw();
            }
            if (_stats._build._buildingsQueued[currentPlayer] != null)
            {
                sgd = new StructureGraphicsData(Color.white);
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

