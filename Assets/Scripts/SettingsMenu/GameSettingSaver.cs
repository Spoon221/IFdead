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
        LoadXml();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void LoadXml()
    {
        isBusy = true;
        if (!File.Exists(Application.persistentDataPath + "\\settings" + ".cfg"))
        {
            settings = new GameSaverXML();
            isBusy = false;

            return;
        }


        XmlSerializer serializer = new XmlSerializer(typeof(GameSaverXML));

        FileStream fs = new FileStream(Application.persistentDataPath + "\\settings" + ".cfg", FileMode.Open,
            FileAccess.Read);
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
        //if (File.Exists(datapath)) File.Delete(datapath);

        FileStream fs;

        if (File.Exists(datapath))
        {
            fs = new FileStream(datapath, FileMode.Truncate,
                FileAccess.Write);
        }
        else
        {
            fs = File.Create(datapath);
        }


        var serializer = new XmlSerializer(typeof(GameSaverXML));
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

    [XmlElement("Fullscreen")]
    private bool _fullscreen = true;

    //[XmlElement("ResolutionX")]
    //private int _resolutionX;

    //[XmlElement("ResolutionY")]
    //private int _resolutionY;

    [XmlElement("Resolution")]
    private Resolution _resolution = Screen.currentResolution;

    public Resolution Resolution
    {
        get => _resolution;
        set
        {
            if (Equals(_resolution,value)) return;
            _resolution = value;
            GameSettingSaver.SaveXml();
        }
    }
    public bool Fullscreen
    {
        get => _fullscreen;
        set
        {
            if(value == _fullscreen) return;
            _fullscreen = value;
            GameSettingSaver.SaveXml();
        }
    }


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