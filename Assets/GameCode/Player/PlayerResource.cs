using UnityEngine;
using System.Collections;

public class PlayerResources
{
    float _resource;
    public PlayerResources(float startingResource)
    {
        _resource = startingResource;
    }
    public bool CanPay(float payAmount)
    {
        return payAmount <= _resource;
    }
    public void Pay(float payAmount)
    {
        _resource -= payAmount;
    }
    public void Gain(float income)
    {
        _resource += income;
    }
}
