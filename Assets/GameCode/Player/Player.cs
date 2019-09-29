using UnityEngine;
using System.Collections;


public class Player
{
    public string _name;
    public PlayerResources _resources;
    public Player(string name)
    {
        _name = name;
        _resources = new PlayerResources(3f);
    }
}
