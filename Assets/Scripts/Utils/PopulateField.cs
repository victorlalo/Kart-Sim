using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateField : MonoBehaviour
{
    [SerializeField] List<GameObject> objectPrefabs;
    [SerializeField] int amountOfObjects;

    [SerializeField] Vector2 xSpawnRange = new Vector2(-10, 10);
    [SerializeField] Vector2 ySpawnRange = new Vector2(-10, 10);

    void Start()
    {
        for (int i = 0; i < amountOfObjects; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(xSpawnRange.x, xSpawnRange.y), 1,  Random.Range(ySpawnRange.x, ySpawnRange.y));
            GameObject g = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count - 1)], spawnPos, Quaternion.identity);
            g.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0f,1f), 1, Random.Range(0.5f, 1f));
            g.transform.localScale *= Random.Range(0.5f, 3.2f);
        }
    }
}
