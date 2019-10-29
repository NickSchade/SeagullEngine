using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemoUi : MonoBehaviour
{
    public PlayerDemo _prefabPlayerDemo;

    Dictionary<Player, PlayerDemo> demos;
    HomelandsGame _game;

    public void InitializeUi(HomelandsGame game)
    {
        _game = game;

        demos = new Dictionary<Player, PlayerDemo>();
        foreach (Player p in _game._playerSystem.GetPlayers())
        {
            PlayerDemo pd = Instantiate(_prefabPlayerDemo, transform);
            pd.InitializeUi(p);
            pd.UpdateUi();
            demos[p] = pd;
        }
    }

    public void UpdateUi()
    {
        foreach (Player p in _game._playerSystem.GetPlayers())
        {
            demos[p].UpdateUi();
        }
    }
}
