using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    enum CamState
    {
        forward,
        down        
    }

    private float Speed;
    public Transform spawnPos;

    private  CarSpawner carSp;
    private Transform thisTransform;
    private Vector3 velocity;
    private bool lookDown;
    private Vector3 moveDir;

    void Start()
    {
        carSp = GameObject.Find("CarSpawner").GetComponent<CarSpawner>();
        thisTransform = transform;
        lookDown = false;
        Speed = 20;
    }

    void Update()
    {
        moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDir.z += Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDir.z -= Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.I) && thisTransform.position.y < 100)
        {
            moveDir.y += Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.O) && thisTransform.position.y > 3)
        {
            moveDir.y -= Speed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && thisTransform.position.x > -6)
        {
            moveDir.x -= Speed * Time.deltaTime;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && thisTransform.position.x < 6)
        {
            moveDir.x += Speed * Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            if (lookDown)
            {
                thisTransform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));
            }
            else
            {
                thisTransform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
            lookDown = !lookDown;
        }
        if(Input.GetKey(KeyCode.Delete))
        {
            carSp.Delete();
        }
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.Space)))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Area")
                {
                    Vector3 newpos = spawnPos.position;
                    newpos.z = hit.point.z;
                    spawnPos.position = newpos;

                    carSp.ChangePos();
                }
                else if(hit.transform.tag == "Car")
                {
                    carSp.SelectCar(hit.transform.gameObject);
                }
                else if (hit.transform.tag == "Respawn")
                {
                    Vector3 newpos = spawnPos.position;
                    newpos.z = 0;
                    spawnPos.position = newpos;

                    carSp.ChangePos();
                }
            }
            else
            {
                carSp.Unselect();
            }
        }
        thisTransform.position += moveDir;
    }
}