using UnityEngine;
using System.Collections.Generic;

public class AreaSpawner : MonoBehaviour
{
    public GameObject[] areaPrefab;
    public GameObject[] fAreaPrefab;
    public Transform spawnPos;

    private List<GameObject> areas;
    public bool inTheForest;

    void Awake()
    {
        areas = new List<GameObject>();
        Change();
    }

    void SpawnArea(Vector3 pos)
    {
        int index;

        if (inTheForest)
        {
            index = Random.Range(0, fAreaPrefab.Length);
            areas.Add(GameObject.Instantiate(fAreaPrefab[index], pos, fAreaPrefab[index].transform.rotation) as GameObject);
        }
        else
        {
             index = Random.Range(0, areaPrefab.Length);
             areas.Add(GameObject.Instantiate(areaPrefab[index], pos, areaPrefab[index].transform.rotation) as GameObject);
        }
    }

    void Change()
    {
        for(int i = 0; i < areas.Count; i++)
        {
            GameObject.Destroy(areas[i]);
        }
        areas.Clear();

        Vector3 pos = spawnPos.position;
        for (int i = 0; i < 6; i++)
        {
            SpawnArea(pos);
            pos.z -= 120;
        }
    }
}