using UnityEngine;
using System.Collections.Generic;

public class CarManager : MonoBehaviour
{
    public bool play;
    [System.NonSerialized]
    public List<CarData>[] data;
    
    [System.NonSerialized]
    public CarData furtherCar;
    [System.NonSerialized]
    public List<Car> cars;

    public class Car
    {
        public Vector3 velocity;
        public GameObject entity;

        public Car(GameObject car, Vector3 vel)
        {
            entity = car;
            velocity = vel;
        }

        public Car(Car _car)
        {
            entity = _car.entity;
            velocity = _car.velocity;
        }

        public Car()
        {
            entity = new GameObject();
            velocity = Vector3.zero;
        }
    }

    void Start()
    {
        play = false;
        cars = new List<Car>();
    }

	void Update () {
        if (play)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < data[i].Count; k++)
                {
                    Vector3 newpos = data[i][k].entity.transform.position;
                    newpos.z -= data[i][k].carspeed * Time.deltaTime;
                    data[i][k].entity.transform.position = newpos;
                }
            }

            if (furtherCar.entity.transform.position.z < -86)
            {
                gameObject.GetComponent<CarSpawner>().Stop();
            }

            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].entity.transform.position += cars[i].velocity * Time.deltaTime;

                if (cars[i].entity.transform.position.z < -86)
                {
                    GameObject.Destroy(cars[i].entity);
                    cars.RemoveAt(i);
                }
            }
        }
	}

    public void AddCar(GameObject car, Vector3 velocity)
    {
        Car go = new Car(car, velocity);
        cars.Add(go);
    }

    public void Play()
    {
        Reset();
        if (!ChangeMaxDist())
            return;
        play = true;
    }

    public void Reset()
    {
        play = false;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < data[i].Count; k++)
            {
                Vector3 newpos = data[i][k].entity.transform.position;
                newpos.z = data[i][k].dist;
                data[i][k].entity.transform.position = newpos;
            }
        }

        for (int i = 0; i < cars.Count; i++)
        {
            GameObject.Destroy(cars[i].entity);
        }
        cars.Clear();
    }

    private bool ChangeMaxDist()
    {
        float maxTime = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < data[i].Count; k++)
            {
                if (((data[i][k].dist + 86) / data[i][k].carspeed) >= maxTime && data[i][k].carspeed != 0)
                {
                    maxTime = ((data[i][k].dist + 86) / data[i][k].carspeed);
                    furtherCar = data[i][k];
                }
            }
        }

        if (!furtherCar.entity)
        {
            print("no cars in scene");
            gameObject.GetComponent<CarSpawner>().Stop();
            return false;
        }
        else
            return true;
    }
}
