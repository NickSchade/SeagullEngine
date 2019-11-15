using UnityEngine;
using System.Collections.Generic;


public class Player
{
    public HomelandsGame _game;
    public string _name;
    public Color _color;
    public PlayerResources _resources;
    public BuildQueueElegant _buildQueue;
    public Player(HomelandsGame game, PlayerDemographics demo)
    {
        _game = game;
        _name = demo.name;
        _color = demo.color;
        _resources = new PlayerResources(this, demo.resources);
        _buildQueue = new BuildQueueElegant(this, 3);
    }
    public PlayerDemographics GetDemographics()
    {
        PlayerDemographics demo = new PlayerDemographics(_name, _color, _resources._resource);
        return demo;
    }
}
