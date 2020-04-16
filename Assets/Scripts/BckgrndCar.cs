using UnityEngine;
using System.Collections;

public class BckgrndCar : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 startPos;

    void Update()
    {
        if (Distance(transform.position, startPos) > 90)
        {
            Destroy(gameObject);
            return;
        }
        gameObject.transform.position += velocity * Time.deltaTime;
    }

    int Distance(Vector3 a, Vector3 b)
    {
        return System.Convert.ToInt32(System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)));
    }
}