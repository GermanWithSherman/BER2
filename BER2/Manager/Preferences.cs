﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

[JsonObject(MemberSerialization.OptIn)]
public class Preferences
{
    public Preferences()
    {
    }

    //public delegate void OnUpdateMethod();
    //public OnUpdateMethod OnUpdate = delegate { };


    public CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-US");

    [JsonProperty("ActivatedModIDs")]
    private IList<string> _activatedModIDs = new List<string>();

    public IList<string> ActivatedModIDs
    {
        get => _activatedModIDs;
        set
        {
            _activatedModIDs = value;
        }
    }

    [JsonProperty("LastGame")]
    private string _lastGame;
    public string LastGame
    {
        get => _lastGame;
        set
        {
            _lastGame = value;
        }
    }

    [JsonProperty("ModsPath")]
    private string _modsPath;
    public string ModsPath
    {
        get
        {
            if (String.IsNullOrEmpty(_savePath))
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BER2", "mods");
            return _modsPath;
        }
        set => _modsPath = value;
    }

    [JsonProperty("SavePath")]
    private string _savePath;
    public string SavePath
    {
        get
        {
            if (String.IsNullOrEmpty(_savePath))
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BER2","saves");
            return _savePath;
        }
        set => _savePath = value;
    }


}
