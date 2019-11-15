using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public Hud _pfHub;
    private Hud _hud;
    // Start is called before the first frame update
    void Start()
    {
        _hud = Instantiate(_pfHub, transform);
    }

    public void InitializeUi(HomelandsGame game, GameSettings settings)
    {
        _hud.InitializeUi(game, settings);
    }
    public void UpdateUi(TickInfo tick)
    {
        _hud.UpdateUi(tick);
    }
    public void ToggleHelp()
    {
        _hud.ToggleHelp();
    }
    public void ToggleScreen(PlayerSwitchData data = null)
    {
        _hud.ToggleScreen(data);
    }
    public bool ScreenIsActive()
    {
        return _hud._turnSwitch.gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
