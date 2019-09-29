using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public abstract partial class HomelandsGame
{
    public GameManager _gameManager;

    public eGame _gameType { get; set; }
    public eTileShape _tileShape { get; set; }

    public IPlayerSystem _playerSystem { get; set; }
    public IKeyHandler _keyHandler { get; set; }
    public IMouseHandler _mouseHandler { get; set; }

    public Dictionary<Pos, HomelandsLocation> _locations;
    public Viewer _viewer;
    public IStatsBuilder _statsBuilder;
    public GameStats _stats;


    public ITickSystem _tickSystem;

    public HomelandsGame(GameManager gameManager)
    {
        Debug.Log("Constructing Homelands Game");

        _gameManager = gameManager;
        _tileShape = gameManager._tileShape;
        _gameType = gameManager._gameType;

        _statsBuilder = new StatsBuilderBasic(this);
        _viewer = new Viewer(this);
        _locations = MapBuilder.Make(this);
        _mouseHandler = new MouseHandlerHomelands(_locations);
        _keyHandler = new KeyHandlerHomelands(this);

        List<Player> players = new List<Player> { new Player("a"), new Player("b") };
        _playerSystem = PlayerSystemFactory.Make(ePlayerSystem.TurnBased, this, players);
    }

    public virtual TickInfo TakeTick(InputHandlerInfo inputHandlerInfo)
    {
        // RESOLVE TURN
        UpdateGame();

        // RESOLVE INPUT IN GAME
        bool mouseHandle = _mouseHandler.HandleMouse(inputHandlerInfo._mouseHandlerInfo);
        bool keyHandle = _keyHandler.HandleKeys(inputHandlerInfo._keyHandlerInfo);

        // DRAW GAME
        List<GraphicsData> graphicsToDraw = Draw();

        // OUTPUT TURN INFO
        TickInfo tickInfo = new TickInfo(graphicsToDraw);
        return tickInfo;
    }

    void UpdateGame()
    {
        _stats = new GameStats(this);
        UpdateLocationStats();
    }

    void UpdateLocationStats()
    {
        List<Stats> stats = _statsBuilder.GetPosViews();
        foreach (Stats stat in stats)
        {
            Pos p = stat._pos;
            _locations[p]._stats = stat;
        }
    }

    public virtual List<GraphicsData> Draw()
    {
        List<GraphicsData> graphics = new List<GraphicsData>();
        foreach (Pos p in _locations.Keys)
        {
            HomelandsLocation location = _locations[p];
            graphics.Add(location.Draw());
        }
        return graphics;
    }


}



