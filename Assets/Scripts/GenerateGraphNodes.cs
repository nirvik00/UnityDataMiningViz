using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGraphNodes : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabNode;


    public List<GameObject> prefabNodeLi;

    ParseJsonGenerateSpaces generateAllRooms;

    void Awake()
    {
        prefabNodeLi = new List<GameObject>();
        generateAllRooms = GetComponent<ParseJsonGenerateSpaces>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
