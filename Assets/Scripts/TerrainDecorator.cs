using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDecorator : MonoBehaviour
{
    public float minRange = 15.0f;
    public float maxRange = 200.0f;

    public int[] objectCount = { 1000, 1000 };
    public GameObject[] objectPrefabs;
    public int[] objectPrefabIndex = { 0, 5 };
    public float[] objectScaling = { 0.2f, 0.5f };
    public Transform[] objectParents;
    public float yOffset = -3.0f;

    private GameObject terrain;

    void Start()
    {
        // Sets randomizer seed to get trees spawing in the same position every time
        //Random.InitState(12);
        terrain = transform.Find("Terrain").gameObject;

        // For every object type we want to place
        for (int i = 0; i < objectCount.Length; i++)
        {
            // For the number of object we want to place
            for (int j = 0; j < objectCount[i]; j++)
            {
                // Generate random position
                Vector2 pos = Vector2.zero;
                while (Vector2.Distance(pos, Vector2.zero) < minRange)
                    pos = Random.insideUnitCircle * maxRange;

                // Find the y position
                float yPos = terrain.GetComponent<Terrain>().SampleHeight(new Vector3(pos.x, 0, pos.y)) + yOffset;

                // Convert positions to a vector 3 spawn position
                Vector3 spawnPos = new Vector3(pos.x, yPos, pos.y);

                if (validSpawnPosition(spawnPos))
                {
                    // Spawns tree
                    GameObject newObject = Instantiate(objectPrefabs[Random.Range(objectPrefabIndex[i], i+1 < objectPrefabIndex.Length ? objectPrefabIndex[i+1] : objectPrefabs.Length)], spawnPos, Quaternion.identity);
                    newObject.transform.localScale = Vector3.one * objectScaling[i];
                    newObject.transform.parent = objectParents[i];
                }
            }
        }
    }

    private bool validSpawnPosition(Vector3 position)
    {
        // Casts ray down to see if the space is valid
        Ray ray = new(new Vector3(position.x, position.y + 100, position.z), Vector3.down);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, 150, ~(1 << 2));

        if (cast)
        {
            GameObject hitObject = hit.collider.gameObject;
            // if the cast his the terrain then the position is valid
            if (hitObject == terrain)
                return true;
        }

        return false;
    }
}
