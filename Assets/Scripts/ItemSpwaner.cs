using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpwaner : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        // gets item list
        List<GameObject> allItems = new List<GameObject>(items);
        List<Transform> allSpawns = new List<Transform>(transform.Find("SpawnPositions").GetComponentsInChildren<Transform>());

        while (allItems.Count != 0 && allSpawns.Count != 0)
        {
            // spawn item
            GameObject item = allItems[Random.Range(0, allItems.Count)];
            Transform spawn = allSpawns[Random.Range(0, allSpawns.Count)];

            GameObject newItem = Instantiate(item);
            newItem.transform.position = spawn.position;
            newItem.transform.parent = transform;

            allItems.Remove(item);
            allSpawns.Remove(spawn);
        }
    }
}
