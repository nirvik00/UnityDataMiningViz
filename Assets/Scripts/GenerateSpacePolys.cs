using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpacePolys : MonoBehaviour
{
    [SerializeField]
    private GameObject polyPrefab;

    [SerializeField]
    private SpacesStats stats;

    [SerializeField]
    private GameObject SpacePolysParent;

    List<GameObject> polyPrefabLi;

    List<SpaceObj> SpaceObjLi;

    void Awake()
    {
        polyPrefabLi = new List<GameObject>();
        SpaceObjLi = new List<SpaceObj>();
    }

    private void Start()
    {
        GetDataFromInstance();
    }

    private void GetDataFromInstance()
    {
        var spaceObjLi2 = SpaceObjSingleton.Instance.SpaceObjLi;
        for (int i = 0; i < spaceObjLi2.Count; i++)
        {
            var points = spaceObjLi2[i].points;
            GeneratePoly(points);
        }
    }

    private void GeneratePoly(List<Vector3> polyCoords)
    {
        var poly = Instantiate(polyPrefab);
        var polyRenderer = poly.GetComponent<LineRenderer>();
        polyRenderer.positionCount = polyCoords.Count;
        int count = 0;
        for (int i = 0; i < polyCoords.Count; i++)
        {
            polyRenderer.SetPosition(count, polyCoords[i]);
            count++;
        }
        polyRenderer.loop = true;
        polyPrefabLi.Add(poly);
        poly.transform.parent = SpacePolysParent.transform;
    }

    public List<GameObject> GetPolyPrefabLi()
    {
        return polyPrefabLi;
    }
}
