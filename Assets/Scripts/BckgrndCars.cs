using UnityEngine;
using System.Collections;

public class BckgrndCars : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] carPrefab;
    public Vector3 velocity;
    public int density;
    private GameObject[] spawnedCar;
    private Quaternion rotation;

    void Start()
    {
        spawnedCar = new GameObject[spawnPoints.Length];
        rotation.eulerAngles = new Vector3(270, 0, 0);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnCar(velocity, i, Random.Range(0, 60), Random.Range(0, carPrefab.Length - 1));
        }
    }

    void Update()
    {
        for (int curn = 0; curn < spawnPoints.Length; curn++)
        {
            if (spawnedCar[curn])
            {
                if (Distance(spawnedCar[curn].transform.position, transform.position) > density)
                {
                    SpawnCar(velocity, curn, Random.Range(0, 35), Random.Range(0, carPrefab.Length));
                }
            }
            else
            {
                SpawnCar(velocity, curn, Random.Range(0, 35), Random.Range(0, carPrefab.Length));
            }
        }
    }

    private void SpawnCar(Vector3 velocity, int line, float dist, int id)
    {
        Vector3 spawnpos = spawnPoints[line].transform.position;
        spawnpos.x -= dist;

        spawnedCar[line] = Instantiate(carPrefab[id], spawnpos, rotation) as GameObject;
        spawnedCar[line].AddComponent<BckgrndCar>();
        spawnedCar[line].GetComponent<BckgrndCar>().velocity = velocity;
        spawnedCar[line].GetComponent<BckgrndCar>().startPos = transform.position;
        spawnedCar[line].transform.parent = transform;
    }

    int Distance(Vector3 a, Vector3 b)
    {
        return System.Convert.ToInt32(System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)));
    }
}