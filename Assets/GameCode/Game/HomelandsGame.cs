using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public abstract partial class HomelandsGame
{
    public GameManager _gameManager;

    public eGame _gameType { get; set; }
    public eMap _mapType { get; set; }
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
        _mapType = gameManager._mapType;

        _statsBuilder = new StatsBuilderBasic(this);
        _viewer = new Viewer(this);
        IMapBuilder mapBuilder = MapBuilderFactory.Make(_mapType, this);
        _locations = mapBuilder.Make();
        _mouseHandler = new MouseHandlerHomelands(_locations);
        _keyHandler = new KeyHandlerHomelands(this);
        
        _playerSystem = PlayerSystemFactory.Make(ePlayerSystem.TurnBased, this);
        _playerSystem.AddPlayer();
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
        //Extraction();
        //War();
    }

    void War()
    {
        foreach (HomelandsLocation location in _locations.Values)
        {
            HomelandsStructure structure = location._structure;
            if (structure != null)
            {
                Player owner = structure._owner;
                Dictionary<Player, float> attackValues = location._stats._military._attack;
                foreach (Player player in attackValues.Keys)
                {
                    if (player != owner)
                    {
                        float attackAmount = attackValues[player];
                        structure.TakeDamage(attackAmount);
                    }
                }
            }
        }
    }

    void Extraction()
    {
        Dictionary<Player, float> income = new Dictionary<Player, float>();
        List<Player> players = _playerSystem.GetPlayers();
        foreach (Player p in players)
        {
            income[p] = 0f;
        }
        foreach (HomelandsLocation location in _locations.Values)
        {
            if (location._resource != null)
            {
                Dictionary<Player, float> extractionRates = location._stats._extraction._extractionRate;
                foreach (Player p in players)
                {
                    income[p] += extractionRates[p];
                }
            }
        }
        foreach (Player p in players)
        {
            p._resources.Gain(income[p]);
        }
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



