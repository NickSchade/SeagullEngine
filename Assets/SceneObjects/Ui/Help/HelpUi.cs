using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUi : MonoBehaviour
{
    public HelpLine _helpLine;
    
    void InstantiateHelpLine(ePlayerAction action, KeyCode keyboardKey)
    {
        HelpLine helpLine = Instantiate(_helpLine, transform);
        helpLine._actionText.text = action.ToString();
        helpLine._keyBoardText.text = keyboardKey.ToString();
        helpLine._controllerText.text = "";
    }

    internal void InitializeUi(HomelandsGame game)
    {
        List<ePlayerAction> actions = InputConfigs.GetPlayerActions();
        foreach (ePlayerAction action in actions)
        {
            KeyCode key = InputSettings.GetKeyCode(action);
            InstantiateHelpLine(action, key);
        }
    }
}
