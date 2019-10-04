using UnityEngine;
using System.Collections;

public class PlayerResources
{
    Player _player;
    public float _resource;

    public PlayerResources(Player player, float startingResource)
    {
        _player = player;
        _resource = startingResource;
    }
    public void Pay(float payAmount)
    {
        _resource -= payAmount;
    }
    public void Gain(float income)
    {
        _resource += income;
        if (income > 0f)
        {
            Debug.Log($@"{_player} gained {income} up to {_resource}");
        }
    }
}
