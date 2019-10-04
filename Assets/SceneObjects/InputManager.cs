using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public GameManager _gameManager;

    private IKeyHandler _keyHandler;
    private IMouseHandler _mouseHandler;
    
    public void Initialize(GameManager gameManager, IInputHandler inputHandler)
    {
        _gameManager = gameManager;
        _keyHandler = inputHandler._keyHandler;
        _mouseHandler = inputHandler._mouseHandler;
    }

    public InputHandlerInfo GetInput()
    {
        KeyHandlerInfo khi = GetKeyInfo();
        MouseHandlerInfo mhi = GetMouseInfo();
        InputHandlerInfo ihi = new InputHandlerInfo(khi, mhi);
        return ihi;
    }
    List<KeyCode> GetInputKeys()
    {
        return Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToList();
    }
    public KeyHandlerInfo GetKeyInfo()
    {
        List<KeyCode> inputKeys = GetInputKeys();
        Dictionary<KeyCode, KeyCodeInfo> keyCodeInfos = new Dictionary<KeyCode, KeyCodeInfo>();
        foreach (KeyCode key in inputKeys)
        {
            KeyCodeInfo kci = GetKeyCodeInfos(key);
            keyCodeInfos[key] = kci;
        }
        KeyHandlerInfo khi = new KeyHandlerInfo(keyCodeInfos);
        return khi;
    }
    KeyCodeInfo GetKeyCodeInfos(KeyCode key)
    {
        Dictionary<eInput, bool> kcd = new Dictionary<eInput, bool>();
        kcd[eInput.Held] = Input.GetKey(key);
        kcd[eInput.Up] = Input.GetKeyUp(key);
        kcd[eInput.Down] = Input.GetKeyDown(key);
        KeyCodeInfo kci = new KeyCodeInfo(kcd);
        return kci;
    }
    Dictionary<eInput,bool> GetClickStatus(int iButton)
    {
        Dictionary<eInput, bool> clicks = new Dictionary<eInput, bool>();
        clicks[eInput.Up] = Input.GetMouseButtonUp(iButton);
        clicks[eInput.Down] = Input.GetMouseButtonDown(iButton);
        clicks[eInput.Held] = Input.GetMouseButton(iButton);
        return clicks;
    }
    public MouseHandlerInfo GetMouseInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        Dictionary<eInput, bool> leftClick = GetClickStatus(0);
        Dictionary<eInput, bool> rightClick = GetClickStatus(1);
        float axis = Input.GetAxis("Mouse ScrollWheel");

        if (leftClick.ContainsValue(true) || rightClick.ContainsValue(true))
        {
            foreach (RaycastHit hit in hits)
            {
                Clickable clicked = hit.transform.gameObject.GetComponentInParent<Clickable>();

                if (clicked != null)
                {
                    Pos p = clicked.pos;
                    MouseHandlerInfo mhi = new MouseHandlerInfo(p, axis, leftClick, rightClick);
                    return mhi;
                }
            }
        }
        return new MouseHandlerInfo(axis);
    }
}
