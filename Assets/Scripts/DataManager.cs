using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public enum Type
{
    car,
    big,
    mega,
    lift,
    barrier,
    moto,
    Fbig,
    megamega,
    empty
}

public class CarData
{
    public Type type = Type.car;
    public float carspeed;
    public float dist;
    public int bonusId = -1;

    //для едитора
    public GameObject entity;

    public CarData(GameObject car, float speed, float _dist, Type _type, int _bonusId)
    {
        entity = car;
        carspeed = speed;
        dist = _dist;
        bonusId = _bonusId;
        type = _type;
    }
    public CarData()
    {
        dist = 0;
        carspeed = 0;
    }
}

public class LineData
{
    public int count;
    public CarData[] carData;

    public LineData(int _count)
    {
        count = _count;
        carData = new CarData[count];
    }
}

public class SpawnData
{
    public LineData[] lineData;
    public float time = 0;

    public SpawnData()
    {
        lineData = new LineData[4];
    }
}

public class SpawnSequence
{
    public int count;
    public SpawnData[] spawnData;
    public bool free;

    public SpawnSequence(int _count)
    {
        count = _count;
        spawnData = new SpawnData[count];
    }
}

public class DataManager
{
    private List<CarData>[] Data;
    private bool inTheForest;
    public bool free;
    public float time;

    public DataManager(List<CarData>[] _Data, bool _inTheForest)
    {
        Data = _Data;
        inTheForest = _inTheForest;
    }

    public void createXML()
    {
        string path;
        if (inTheForest) path = Application.dataPath + "/SPDataFragments/FData.xml";
        else path = Application.dataPath + "/SPDataFragments/Data.xml";

        XmlTextWriter xml = new XmlTextWriter(path, System.Text.Encoding.UTF8);
        xml.Formatting = Formatting.Indented;
        
        xml.WriteStartDocument();

        if (inTheForest)
        {
            xml.WriteStartElement("AnimalRun");
            xml.WriteStartElement("LesoPodstavi"); xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("ForestSequence"); xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("SpawnData");
            xml.WriteAttributeString("time", time.ToString());

            for (int line = 0; line < 3; line++)
            {
                xml.WriteStartElement("EntityData");
                xml.WriteAttributeString("line", line.ToString());
                xml.WriteAttributeString("count", Data[line].Count.ToString());
                for (int i = 0; i < Data[line].Count; i++)
                {
                    xml.WriteStartElement("Entity");
                    xml.WriteAttributeString("speed", Data[line][i].carspeed.ToString());
                    xml.WriteAttributeString("dist", Data[line][i].dist.ToString());
                    xml.WriteAttributeString("type", Data[line][i].type.ToString());
                    if (Data[line][i].bonusId != -1)
                        xml.WriteAttributeString("bonusId", Data[line][i].bonusId.ToString());
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }
        }
        else
        {
            xml.WriteStartElement("AnimalRun");
            xml.WriteStartElement("Podstavi"); xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("SpawnSequence"); xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("SpawnData"); xml.WriteAttributeString("time", time.ToString());

            for (int line = 0; line < 4; line++)
            {
                xml.WriteStartElement("CarData");
                xml.WriteAttributeString("line", line.ToString());
                xml.WriteAttributeString("count", Data[line].Count.ToString());
                for (int i = 0; i < Data[line].Count; i++)
                {
                    xml.WriteStartElement("Car");
                    xml.WriteAttributeString("speed", Data[line][i].carspeed.ToString());
                    xml.WriteAttributeString("dist", Data[line][i].dist.ToString());
                    xml.WriteAttributeString("type", Data[line][i].type.ToString());
                    if (Data[line][i].bonusId != -1)
                        xml.WriteAttributeString("bonusId", Data[line][i].bonusId.ToString());
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }
        }
        xml.WriteFullEndElement();
        xml.WriteEndDocument();

        xml.Close();
    }
}