using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GRAPHICS
    public GraphicsManager _graphicsManager;
    // HACKY PLAYER DEMO UI
    public UiPlayerDemoManager _playerDemoManager;
    // INPUT
    public InputManager _inputManager;
    // GAME
    public eGame _gameType;
    public eMap _mapType;
    public eTileShape _tileShape;
    public eBuildQueue _buildQueueType;
    public int _numberOfPlayers;
    public HomelandsGame _game;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _game = GameFactory.Make(this);
        _inputManager.Initialize(this, _game._inputHandler);
        _playerDemoManager.Initialize(_game);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandlerInfo inputHandlerInfo = _inputManager.GetInput();
        HandleCamera(inputHandlerInfo);
        TickInfo tick = _game.TakeTick(inputHandlerInfo);
        _graphicsManager.Draw(tick._graphicsData);
        _playerDemoManager.Draw();
    }

    void HandleCamera(InputHandlerInfo ihi)
    {
        HandleZoom(ihi._mouseHandlerInfo);
        HandleScroll(ihi._keyHandlerInfo);
    }
    void HandleZoom(MouseHandlerInfo mhi)
    {
        float d = mhi._axis;
        if (d != 0)
        {
            if (d > 0f)
            {
                Camera.main.GetComponent<Camera>().orthographicSize -= 1;
            }
            else if (d < 0f)
            {
                Camera.main.GetComponent<Camera>().orthographicSize += 1;
            }
        }
    }
    void HandleScroll(KeyHandlerInfo khi)
    {
        Dictionary<KeyCode, Vector3> scrollers = new Dictionary<KeyCode, Vector3>
        {
            {KeyCode.W, new Vector3(0,0,1) },
            {KeyCode.S, new Vector3(0,0,-1) },
            {KeyCode.A, new Vector3(-1,0,0) },
            {KeyCode.D, new Vector3(1,0,0) }
        };

        Vector3 p = Camera.main.transform.position;
        Vector3 newP = new Vector3(p.x, p.y,p.z);
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
