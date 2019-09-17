using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HomelandsGraphicsManager : MonoBehaviour
{
    public GameObject _dynamic;

    public GameObject _prefabLocation;
    public GameObject _prefabStructure;
    public GameObject _prefabResource;

    Dictionary<Pos, GameObject> _locations;
    Dictionary<Pos, GameObject> _structures;
    Dictionary<Pos, GameObject> _resources;

    public GameManager _gameManager;

    float tileSpread = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        _locations = new Dictionary<Pos, GameObject>();
        _structures = new Dictionary<Pos, GameObject>();
        _resources = new Dictionary<Pos, GameObject>();
    }

    GameObject InstantiateGo(GameObject pf, Pos p, Color c)
    {
        Loc l = p.mapLoc;
        Vector3 pos = new Vector3(l.x() * tileSpread, l.z(), l.y() * tileSpread);
        GameObject go = Instantiate(pf, pos, Quaternion.identity, _dynamic.transform);
        go.GetComponentInChildren<Clickable>().setPos(p);
        SetColor(go, c);
        return go;
    }


    void SetColor(GameObject go, Color c)
    {
        go.GetComponentInChildren<Renderer>().material.color = c;
    }

    public void Draw(List<GraphicsData> graphicsData)
    {
        foreach (GraphicsData gd in graphicsData)
        {
            Pos pos = gd._pos;
            // LOCATION
            if (!_locations.ContainsKey(pos))
            {
                GameObject newGo = InstantiateGo(_prefabLocation, pos, gd._location.GetColor());
                _locations[pos] = newGo;
            }
            SetColor(_locations[pos], gd._location.GetColor());
            // STRUCTURE
            if (gd._structure != null)
            {
                if (!_structures.ContainsKey(pos))
                {
                    GameObject newGo = InstantiateGo(_prefabStructure, pos, gd._structure.GetColor());
                    _structures[pos] = newGo;
                }
                SetColor(_structures[pos], gd._structure.GetColor());
            }
            // RESOURCE
            if (gd._resource != null)
            {
                if (!_resources.ContainsKey(pos))
                {
                    GameObject newGo = InstantiateGo(_prefabResource, pos, gd._resource.GetColor());
                    _resources[pos] = newGo;
                }
                SetColor(_resources[pos], gd._resource.GetColor());
            }

        }
    }
}
