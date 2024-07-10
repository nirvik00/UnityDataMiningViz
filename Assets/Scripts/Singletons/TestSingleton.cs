using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    System.Random rnd;

    public static TestSingleton Instance { get; private set; }

    public float health;

    public string[] names = {"a", "b", "c", "d", "e", "f", "g", "i", "j", "k"};

    public List<int> ages;

    public List<Person> people;

    private void Awake()
    {
        rnd = new System.Random();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        people = new List<Person>();

        ages = new List<int>();
        for(int i=0; i<10; i++){
            int x = rnd.Next(10, 100);
            ages.Add(x);
            Person p = new Person(){name= names[i], age = ages[i]};
            people.Add(p);
        }

        health = 100f;
    }

    void Start()
    {

    }


    void Update()
    {

    }
}

[SerializeField]
public class Person{
    public string name;
    public int age;
}


