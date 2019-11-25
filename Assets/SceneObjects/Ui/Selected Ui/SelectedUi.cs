using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUi : MonoBehaviour
{
    public Text _posText;
    public Text _terrainText;
    public Text _structureText;

    public Image _locationImage;
    public Image _structureImage;

    HomelandsGame _game;

    public void InitializeUi(HomelandsGame game)
    {
        _game = game;

        _structureImage.gameObject.SetActive(false);
    }
    
    void UpdateText(Pos pos, eTerrain terrain, HomelandsStructure structure)
    {
        string posString = 
        _posText.text = pos._gridLoc.key();

        _terrainText.text = terrain.ToString();
        _structureText.text = structure == null ? "" : structure.Describe();
    }
    public void UpdateText(SelectedData data)
    {
        if (data != null)
        {
            if (data._structure == null)
            {
                _structureImage.gameObject.SetActive(false);
            }
            else
            {
                _structureImage.gameObject.SetActive(true);
            }

            UpdateText(data._pos, data._terrain, data._structure);
        }
    }
}
