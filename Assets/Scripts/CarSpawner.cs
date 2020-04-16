using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject[] carPrefab;
    public GameObject[] bonusPrefab;
    public GameObject[] stuffPrefab;
    public GameObject[] motoPrefab;
    public float speed;
    public GameObject[] accidentPrefab;
    public GameObject TrafficLightPrefab;
    [System.NonSerialized]
    public GameObject accident;
    [System.NonSerialized]
    public GameObject trafficLight;

    public GameObject empty;
    public bool inTheForest;
    public Transform cursorCar;
    public Transform cursorLine;
    public Transform Rabbit;

    public UIInput SpeedInput;
    public UIInput DistInput;
    public UIInput TimeInput;
    public UIPopupList ListType;
    public UIPopupList ListLine;
    public UIPopupList ListBonus;
    public UISprite buttonPlay;
    public UICheckbox CheckBoxFree;

    private CarData selectedCar;
    private bool play;
    private bool free;
    private int line;
    private Type type;
    private int bonusId;
    private List<CarData>[] spawnedCars;
    private CarManager carManager;
    private DataLoader dl;
    private DataManager dm;
    private int StartCursorPos;
    private CarData[] freeCars;

    public enum SeqType
    {
        free,
        accident,
        trafficlight
    }

    private SeqType seqType = SeqType.free;

    void Awake()
    {
        freeCars = new CarData[4];
        for (int i = 0; i < 4; i++)
        {
            freeCars[i] = new CarData();
        }

        spawnedCars = new List<CarData>[4];
        for (int i = 0; i < 4; i++)
        {
            spawnedCars[i] = new List<CarData>();
        }

        play = false;
        free = true;
        StartCursorPos = 0;

        carManager = gameObject.GetComponent<CarManager>();
        carManager.data = spawnedCars;
        carManager.furtherCar = new CarData(cursorCar.gameObject, 0, 0, Type.car, 0);

        selectedCar = new CarData();

        type = Type.car;
        line = 0;

        cursorCar.transform.GetChild(0).animation.wrapMode = WrapMode.Loop;
        dl = new DataLoader(inTheForest);
        dm = new DataManager(spawnedCars, inTheForest);
    }

    void Update()
    {
        if (play)
        {
            for (int i = 0; i < 4; i++)
            {
                if (spawnedCars[i].Count > 0)
                {
                    if (spawnedCars[i][0].entity.name == "empty(Clone)")
                    {
                        if (freeCars[i].entity)
                        {
                            if (freeCars[i].entity.transform.position.z < -60)
                                SpawnFreeCar((Random.Range(20, 41 / (35 / freeCars[i].carspeed))), i, Random.Range(0, 60), ParseType(Random.Range(0, 4)));
                        }
                        else
                        {
                            SpawnFreeCar(20, i, Random.Range(0, 60), ParseType(Random.Range(0, 4)));
                        }
                    }
                }
            }

            Vector3 newPos = Rabbit.position;
            newPos.z += 20 * Time.deltaTime;
            Rabbit.position = newPos;
        }
    }

    private void SpawnFreeCar(float carspeed, int carline, float dist, Type carType)
    {
        Vector3 spawnpos = spawnPoints[carline].position;
        GameObject go;
        spawnpos.z += dist;
        int id;

        if (carType == Type.empty)
        {
            go = Instantiate(empty, spawnpos, Quaternion.identity) as GameObject;
        }
        else if (carType == Type.lift || carType == Type.barrier)
        {
            if (carType == Type.barrier)
                id = Random.Range(0, 2);
            else
                id = Random.Range(2, stuffPrefab.Length);

            go = Instantiate(stuffPrefab[id], spawnpos, stuffPrefab[id].transform.rotation) as GameObject;
        }
        else if (carType == Type.moto)
        {
            id = Random.Range(0, motoPrefab.Length);

            go = Instantiate(motoPrefab[id], spawnpos, motoPrefab[id].transform.rotation) as GameObject;
        }
        else
        {
            if (carType == Type.car)
                id = Random.Range(0, 6);
            else if (carType == Type.big)
                id = Random.Range(6, 11);
            else
                id = Random.Range(11, carPrefab.Length);

            go = Instantiate(carPrefab[id], spawnpos, carPrefab[id].transform.rotation) as GameObject;
        }

        carManager.AddCar(go, new Vector3(0, 0, -carspeed));
        freeCars[carline].entity = go;
        freeCars[carline].carspeed = speed;
    }

    private CarData SpawnCar(float carspeed, int line, float dist, Type carType, int bonusId)
    {
        Vector3 spawnpos = spawnPoints[line].position;
        spawnpos.z += dist;
        int id;

        if(carType == Type.empty)
        {
            spawnedCars[line].Add(new CarData(Instantiate(empty, spawnpos, Quaternion.identity) as GameObject, carspeed, dist, carType, bonusId));
        }
        else if (carType == Type.lift || carType == Type.barrier || carType == Type.Fbig)
        {
            if (carType == Type.barrier)
                id = Random.Range(0, 2);
            else if (carType == Type.lift)
                id = Random.Range(2, 4);
            else //if(carType == Type.Fbig)
                id = Random.Range(4, stuffPrefab.Length);

            spawnedCars[line].Add(new CarData(Instantiate(stuffPrefab[id], spawnpos, stuffPrefab[id].transform.rotation) as GameObject, carspeed, dist, carType, bonusId));
        }
        else if (carType == Type.moto)
        {
            id = Random.Range(0, motoPrefab.Length);
            spawnedCars[line].Add(new CarData(Instantiate(motoPrefab[id], spawnpos, motoPrefab[id].transform.rotation) as GameObject, carspeed, dist, carType, bonusId));
        }
        else
        {
            if (carType == Type.car)
                id = Random.Range(0, 6);
            else if (carType == Type.big)
                id = Random.Range(6, 11);
            else if (carType == Type.mega)
                id = Random.Range(11, 16);
            else
                id = Random.Range(16, carPrefab.Length);

            spawnedCars[line].Add(new CarData(Instantiate(carPrefab[id], spawnpos, carPrefab[id].transform.rotation) as GameObject, carspeed, dist, carType, bonusId));
        }

        if (bonusId != -1)
        {
            if (carType == Type.car || carType == Type.moto || carType == Type.barrier)
                spawnpos.y += 2;
            else if(carType != Type.empty)
                spawnpos.y += 4;
            GameObject bonus = Instantiate(bonusPrefab[bonusId], spawnpos, bonusPrefab[bonusId].transform.rotation) as GameObject;
            bonus.transform.parent = spawnedCars[line][spawnedCars[line].Count-1].entity.transform;
        }
        else
            if (carspeed == 0 && (carType == Type.Fbig || carType == Type.big || carType == Type.mega) && Random.Range(0, 5) == 0)
            {
                spawnpos.y += 4;
                GameObject bonus = Instantiate(bonusPrefab[1], spawnpos, bonusPrefab[1].transform.rotation) as GameObject;
                bonus.transform.parent = spawnedCars[line][spawnedCars[line].Count - 1].entity.transform;
            }

        return spawnedCars[line][spawnedCars[line].Count - 1];
    }

    private void SpawnAttributes()
    {
        if (inTheForest) return;

        if (seqType == SeqType.free)
        {
            if (accident)
                GameObject.Destroy(accident);
            if (trafficLight)
                GameObject.Destroy(trafficLight);
        }
        else if (seqType == SeqType.accident && !accident)
        {
            int index = Random.Range(0, accidentPrefab.Length);
            accident = Instantiate(accidentPrefab[index], spawnPoints[1].position, accidentPrefab[index].transform.rotation) as GameObject;

            if (trafficLight)
                GameObject.Destroy(trafficLight);
        }
        else if (!trafficLight)
        {
            trafficLight = Instantiate(TrafficLightPrefab, spawnPoints[1].position, TrafficLightPrefab.transform.rotation) as GameObject;

            if (accident)
                GameObject.Destroy(accident);
        }
    }

    Type ParseType(int index)
    {
        switch (index)
        {
            case 0:
                return Type.car;
            case 1:
                return Type.big;
            case 2:
                return Type.moto;
            case 3:
                return Type.mega;
            case 4:
                return Type.barrier;
            case 5:
                return Type.lift;
            default:
                {
                    Debug.Log("debil");
                    return Type.car;
                };
        }
    }

    void Add()
    {
        Read();
        SpawnCar(speed, line, cursorLine.position.z, type, bonusId);

        Vector3 newpos = cursorLine.position;
        switch (type)
        {
            case Type.car:
                newpos.z += 5;
                break;
            case Type.big:
                newpos.z += 9;
                break;
            case Type.Fbig:
                newpos.z += 9;
                break;
            case Type.mega:
                newpos.z += 13;
                break;
            case Type.megamega:
                newpos.z += 24;
                break;
            case Type.moto:
                newpos.z += 5;
                break;
            case Type.barrier:
                newpos.z += 7;
                break;
            case Type.lift:
                newpos.z += 8;
                break;
            default:
                return;
        }
        cursorLine.position = newpos;

        DistInput.text = cursorLine.position.z.ToString();
    }

    void OnSpeedSubmit(string val)
    {
        try
        {
            speed = float.Parse(val);
        }
        catch
        {
            speed = 0;
            SpeedInput.text = "";
        }
    }

    void OnDistSubmit(string val)
    {
        try
        {
            Vector3 pos = new Vector3(0, 0, float.Parse(val));
            cursorLine.position = pos;
        }
        catch
        {
            cursorLine.position = new Vector3(0, 0, StartCursorPos);
            DistInput.text = StartCursorPos.ToString();
        }
    }

    void OnLineSelectionChange(string val)
    {
        if (spawnedCars[int.Parse(val)].Count == 0)
        {
            cursorLine.position = new Vector3(0, 0, StartCursorPos);
            DistInput.text = StartCursorPos.ToString();
        }
    }

    public void SelectCar(GameObject car)
    {
        Read();
        int xline, xpos;
        selectedCar = Find(car, out xline, out xpos);

        if (selectedCar == null)
        {
            print("Random car was selected");
            return;
        }
        MoveCarCursor();

        SpeedInput.text = selectedCar.carspeed.ToString();
        ListType.textLabel.text = selectedCar.type.ToString();
        ListLine.textLabel.text = xline.ToString();
        ListBonus.textLabel.text = selectedCar.bonusId.ToString();
        OnDistSubmit(selectedCar.dist.ToString("#.#"));
        DistInput.text = selectedCar.dist.ToString();
    }

    public void Unselect()
    {
        selectedCar = null;
        cursorCar.parent = null;
        cursorCar.position = Vector3.zero;
    }

    public void ChangePos()
    {
        float dist = cursorLine.position.z;
        DistInput.text = dist.ToString("#.#");
    }

    void Read()
    {
        type = DataLoader.parseType(ListType.textLabel.text);
        line = int.Parse(ListLine.textLabel.text);
        bonusId = int.Parse(ListBonus.textLabel.text);

        try
        {
            cursorLine.position = new Vector3(0, 0, float.Parse(DistInput.text));
        }
        catch
        {
            cursorLine.position = new Vector3(0, 0, StartCursorPos);
            DistInput.text = StartCursorPos.ToString();
        }

        try
        {
            speed = float.Parse(SpeedInput.text);
        }
        catch
        {
            speed = 0;
            SpeedInput.text = "0";
        }
    }

    void Change()
    {
        if(selectedCar != null && selectedCar.entity)
        {
            Read();

            if (selectedCar.type != type || selectedCar.bonusId != bonusId)
            {
                int xline, xpos;
                selectedCar = Find(selectedCar.entity, out xline, out xpos);
                spawnedCars[line].RemoveAt(xpos);
                GameObject.Destroy(selectedCar.entity);
                selectedCar = SpawnCar(speed, line, cursorLine.position.z, type, bonusId);
            }

            selectedCar.carspeed = speed;

            selectedCar.dist = cursorLine.position.z;
            Vector3 newpos = selectedCar.entity.transform.position;
            newpos.z = selectedCar.dist;
            selectedCar.entity.transform.position = newpos;

            MoveCarCursor();
        }
    }

    public void Delete()
    {
        if (selectedCar != null && selectedCar.entity)
        {
            int xline; int xpos;
            selectedCar = Find(selectedCar.entity, out xline, out xpos);
            spawnedCars[xline].RemoveAt(xpos);
            GameObject.Destroy(selectedCar.entity);

            cursorCar.parent = null;
            cursorCar.position = Vector3.zero;
        }
    }

    void MoveCarCursor()
    {
        if(selectedCar != null && selectedCar.entity)
        {
            Vector3 cursorPos = selectedCar.entity.transform.position;
            cursorPos.y += 2;
            cursorCar.position = cursorPos;
            cursorCar.parent = selectedCar.entity.transform;
        }
        else
        {
            cursorCar.position = Vector3.zero;
        }
    }

    void Play()
    {
        if (play)
        {
            buttonPlay.spriteName = "play";
            carManager.play = false;
        }
        else
        {
            buttonPlay.spriteName = "pause";
            carManager.Play();
        }
        play = !play;
    }

    public void Stop()
    {
        play = false;
        buttonPlay.spriteName = "play";
        carManager.Reset();
        Rabbit.position = new Vector3(0, 0, -120);
    }

    void Save()
    {
        dm.createXML();
    }

    void Open()
    {
        Clear();

        dl.LoadData();
        //OnFreeChanged(dl.free);
        TimeInput.text = dl.time.ToString("#.#");

        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < dl.data[i].Count; k++)
            {
                SpawnCar(dl.data[i][k].carspeed, i, dl.data[i][k].dist, dl.data[i][k].type, dl.data[i][k].bonusId);
            }
        }
    }

    void Clear()
    {
        cursorCar.parent = null;
        cursorCar.position = Vector3.zero;

        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < spawnedCars[i].Count; k++)
            {
                GameObject.Destroy(spawnedCars[i][k].entity);
            }
            spawnedCars[i].Clear();
        }
    }

    CarData Find(GameObject car, out int xline, out int xpos)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < spawnedCars[i].Count; j++)
            {
                if (spawnedCars[i][j].entity == car)
                {
                    xpos = j;
                    xline = i;
                    return spawnedCars[i][j];
                }
            }
        }
        xpos = 0;
        xline = 0;
        return null;
    }

    void AttributesChanged()
    {
        if (!float.TryParse(TimeInput.text, out dm.time))
        {
            TimeInput.text = "0";
            dm.time = 0;
        }
    }

    void OnSeqTypeChanged(string val)
    {
        if (!inTheForest)
        {
            switch (val)
            {
                case "free":
                    {
                        seqType = SeqType.free;
                        if (cursorLine.position.z == StartCursorPos)
                            cursorLine.position = Vector3.zero;
                        StartCursorPos = 0;
                    }
                    break;
                case "accident":
                    {
                        seqType = SeqType.accident;
                        if (cursorLine.position.z == StartCursorPos)
                            cursorLine.position = new Vector3(0, 0, 18);
                        StartCursorPos = 18;
                    }
                    break;
                case "trafficlight":
                    {
                        seqType = SeqType.trafficlight;
                        if (cursorLine.position.z == StartCursorPos)
                            cursorLine.position = new Vector3(0, 0, 7);
                        StartCursorPos = 7;
                    }
                    break;
            }
            SpawnAttributes();
            ChangePos();
        }
    }
}