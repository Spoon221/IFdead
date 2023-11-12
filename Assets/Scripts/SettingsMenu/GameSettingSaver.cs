using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

public static class GameSettingSaver
{
    public static GameSaverXML settings;
    private static bool isBusy;

    static GameSettingSaver()
    {
        isBusy = true;
        if (!File.Exists(Application.persistentDataPath + "\\settings" + ".cfg"))
        {
            settings = new GameSaverXML();
            return;
        }


        XmlSerializer serializer = new XmlSerializer(typeof(GameSaverXML));

        FileStream fs = new FileStream(Application.persistentDataPath + "\\settings" + ".cfg", FileMode.Open, FileAccess.Read);
        GameSaverXML settingsXml = (GameSaverXML)serializer.Deserialize(fs);
        fs.Close();

        settings = settingsXml;
        isBusy = false;
    }

    public static async void SaveXml()
    {
        while (isBusy)
        {
            await Task.Delay(500);
        }
        isBusy = true;
        var datapath = Application.persistentDataPath + "\\settings" + ".cfg";
        if (File.Exists(datapath)) File.Delete(datapath);

        var serializer = new XmlSerializer(typeof(GameSaverXML));

        var fs = new FileStream(datapath, FileMode.CreateNew, FileAccess.Write);
        serializer.Serialize(fs, settings);
        fs.Close();
        isBusy = false;
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