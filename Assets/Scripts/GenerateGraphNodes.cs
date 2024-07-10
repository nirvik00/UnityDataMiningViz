using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGraphNodes : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabNode;


    public List<GameObject> prefabNodeLi;


    void Awake()
    {
        prefabNodeLi = new List<GameObject>();
    }

    void Start()
    {
        GenerateGraphNodesFromSingleton();
    }

    void Update()
    {

    }

    private void GenerateGraphNodesFromSingleton()
    {   
        var spaceObjLi = SpaceObjSingleton.Instance.SpaceObjLi;
        foreach(var s in spaceObjLi)
        {
            Vector3 p = s.GetCentroid();
            GameObject node = Instantiate(prefabNode);
            node.transform.position = new Vector3(p.x, p.y, p.z);
            node.transform.localScale = new Vector3(10, 10, 10);
            prefabNodeLi.Add(node);
        }
    }
}






