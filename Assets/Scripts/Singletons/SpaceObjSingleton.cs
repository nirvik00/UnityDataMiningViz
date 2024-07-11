using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceObjSingleton : MonoBehaviour
{
    [SerializeField]
    private string Filepath;
    public static SpaceObjSingleton Instance { get; private set; }

    public List<SpaceObj> SpaceObjLi;

    string FilePath = @"C:\Users\nsdev\UnityProjects\test3d\Assets\data\185171_biocontainment\SpaceEquipmentData_185171_biocontainment_research_facility.json";

    private void Awake(){
        ProcessJsonFile();
    }

    public void ProcessJsonFile()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SpaceObjLi = new List<SpaceObj>();


        // string jsonStr = File.ReadAllText(".\\Assets\\data\\185171_biocontainment\\SpaceEquipmentData_185171_biocontainment_research_facility.json");

        // string jsonStr= File.ReadAllText(".\\Assets\\data\\sample_house\\SpaceEquipmentData_SampleHouse.json");


        string jsonStr = File.ReadAllText(FilePath);

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
                // flip y and z in the get coordinates functions
                List<Vector3> polyCoords = GetCoordinates(SpaceRoomLi[i].coordinates);
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
        Vector3 vec = new Vector3((MaxX + MinX) * 0.5f, (MaxY + MinY) * 0.5f, (MaxZ + MinZ) * 0.5f);
        
        //
        for (int i = 0; i < SpaceRoomLi.Count; i++)
        {
            try
            {
                // flip y and z in the get coordinates functions
                List<Vector3> polyCoords = GetCoordinates(SpaceRoomLi[i].coordinates);

                //
                SpaceRoom s = SpaceRoomLi[i];
                SpaceObj spaceobj = new SpaceObj(s.category, s.spaceName, s.spaceFullName, s.level, s.area, s.uniqueIdentifier, s.spaceId, s.bounds);
                spaceobj.Setpoints(polyCoords, vec);

                //
                SpaceObjLi.Add(spaceobj);
            }
            catch (Exception) { }
        }
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
    public string bounds;

    public List<Vector3> points;

    public Vector3 SpaceCentroid;

    public SpaceObj(string category, string spaceName, string spaceFullName, string level, string area, string uniqueIdentifier, string spaceId, string bounds)
    {
        points = new List<Vector3>();
        this.category = category;
        this.spaceName = spaceName;
        this.spaceFullName = spaceFullName;
        this.level = level;
        this.area = area;
        this.uniqueIdentifier = uniqueIdentifier;
        this.spaceId = spaceId;
        this.bounds = bounds;
        SpaceCentroid = Vector3.zero;
    }

    public Vector3 GetCentroid()
    {
        return SpaceCentroid;
    }

    public void Setpoints(List<Vector3> pts, Vector3 vec)
    {
        points = pts;
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= new Vector3(vec.x, vec.y, vec.z);
        }
        //
        float MinX = float.MaxValue;
        float MinY = float.MaxValue;
        float MinZ = float.MaxValue;
        float MaxX = float.MinValue;
        float MaxY = float.MinValue;
        float MaxZ = float.MinValue;
        //
        int n = points.Count;
        //
        float minX = points.OrderBy(o => o.x).ToList()[0].x;
        float maxX = points.OrderBy(o => o.x).ToList()[n - 1].x;
        //
        float minY = points.OrderBy(o => o.y).ToList()[0].y;
        float maxY = points.OrderBy(o => o.y).ToList()[n - 1].y;
        //
        float minZ = points.OrderBy(o => o.z).ToList()[0].z;
        float maxZ = points.OrderBy(o => o.z).ToList()[n - 1].z;
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
        SpaceCentroid = new Vector3((MinX + MaxX) * 0.5f, (MinY + MaxY) * 0.5f, (MinZ + MaxZ) * 0.5f);
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
    public string bounds;
    public string coordinates;
}


[System.Serializable]
public class ListOfSpaceRoom
{
    public List<SpaceRoom> data;
}