using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class ParseJsonGenerateSpaces : MonoBehaviour
{

    [SerializeField]
    private GameObject polyPrefab;

    [SerializeField]
    private SpacesStats stats;

    List<GameObject> polyPrefabLi;

    List<SpaceObj> SpaceObjLi;

    void Awake()
    {
        SpaceObjLi = new List<SpaceObj>();
    }

    void Start()
    {
        polyPrefabLi = new List<GameObject>();
        string jsonStr = File.ReadAllText(".\\Assets\\data\\185171_biocontainment\\SpaceEquipmentData_185171_biocontainment_research_facility.json");
        // string jsonStr= File.ReadAllText(".\\Assets\\data\\sample_house\\SpaceEquipmentData_SampleHouse.json");
        ListOfSpaceRoom SpaceRooms = JsonUtility.FromJson<ListOfSpaceRoom>(jsonStr);


        List<SpaceRoom> SpaceRoomLi = SpaceRooms.data;

        //
        float MinX = float.MaxValue;
        float MinY = float.MaxValue;
        float MinZ = float.MaxValue;
        float MaxX = float.MinValue;
        float MaxY = float.MinValue;
        float MaxZ = float.MinValue;

        //
        for (int i = 0; i < SpaceRoomLi.Count; i++)
        {
            try
            {
                List<Vector3> polyCoords = GetCoordinates(SpaceRoomLi[i].coordinates);

                //
                SpaceRoom s = SpaceRoomLi[i];
                SpaceObj spaceobj = new SpaceObj(s.category, s.spaceName, s.spaceFullName, s.level, s.area, s.uniqueIdentifier, s.spaceId, s.Bounds);
                spaceobj.Setpoints(polyCoords);

                try
                {
                    spaceobj.ComputeCentroid();
                }
                catch (Exception e)
                {
                    print($"error computing centroids: \n{e.Message}");
                }

                //
                SpaceObjLi.Add(spaceobj);
                //
                int n = polyCoords.Count;
                //
                float minX = polyCoords.OrderBy(o => o.x).ToList()[0].x;
                float maxX = polyCoords.OrderBy(o => o.x).ToList()[n - 1].x;
                //
                float minY = polyCoords.OrderBy(o => o.y).ToList()[0].y;
                float maxY = polyCoords.OrderBy(o => o.y).ToList()[n - 1].y;
                //
                float minZ = polyCoords.OrderBy(o => o.z).ToList()[0].z;
                float maxZ = polyCoords.OrderBy(o => o.z).ToList()[n - 1].z;
                //
                if (minX < MinX)
                {
                    MinX = minX;
                }
                if (minY < MinY)
                {
                    MinY = minY;
                }
                if (minZ < MinZ)
                {
                    MinZ = minZ;
                }
                if (maxX > MaxX)
                {
                    MaxX = maxX;
                }
                if (maxY > MaxY)
                {
                    MaxY = maxY;
                }
                if (maxZ > MaxZ)
                {
                    MaxZ = maxZ;
                }

            }
            catch (Exception) { }
        }

        //
        Vector3 vec = new Vector3((MaxX - MinX) * 0.5f, (MaxY - MinY) * 0.5f, (MaxZ - MinZ) * 0.5f);

        //
        for (int i = 0; i < SpaceObjLi.Count; i++)
        {
            SpaceObjLi[i].UpdatePoints(vec);
        }

        //
        for (int i = 0; i < SpaceObjLi.Count; i++)
        {
            GeneratePoly(SpaceObjLi[i].points);
        }

        stats.numSpaces = SpaceObjLi.Count;
    }

    private List<Vector3> GetCoordinates(string coordStr)
    {
        List<Vector3> polyPts = new List<Vector3>();
        string[] arr = coordStr.Split(',');
        for (int i = 0; i < arr.Length - 2; i += 3)
        {
            float a = float.Parse(arr[i]);
            float b = float.Parse(arr[i + 1]);
            float c = float.Parse(arr[i + 2]);
            Vector3 v = new Vector3(a, c, b);
            v.Scale(new Vector3(10, 10, 10));
            polyPts.Add(v);
        }
        return polyPts;
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
    }

    public List<SpaceObj> GetSpaceObjLi()
    {
        return SpaceObjLi;
    }

    public List<GameObject> GetPolyPrefabLi()
    {
        return polyPrefabLi;
    }

    public int GetNumSpaces(){
        return SpaceObjLi.Count;
    }
}


[System.Serializable]
public class SpaceObj
{
    public string category;
    public string spaceName;
    public string spaceFullName;
    public string level;
    public string area;
    public string uniqueIdentifier;
    public string spaceId;
    public string Bounds;

    public List<Vector3> points;

    public Vector3 SpaceCentroid;

    public SpaceObj(string category, string spaceName, string spaceFullName, string level, string area, string uniqueIdentifier, string spaceId, string Bounds)
    {
        this.category = category;
        this.spaceName = spaceName;
        this.spaceFullName = spaceFullName;
        this.level = level;
        this.area = area;
        this.uniqueIdentifier = uniqueIdentifier;
        this.spaceId = spaceId;
        this.Bounds = Bounds;
        SpaceCentroid = Vector3.zero;
    }

    public void ComputeCentroid()
    {
        string[] arr = Bounds.Split(',');
        float a = float.Parse(arr[0]);
        float b = float.Parse(arr[1]);
        float c = float.Parse(arr[2]);
        float d = float.Parse(arr[3]);
        float e = float.Parse(arr[4]);
        float f = float.Parse(arr[5]);
        float x = (a + d) * 0.5f;
        float y = (b + e) * 0.5f;
        float z = (c + f) * 0.5f;
        SpaceCentroid = new Vector3(x, y, z);
    }

    public Vector3 GetCentroid()
    {
        return SpaceCentroid;
    }

    public void Setpoints(List<Vector3> pts)
    {
        points = pts;
    }

    public void UpdatePoints(Vector3 vec)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] += new Vector3(vec.x * 3, vec.y, vec.z);
        }
    }

    public List<Vector3> GetPoints()
    {
        return points;
    }
}


[System.Serializable]
public class SpaceRoom
{
    public string category;
    public string spaceName;
    public string spaceFullName;
    public string level;
    public string area;
    public string uniqueIdentifier;
    public string spaceId;
    public string Bounds;
    public string coordinates;
}


[System.Serializable]
public class ListOfSpaceRoom
{
    public List<SpaceRoom> data;
}