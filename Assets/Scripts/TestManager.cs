using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    void Start()
    {
        Test();
    }

    void Update()
    {

    }

    private void Test()
    {
        print($"Test manager : health is {TestSingleton.Instance.health}");
        List<int> ages = TestSingleton.Instance.ages;
        foreach (int a in ages)
        {
            print($"Test manager : data itr is {a}");
        }

        var people = TestSingleton.Instance.people;
        foreach (var person in people)
        {
            print($"User is {person.name} of age is {person.age}");
        }

        var spaceObjLi = SpaceObjSingleton.Instance.SpaceObjLi;
        for (int i = 0; i < spaceObjLi.Count; i++)
        {
            print($"{i}) {spaceObjLi[i].spaceFullName}");
         }
    }
}
