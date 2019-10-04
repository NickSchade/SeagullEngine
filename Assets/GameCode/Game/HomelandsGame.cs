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
    public int _numberOfPlayers { get; set; }

    public IPlayerSystem _playerSystem { get; set; }

    public IInputHandler _inputHandler { get; set; }

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
        _numberOfPlayers = gameManager._numberOfPlayers;

        _tickSystem = TickSystemFactory.Make(eTickSystem.SemiRealTime, this);
        _statsBuilder = new StatsBuilderBasic(this);
        _viewer = new Viewer(this);
        IMapBuilder mapBuilder = MapBuilderFactory.Make(_mapType, this);
        MapSettings settings = new MapSettings(_tileShape, 19, 19, false, false);
        _locations = mapBuilder.Make(settings);
        _inputHandler = new InputHandler(this);

        _playerSystem = PlayerSystemFactory.Make(ePlayerSystem.TurnBased, this, _numberOfPlayers);
        //_playerSystem.AddPlayer();
    }

    public virtual TickInfo TakeTick(InputHandlerInfo inputHandlerInfo)
    {
        // RESOLVE TURN
        UpdateGame();

        // RESOLVE INPUT IN GAME
        HomelandsTurnData turnData = _inputHandler.HandleInput(inputHandlerInfo);

        // DRAW GAME
        List<GraphicsData> graphicsToDraw = Draw();

        // OUTPUT TURN INFO
        TickInfo tickInfo = _tickSystem.GetTick(graphicsToDraw);
        return tickInfo;
    }

    void UpdateGame()
    {
        _stats = new GameStats(this);
        UpdateLocationStats();
    }

    public void EndTurn()
    {
        Debug.Log("Ending Turn");
        PerformExtraction();
        PerformWar();
        PerformBuild();
    }

    void PerformBuild()
    {
        List<Player> players = _playerSystem.GetPlayers();
        foreach (Player player in players)
        {
            player._buildQueue.Build();
        }
    }

    void PerformExtraction()
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

    void PerformWar()
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



