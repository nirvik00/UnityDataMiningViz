using System.Collections;
using System.Collections.Generic;
using System.IO;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine;

public class ReadJson : MonoBehaviour
{
    private ThingModel Model;

    private string jsonString;

    private Coordinate Coord;

    private LineRenderer polyRenderer;

    private void Awake()
    {
        polyRenderer = GetComponent<LineRenderer>();

        jsonString = File.ReadAllText(".\\Assets\\values.json");
        Model = JsonUtility.FromJson<ThingModel>(jsonString);
        // print(jsonString);
        // print($"Name: {Model.Name}, Number: {Model.Number}, Color: {Model.Color}, IsSquare: {Model.IsSquare}");

        string s2 = File.ReadAllText(".\\Assets\\data\\test.json");
        Coord = JsonUtility.FromJson<Coordinate>(s2);
        // print(s2);
        // print($"coords = {Coord.coordinates}");
        GeneratePoly(Coord.coordinates);
        // PrintOutTheData(Coord.coordinates);
    }


    private void GeneratePoly(string coordStr)
    {
        string[] arr = coordStr.Split(',');
        polyRenderer.positionCount = arr.Length/3;
        int count = 0;
        for (int i = 0; i < arr.Length - 2; i += 3)
        {
            float a = float.Parse(arr[i]);
            float b = float.Parse(arr[i + 1]);
            float c = float.Parse(arr[i + 2]);
            Vector3 v = new Vector3(a, c, b);
            v.Scale(new Vector3(10,10,10));
            polyRenderer.SetPosition(count, v);
            count++;
        }
        polyRenderer.loop = true;
        // print($"total number of points = {count}");
    }

    private void PrintOutTheData(string coordStr)
    {
        string[] arr = coordStr.Split(',');
        List<string> s2 = new List<string>();
        int count = 0;
        for (int i = 0; i < arr.Length - 2; i += 3)
        {
            float a = float.Parse(arr[i]);
            float b = float.Parse(arr[i + 1]);
            float c = float.Parse(arr[i + 2]);
            string s = $"{a}, {b}, {c}";
            s2.Add(s);
            count++;
        }
        File.WriteAllLines(".\\Assets\\data\\outtest.csv", s2);
    }
}


[System.Serializable]
public class ThingModel
{
    public string Name;
    public string Color;
    public int Number;
    public bool IsSquare;
}



[System.Serializable]
public class Coordinate
{
    public string coordinates;
}
