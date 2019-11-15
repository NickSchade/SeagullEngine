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
        Debug.Log($@"{_player} gained {income} up to {_resource}");
    }
    public bool CanAfford(float amount)
    {
        bool canAfford = amount <= _resource;
        //Debug.Log($@"{_player._name} {(canAfford ? "can" : "can't")} afford paying {amount} of {_resource}");
        return canAfford;
    }
}
