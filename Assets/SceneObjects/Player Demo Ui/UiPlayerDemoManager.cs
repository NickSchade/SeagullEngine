using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPlayerDemoManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject panel;
    public UiPlayerDemo pfUiPlayerDemo;

    Dictionary<Player, UiPlayerDemo> _ui;
    HomelandsGame _game;
    
    public void Initialize(HomelandsGame game)
    {
        _ui = new Dictionary<Player, UiPlayerDemo>();
        _game = game;
        gameManager = game._gameManager;
    }

    public void Draw()
    {
        List<Player> players = _game._playerSystem.GetPlayers();
        foreach (Player p in players)
        {
            if (!_ui.ContainsKey(p))
            {
                PlayerDemographics pd = p.GetDemographics();
                UiPlayerDemo upd = Instantiate(pfUiPlayerDemo, panel.transform);
                upd.SetPlayer(p);
                _ui[p] = upd;
            }
            _ui[p].Draw();
        }
    }
}
