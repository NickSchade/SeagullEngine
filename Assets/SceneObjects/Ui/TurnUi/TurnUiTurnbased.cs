using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUiTurnbased : MonoBehaviour
{
    public Text _turnText;

    HomelandsGame _game;

    public void InitializeUi(HomelandsGame game)
    {
        gameObject.SetActive(true);
        _game = game;
    }
    public void UpdateUi(TickInfo tick)
    {
        _turnText.text = "End Turn #" + tick._turnNumber;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
