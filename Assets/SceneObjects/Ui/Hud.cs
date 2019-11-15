using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public TurnUi _turnUi;
    public PlayerDemoUi _playerDemoUi;
    public HelpUi _helpUi;
    public SelectedUi _selectedUi;
    public TurnSwitchScreenUi _turnSwitch;

    // Start is called before the first frame update
    void Start()
    {
        _playerDemoUi.gameObject.SetActive(true);
        _selectedUi.gameObject.SetActive(true);
        _turnUi.gameObject.SetActive(true);

        _turnSwitch.gameObject.SetActive(false);
        _helpUi.gameObject.SetActive(false);
    }

    public void ToggleHelp()
    {
        _helpUi.gameObject.SetActive(!_helpUi.gameObject.activeSelf);
    }
    public void ToggleScreen(PlayerSwitchData data)
    {
        _turnSwitch.gameObject.SetActive(!_turnSwitch.gameObject.activeSelf);
        
        if (_turnSwitch.gameObject.activeSelf)
            _turnSwitch.UpdateUi(data);

    }

    public void InitializeUi(HomelandsGame game, GameSettings settings)
    {
        _turnUi.Initialize(game, settings._tickSettings);
        _playerDemoUi.InitializeUi(game);
        _helpUi.InitializeUi(game);
        _selectedUi.InitializeUi(game);
    }

    public void UpdateUi(TickInfo tick)
    {
        _turnUi.UpdateUi(tick);
        _playerDemoUi.UpdateUi();
        _selectedUi.UpdateText(tick._turnData._mho._selected);
    }
    
}
