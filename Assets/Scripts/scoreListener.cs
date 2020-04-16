using UnityEngine;
using System.Collections;
using System.Xml;

public class scoreListener : MonoBehaviour
{

    [System.NonSerialized]
    public float score;
    public GUISkin skin;
    public Texture2D cameraIMG;
    [System.NonSerialized]
    public float maxScore;
    private bool playing;
    private GameObject player;

    enum CurrentMenu
    {
        paused,
        gameover,
        playing
    }
    private CurrentMenu curMenu;

    void Start()
    {
        score = 0;
        maxScore = 0;
        playing = true;
        curMenu = CurrentMenu.playing;
        player = GameObject.FindGameObjectWithTag("Player");

        try
        {
            readXML();
        }
        catch
        {
            createXML();
        }
    }

    void Update()
    {
        if (playing)
        {
            score += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Escape) && playing)
        {
            Time.timeScale = 0;
            curMenu = CurrentMenu.paused;
            playing = false;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 25;
        GUI.skin.button.fontSize = 30;
        GUI.skin.box.fontSize = 30;
        GUI.skin = skin;

        if (curMenu == CurrentMenu.playing)
        {
            GUI.Label(new Rect(Screen.width - 100, 120, 100, 100), getScore(score).ToString());
            GUI.Label(new Rect(0, Screen.height - 120, 200, 100), "Рекорд: " + getScore(maxScore));
            if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height / 25, Screen.width / 8, Screen.width / 8), cameraIMG))
            {
                player.SendMessage("ChangeCam");
            }
        }
        else if (curMenu == CurrentMenu.gameover)
        {
            GUI.Box(new Rect(Screen.width / 6, Screen.height / 16, (Screen.width * 2) / 3, Screen.height / 2), "Сбила машина");
            GUI.Label(new Rect(Screen.width / 5, Screen.height / 7, Screen.width / 2, Screen.height / 2), "Результат: " + getScore(score).ToString());
            GUI.Label(new Rect(Screen.width / 5, Screen.height / 4, Screen.width / 2, Screen.height / 2), "Рекорд: " + getScore(maxScore).ToString());
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 5), "Заново"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            if (GUI.Button(new Rect(Screen.width / 2, (Screen.height / 6) * 5, Screen.width / 2.3f, Screen.height / 7), "Главное меню"))
            {
                Application.LoadLevel(0);
            }
        }
        else
        {
            GUI.Box(new Rect(Screen.width / 6, Screen.height / 16, (Screen.width * 2) / 3, Screen.height / 2), "Пауза");
            GUI.Label(new Rect(Screen.width / 5, Screen.height / 7, Screen.width / 2, Screen.height / 2), "Результат: " + getScore(score).ToString());
            GUI.Label(new Rect(Screen.width / 5, Screen.height / 4, Screen.width / 2, Screen.height / 2), "Рекорд: " + getScore(maxScore).ToString());
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 5), "Продолжить"))
            {
                curMenu = CurrentMenu.playing;
                Time.timeScale = 1;
                playing = true;
            }
            if (GUI.Button(new Rect(Screen.width / 2, (Screen.height / 6) * 5, Screen.width / 2.3f, Screen.height / 7), "Главное меню"))
            {
                Time.timeScale = 1;
                Application.LoadLevel(0);
            }
        }
    }

    void GameOver()
    {
        if (playing)
        {
            curMenu = CurrentMenu.gameover;
            playing = false;

            if (score > maxScore)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.persistentDataPath + "/274.xml");

                xmlDoc.SelectSingleNode("Information/MaxScore").InnerText = score.ToString();
                maxScore = score;

                xmlDoc.Save(Application.persistentDataPath + "/274.xml");
            }
        }
    }

    private void readXML()
    {
        XmlTextReader reader = new XmlTextReader(Application.persistentDataPath + "/274.xml");
        string NodeName = "";
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Text && NodeName == "MaxScore")
            {
                maxScore = float.Parse(reader.Value);
                break; //можно прервать цикл (нужно прочитать только одно значение)
            }
            else if (reader.NodeType == XmlNodeType.Element)
            {
                NodeName = reader.Name;
            }
        }
        reader.Close();
    }

    private void createXML()
    {
        XmlTextWriter xml = new XmlTextWriter(Application.persistentDataPath + "/274.xml", System.Text.Encoding.Default);

        xml.WriteStartDocument();
        xml.WriteStartElement("Information");
        xml.WriteStartElement("MaxScore");
        xml.WriteValue(0);
        xml.WriteEndElement();
        xml.WriteEndElement();
        xml.WriteEndDocument();

        xml.Close();
    }

    private int getScore(float mScore)
    {
        return (int)(mScore * 1000);
    }
}