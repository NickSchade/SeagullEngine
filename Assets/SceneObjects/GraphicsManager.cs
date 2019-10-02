using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphicsManager : MonoBehaviour
{
    public GameObject _dynamic;

    public GameObject _prefabLocation;
    public GameObject _prefabStructure;
    public GameObject _prefabResource;

    Dictionary<Pos, GameObject> _locations;
    Dictionary<Pos, GameObject> _structures;
    Dictionary<Pos, GameObject> _resources;

    public GameManager _gameManager;

    float xSpread = 1.0f;
    float ySpread = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        _locations = new Dictionary<Pos, GameObject>();
        _structures = new Dictionary<Pos, GameObject>();
        _resources = new Dictionary<Pos, GameObject>();

        ySpread = _gameManager._tileShape == eTileShape.Square ? 1.0f : 1.3f;
    }

    GameObject InstantiateGo(GameObject pf, Pos p, Color c)
    {
        Loc l = p.mapLoc;
        Vector3 pos = new Vector3(l.x() * xSpread, l.z(), l.y() * ySpread);
        GameObject go = Instantiate(pf, pos, Quaternion.identity, _dynamic.transform);
        go.GetComponentInChildren<Clickable>().setPos(p);
        SetColor(go, c);
        return go;
    }


    void SetColor(GameObject go, Color c)
    {
        go.GetComponentInChildren<Renderer>().material.color = c;
    }

    void DrawLocations(GraphicsData gd)
    {
        Pos pos = gd._pos;
        if (!_locations.ContainsKey(pos))
        {
            GameObject newGo = InstantiateGo(_prefabLocation, pos, gd._location.GetColor());
            _locations[pos] = newGo;
        }
        SetColor(_locations[pos], gd._location.GetColor());
    }
    void DrawStructures(GraphicsData gd)
    {
        Pos pos = gd._pos;
        if (gd._structure != null)
        {
            if (!_structures.ContainsKey(pos))
            {
                GameObject newGo = InstantiateGo(_prefabStructure, pos, gd._structure.GetColor());
                _structures[pos] = newGo;
            }
            SetColor(_structures[pos], gd._structure.GetColor());
        }
        else
        {
            if (_structures.ContainsKey(pos))
            {
                Destroy(_structures[pos]);
                _structures.Remove(pos);
            }
        }
    }
    void DrawResources(GraphicsData gd)
    {
        Pos pos = gd._pos;
        if (gd._resource != null)
        {
            if (!_resources.ContainsKey(pos))
            {
                GameObject newGo = InstantiateGo(_prefabResource, pos, gd._resource.GetColor());
                _resources[pos] = newGo;
            }
            SetColor(_resources[pos], gd._resource.GetColor());
        }
        else
        {
            if (_resources.ContainsKey(pos))
            {
                Destroy(_resources[pos]);
                _resources.Remove(pos);
            }
        }
    }

    public void Draw(List<GraphicsData> graphicsData)
    {
        foreach (GraphicsData gd in graphicsData)
        {
            Pos pos = gd._pos;
            DrawLocations(gd);
            DrawStructures(gd);
            DrawResources(gd);
        }
    }
}
