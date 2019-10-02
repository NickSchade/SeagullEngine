using UnityEngine;
using System.Collections;


public class Player
{
    public string _name;
    public Color _color;
    public PlayerResources _resources;
    public Player(string name, Color color)
    {
        _name = name;
        _color = color;
        _resources = new PlayerResources(3f);
    }
}
