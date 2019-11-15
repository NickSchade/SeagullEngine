using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyHandlerHomelands
{
    HomelandsGame _game;
    public KeyHandlerHomelands(HomelandsGame game)
    {
        _game = game;
    }
    public KeyHandlerOutput HandleKeys(KeyHandlerInfo keyHandlerInfo)
    {
        Dictionary<ePlayerAction, bool> keyActions = GetKeyActions(keyHandlerInfo);
        KeyHandlerOutput kho = new KeyHandlerOutput(keyActions);
        return kho;
    }
    Dictionary<ePlayerAction, bool> GetKeyActions(KeyHandlerInfo keyHandlerInfo)
    {
        Dictionary<ePlayerAction, bool> keyActions = new Dictionary<ePlayerAction, bool>();
        Dictionary<KeyCode, KeyCodeInfo> keys = keyHandlerInfo._keysInterfaced;
        foreach (KeyCode key in keys.Keys)
        {
            ePlayerAction action = InputSettings.GetAction(key);
            KeyCodeInfo kci = keys[key];
            keyActions[action] = kci._keyCodeInfo[eInput.Up];
        }
        return keyActions;
    }
    void HandleScroll(KeyHandlerInfo khi)
    {
        Dictionary<KeyCode, Vector3> scrollers = new Dictionary<KeyCode, Vector3>
        {
            {InputSettings.GetKeyCode(ePlayerAction.ScrollUp), new Vector3(0,0,1) },
            {InputSettings.GetKeyCode(ePlayerAction.ScrollDown), new Vector3(0,0,-1) },
            {InputSettings.GetKeyCode(ePlayerAction.ScrollLeft), new Vector3(-1,0,0) },
            {InputSettings.GetKeyCode(ePlayerAction.ScrollRight), new Vector3(1,0,0) }
        };

        Vector3 p = Camera.main.transform.position;
        Vector3 newP = new Vector3(p.x, p.y, p.z);
        foreach (KeyCode k in scrollers.Keys)
        {
            KeyCodeInfo kci = khi._keysInterfaced[k];
            if (kci._keyCodeInfo[eInput.Down] || kci._keyCodeInfo[eInput.Held])
            {
                Vector3 s = scrollers[k];
                newP = new Vector3(newP.x + s.x, newP.y + s.y, newP.z + s.z);
            }
        }
        Camera.main.transform.position = newP;
    }
}
