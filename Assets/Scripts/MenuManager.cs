using UnityEngine;

public class MenuManager : MonoBehaviour
{
    enum CurrentMenu
    {
        main,
        options,
    }

    public AudioClip[] ButtonClickSound;
    public Transform tr;
    public Transform rabbit;

    private int StatePoint;
    private Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        StatePoint = 1;
    }

    void WantToPlay()
    {
        if (StatePoint == 1)
            Application.LoadLevel(1);
        else
            Application.LoadLevel(2);
    }

    void Right()
    {
        StatePoint++;
        if (StatePoint == 3)
            StatePoint = 1;
        playSound();
    }

    void Left()
    {
        StatePoint--;
        if (StatePoint == 0)
            StatePoint = 2;
        playSound();
    }

    void Shop()
    {
        //if (UnityEditor.EditorUtility.DisplayDialog("Exit?", "Лох", "Акайкин"))
            Application.Quit();
    }

    void Update()
    {
        Vector3 nextPos;
        if (StatePoint == 2)
        {
            nextPos = tr.position;
            nextPos.z -= 2;
            nextPos.y += 2;
            nextPos.x += 2;
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, 1);
        }
        else
        {
            nextPos = rabbit.position;
            nextPos.z -= 2;
            nextPos.y += 2;
            nextPos.x += 2;
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, 1);
        }
    }

    void playSound()
    {
        if (Random.Range(0, 2) == 0 && !audio.isPlaying)
        {
            AudioClip clip = ButtonClickSound[Random.Range(0, ButtonClickSound.Length)];
            if (audio.clip != clip)
            {
                audio.clip = clip;
                audio.Play();
            }
        }
    }
}