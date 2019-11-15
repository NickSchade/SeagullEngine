using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDemo : MonoBehaviour
{
    public Text playerName;
    public Text resource;

    Player _player;


    public void InitializeUi(Player p)
    {
        _player = p;
        playerName.text = _player._name;
        playerName.color = _player._color;
    }
    public void UpdateUi()
    {
        bool active = _player == _player._game._playerSystem._currentPlayer;
        gameObject.SetActive(active);
        resource.text = _player._resources._resource.ToString();
    }
}
