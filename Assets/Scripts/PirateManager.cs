using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PirateManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> doors;

    [SerializeField]
    private List<PiratePrefabEntry> piratePrefabs;

    [SerializeField]
    private float spawnInterval;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0f)
        {
            spawnARandomPirate();
            spawnTimer = spawnInterval;
        }
    }

    private void spawnARandomPirate()
    {
        GameObject origin = randomDoor();
        GameObject destination = randomDoor();
        PirateType type = GetRandomEnum<PirateType>();
        GameObject newGO = GameObject.Instantiate(getPiratePrefab(type), origin.transform.position, origin.transform.rotation, transform);
        Pirate newPirate = newGO.GetComponent<Pirate>();
        newPirate.Destination = destination;
    }

    private GameObject randomDoor()
    {
        int rand = UnityEngine.Random.Range(0, doors.Count);
        return doors[rand];
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

    private GameObject getPiratePrefab(PirateType type)
    {
        foreach (PiratePrefabEntry entry in piratePrefabs)
        {
            if (entry.type == type)
                return entry.prefab;
        }
        Debug.Log("No Prefab for Pirate Type: " + type);
        return null;
    }
}

public enum PirateType { Red, Green, Blue, Yellow }

[Serializable]
public struct PiratePrefabEntry
{ 
    public PirateType type;
    public GameObject prefab;
}