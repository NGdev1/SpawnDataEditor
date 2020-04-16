using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DataLoader
{
    public SpawnSequence[] spSeq;
    public SpawnSequence[] forestSeq;
    public List<CarData>[] data;

    public float time;
    public bool free;
    public bool inTheForest;

    private int spSeqId;
    private int id;
    private int line;
    private int carId;

    public void LoadData()
    {
        for (int i = 0; i < 4; i++)
        {
            data[i].Clear();
        }

        XmlDocument xmlDoc = new XmlDocument();

        string path;
        if (inTheForest) path = Application.dataPath + "/SPDataFragments/FData.xml";
        else path = Application.dataPath + "/SPDataFragments/Data.xml";
        xmlDoc.Load(path);	

        LoadTree(xmlDoc.DocumentElement);

        ConvertToEditorData();
    }

    void init()
    {
        data = new List<CarData>[4];
        for (int i = 0; i < 4; i++)
        {
            data[i] = new List<CarData>();
        }

        id = 0;
        line = 0;
        carId = 0;
    }

    public DataLoader(bool _inTheForest)
    {
        inTheForest = _inTheForest;
        init();
    }

    void LoadTree(XmlNode node)
    {
        if (node != null)
            Load(node);

        if (node.HasChildNodes)
        {
            node = node.FirstChild;
            while (node != null)
            {
                LoadTree(node);
                node = node.NextSibling;
            }
        }
    }

    void Load(XmlNode node)
    {
        if (node.Attributes == null)
            return;

        XmlAttributeCollection map = node.Attributes;
        int i = 0;

        if (node.Name == "Podstavi")
        {
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        spSeq = new SpawnSequence[count];
                        spSeqId = -1;
                    }
                }
            }
        }

        if (node.Name == "LesoPodstavi")
        {
            inTheForest = true;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        forestSeq = new SpawnSequence[count];
                        spSeqId = -1;
                    }
                }
            }
        }

        if (node.Name == "SpawnSequence")
        {
            spSeqId++;
            id = -1;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        spSeq[spSeqId] = new SpawnSequence(count);

                        for (; i < count; i++)
                        {
                            spSeq[spSeqId].spawnData[i] = new SpawnData();
                        }
                    }
                    else if (attrnode.Name == "free")
                    {
                        spSeq[spSeqId].free = bool.Parse(attrnode.Value);
                    }
                }
            }
        }
        if (node.Name == "ForestSequence")
        {
            spSeqId++;
            id = -1;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        forestSeq[spSeqId] = new SpawnSequence(count);
                        for (; i < count; i++)
                        {
                            forestSeq[spSeqId].spawnData[i] = new SpawnData();
                        }
                    }
                    else if (attrnode.Name == "free")
                    {
                        forestSeq[spSeqId].free = bool.Parse(attrnode.Value);
                    }
                }
            }
        }
        else if (node.Name == "SpawnData")
        {
            id++;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "time")
                    {
                        if (inTheForest)
                            forestSeq[spSeqId].spawnData[id].time = float.Parse(attrnode.Value);
                        else
                            spSeq[spSeqId].spawnData[id].time = float.Parse(attrnode.Value);
                    }
                }
            }
        }
        else if (node.Name == "EntityData")
        {
            carId = 0;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "line")
                    {
                        line = int.Parse(attrnode.Value);
                    }
                    else if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        forestSeq[spSeqId].spawnData[id].lineData[line] = new LineData(count);

                        for (; i < count; i++)
                        {
                            forestSeq[spSeqId].spawnData[id].lineData[line].carData[i] = new CarData();
                        }
                    }
                }
            }
        }
        else if (node.Name == "CarData")
        {
            carId = 0;
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "line")
                    {
                        line = int.Parse(attrnode.Value);
                    }
                    else if (attrnode.Name == "count")
                    {
                        int count = int.Parse(attrnode.Value);
                        spSeq[spSeqId].spawnData[id].lineData[line] = new LineData(count);

                        for (; i < count; i++)
                        {
                            spSeq[spSeqId].spawnData[id].lineData[line].carData[i] = new CarData();
                        }
                    }
                }
            }
        }
        else if (node.Name == "Car")
        {
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "speed")
                    {
                        float speed = float.Parse(attrnode.Value);
                        spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].carspeed = speed;
                    }
                    else if (attrnode.Name == "dist")
                    {
                        float dist = float.Parse(attrnode.Value);
                        spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].dist = dist;
                    }
                    else if (attrnode.Name == "type")
                    {
                        Type type = parseType(attrnode.Value);
                        spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].type = type;
                    }
                    else if (attrnode.Name == "bonusId")
                    {
                        int bonusId = int.Parse(attrnode.Value);
                        spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].bonusId = bonusId;
                    }
                }
            }
            carId++;
        }
        else if (node.Name == "Entity")
        {
            if (XmlNodeType.Element == node.NodeType)
            {
                foreach (XmlNode attrnode in map)
                {
                    if (attrnode.Name == "speed")
                    {
                        float speed = float.Parse(attrnode.Value);
                        forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].carspeed = speed;
                    }
                    else if (attrnode.Name == "dist")
                    {
                        float dist = float.Parse(attrnode.Value);
                        forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].dist = dist;
                    }
                    else if (attrnode.Name == "type")
                    {
                        Type type = parseType(attrnode.Value);
                        forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].type = type;
                    }
                    else if (attrnode.Name == "bonusId")
                    {
                        int bonusId = int.Parse(attrnode.Value);
                        forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].bonusId = bonusId;
                    }
                }
            }
            carId++;
        }
    }

    public static Type parseType(string text)
    {
        switch (text)
        {
            case "mega": return Type.mega;
            case "big": return Type.big;
            case "car": return Type.car;
            case "lift": return Type.lift;
            case "barrier": return Type.barrier;
            case "empty": return Type.empty;
            case "moto": return Type.moto;
            case "Fbig": return Type.Fbig;
            case "megamega": return Type.megamega;
            default: return Type.car;
        }
    }

    void ConvertToEditorData()
    {
        if (inTheForest)
        {
            for (spSeqId = 0; spSeqId < forestSeq.Length; spSeqId++)
            {
                free = forestSeq[spSeqId].free;
                for (id = 0; id < forestSeq[spSeqId].count; id++)
                {
                    time = forestSeq[spSeqId].spawnData[id].time;
                    for (line = 0; line < 3; line++)
                    {
                        for (carId = 0; carId < forestSeq[spSeqId].spawnData[id].lineData[line].count; carId++)
                        {
                            data[line].Add(new CarData(null,
                                forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].carspeed,
                                forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].dist,
                                forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].type,
                                forestSeq[spSeqId].spawnData[id].lineData[line].carData[carId].bonusId));
                        }
                    }
                }
            }
        }
        else
        {
            for (spSeqId = 0; spSeqId < spSeq.Length; spSeqId++)
            {
                free = spSeq[spSeqId].free;
                for (id = 0; id < spSeq[spSeqId].count; id++)
                {
                    time = spSeq[spSeqId].spawnData[id].time;
                    for (line = 0; line < 4; line++)
                    {
                        for (carId = 0; carId < spSeq[spSeqId].spawnData[id].lineData[line].count; carId++)
                        {
                            data[line].Add(new CarData(null,
                                spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].carspeed,
                                spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].dist,
                                spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].type,
                                spSeq[spSeqId].spawnData[id].lineData[line].carData[carId].bonusId));
                        }
                    }
                }
            }
        }
    }
}