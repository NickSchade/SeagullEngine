using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public abstract partial class HomelandsGame
{
    public GameManager _gameManager;

    public eGame _gameType { get; set; }
    public eTileShape _tileShape { get; set; }

    public IKeyHandler _keyHandler { get; set; }
    public IMouseHandler _mouseHandler { get; set; }

    public Dictionary<Pos, HomelandsLocation> _locations;
    public Viewer _viewer;
    public IHomelandsPosStatsBuilder _statsBuilder;

    public ITickSystem _tickSystem;

    public HomelandsGame(GameManager gameManager)
    {
        Debug.Log("Constructing Homelands Game");

        _gameManager = gameManager;
        _tileShape = gameManager._tileShape;
        _gameType = gameManager._gameType;

        _statsBuilder = new HomelandsPosStatsBuilderBasic(this);
        _viewer = new Viewer(this);
        _locations = MapBuilder.Make(this);
        _mouseHandler = new MouseHandlerHomelands(_locations);
        _keyHandler = new KeyHandlerHomelands(this);
    }

    public virtual List<GraphicsData> Draw(List<HomelandsPosStats> stats)
    {
        List<GraphicsData> graphics = new List<GraphicsData>();
        foreach (HomelandsPosStats stat in stats)
        {
            Pos p = stat._pos;
            HomelandsLocation location = _locations[p];
            graphics.Add(location.Draw(stat));
        }
        return graphics;
    }

    public virtual TickInfo TakeTick(InputHandlerInfo inputHandlerInfo)
    {

        // RESOLVE INPUT IN GAME
        bool mouseHandle = _mouseHandler.HandleMouse(inputHandlerInfo._mouseHandlerInfo);
        bool keyHandle = _keyHandler.HandleKeys(inputHandlerInfo._keyHandlerInfo);

        // RESOLVE TURN
        List<HomelandsPosStats> stats = _statsBuilder.GetPosViews();

        // DRAW GAME
        List<GraphicsData> graphicsToDraw = Draw(stats);

        // OUTPUT TURN INFO
        TickInfo tickInfo = new TickInfo(graphicsToDraw);
        return tickInfo;
    }


}



