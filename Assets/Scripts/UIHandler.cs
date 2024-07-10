using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI DataTMP;

    [SerializeField]
    private SpacesStats stats;

    private void Awake()
    {
        // ParseJsonGenerateSpaces

    }


    void Start()
    {
        UpdateUI();
    }

    void Update()
    {

    }

    void UpdateUI()
    {
        string s2 = $"Number of spaces in file = {stats.numSpaces}";
        DataTMP.text = s2;
    }

}
