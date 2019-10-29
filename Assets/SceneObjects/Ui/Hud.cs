using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public TurnUi _turnUi;
    public PlayerDemoUi _playerDemoUi;

    // Start is called before the first frame update
    void Start()
    {
        _turnUi.gameObject.SetActive(true);
        _playerDemoUi.gameObject.SetActive(true);
    }

    public void InitializeUi(HomelandsGame game, GameSettings settings)
    {
        _turnUi.Initialize(game, settings._tickSettings);
        _playerDemoUi.InitializeUi(game);
    }

    public void UpdateUi(TickInfo tick)
    {
        _turnUi.UpdateUi(tick);
        _playerDemoUi.UpdateUi();
    }
    
}
