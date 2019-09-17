using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract partial class HomelandsGame
{
    protected GameManager _gameManager;

    public eGame _gameType { get; set; }
    public eTileShape _tileShape { get; set; }

    public IKeyHandler _keyHandler { get; set; }
    public IMouseHandler _mouseHandler { get; set; }


    public HomelandsViewer _viewer;
    public ILocationDrawer _locationDrawer { get; set; }
    public Dictionary<Pos, ILocation> _locations;

    public ITickSystem _tickSystem;


    public HomelandsGame(GameManager gameManager)
    {
        Debug.Log("Constructing Homelands Game");
        _gameManager = gameManager;
        _tileShape = gameManager._tileShape;
        _gameType = gameManager._gameType;
        InitializeGame();
    }
    public virtual void InitializeGame()
    {
        _locationDrawer = LocationDrawerFactory.Make(_gameManager._locationDrawerType);
        _locations = MapBuilder.Make(this);
        _mouseHandler = new MouseHandlerHomelands(_locations);
        _keyHandler = new KeyHandlerHomelands(this);
        _viewer = new HomelandsViewer();
    }

    public virtual List<GraphicsData> DrawAll()
    {
        List<GraphicsData> graphicsData = new List<GraphicsData>();
        foreach (ILocation location in _locations.Values)
        {
            graphicsData.Add(location.Draw());
        }
        return graphicsData;
    }
    public virtual List<GraphicsData> DrawUpdated()
    {
        return DrawAll();
    }
    
    public virtual TickInfo TakeTick(InputHandlerInfo inputHandlerInfo)
    {
        bool mouseHandle = _mouseHandler.HandleMouse(inputHandlerInfo._mouseHandlerInfo);
        bool keyHandle = _keyHandler.HandleKeys(inputHandlerInfo._keyHandlerInfo);
        List<GraphicsData> graphicsToDraw = DrawUpdated();
        TickInfo tickInfo = new TickInfo(graphicsToDraw);
        return tickInfo;
    }


}

public partial class HomelandsGame
{
    public Dictionary<Pos, HomelandsPosVisibility> GetPosVisibility()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<Pos, HomelandsPosControl> GetPosControl()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<Pos, HomelandsPosExtraction> GetPosExtraction()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<Pos, HomelandsPosMilitary> GetPosMilitary()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<eRadius, Dictionary<Pos, HomelandsPosStats>> GetPosStatsByType()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<Pos, Dictionary<eRadius, HomelandsPosStats>> GetPosStatsByPos()
    {
        throw new KeyNotFoundException();
    }
}


