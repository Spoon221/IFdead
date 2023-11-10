using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class GameSettingSaver
{
    public static GameSaverXML settings;

    static GameSettingSaver()
    {
        if (!File.Exists(Application.persistentDataPath + "\\settings" + ".cfg"))
        {
            settings = new GameSaverXML();
            return;
        }


        XmlSerializer serializer = new XmlSerializer(typeof(GameSaverXML));

        FileStream fs = new FileStream(Application.persistentDataPath + "\\settings" + ".cfg", FileMode.Open);
        GameSaverXML settingsXml = (GameSaverXML)serializer.Deserialize(fs);
        fs.Close();

        settings = settingsXml;
    }

    public static void SaveXml()
    {
        var datapath = Application.persistentDataPath + "\\settings" + ".cfg";
        if (File.Exists(datapath)) File.Delete(datapath);

        XmlSerializer serializer = new XmlSerializer(typeof(GameSaverXML));

        FileStream fs = new FileStream(datapath, FileMode.CreateNew);
        serializer.Serialize(fs, settings);
        fs.Close();
    }
}



[XmlRoot("SettingsRoot")]
[XmlType("Settings")]
public class GameSaverXML
{
    [XmlElement("Sensitivity")]
    private float _sensitivity = 0.5f;


    public float Sensitivity
    {
        get => _sensitivity;
        set
        {
            if (Math.Abs(value - _sensitivity) < 0.001f) return;
            _sensitivity = value;
            GameSettingSaver.SaveXml();
        }
    }
}