using BER2;
using BER2.UI.OutfitWindow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

public class GameManager
{
    public const string PlayerVersion = "0.0.1";


    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    private GameManager()
    {
        PreferencesLoad();
    }

    public Preferences Preferences;

    public Misc Misc;

    public GameData GameData;// = new GameData();

    public PC PC
    {
        get => GameData.CharacterData.PC;
    }

    #region Caches
    public ConditionCache ConditionCache;
    public DialogueLineCache DialogueLineCache;
    public EventGroupCache EventGroupCache;
    public LocationCache LocationCache;
    public NPCTemplateCache NPCTemplateCache;
    public ServicesCache ServicesCache;
    public ServicepointCache ServicepointCache;
    public ShopTypeCache ShopTypeCache;
    public TextureCache TextureCache;
    public WeightedStringListCache WeightedStringListCache;

    

    public void CacheLocationsReset()
    {
        LocationCache.Reset();
    }

    public void CachesReset()
    {
        CacheLocationsReset();
        DialogueLineCache.Reset();
        EventGroupCache.Reset();
        NPCTemplateCache.Reset();
        ServicesCache.Reset();
        ServicepointCache.Reset();
        ShopTypeCache.Reset();
        TextureCache.Reset();
        WeightedStringListCache.Reset();
    }

    #endregion
    #region Libraries
    public ActivityLibrary ActivityLibrary;
    public DialogueTopicLibrary DialogueTopicLibrary;
    public FunctionsLibrary FunctionsLibrary;
    public ItemsLibrary ItemsLibrary;
    public LocationTypeLibrary LocationTypeLibrary;
    public NPCsLibrary NPCsLibrary;
    public ProceduresLibrary ProceduresLibrary;
    public TemplateLibrary TemplateLibrary;

    
    #endregion
    #region Servers
    //public DialogServer DialogServer;
    public InterruptServer InterruptServer;
    public ModsServer ModsServer;
    #endregion

    //public string DataPath { get => Preferences.DataPath; }
    public string DataPath { get; private set; }

    public string QuicksavePath
    {
        get
        {
            return GetSavegamePath("quicksave.json");
        }
    }

    private string GetSavegamePath(string name)
    {
        return Path.Combine(Preferences.SavePath,name);
    }

    public IEnumerable<Option> CurrentOptions
    {
        get
        {

            if (GameData.CurrentEventStage == null)
                return GameData.CurrentLocation.Options.Values;
            else
                return GameData.CurrentEventStage.Options.Values;
        }
    }

    public IEnumerable<LocationConnection> CurrentReachableLocations
    {
        get
        {
            if(GameData.CurrentEventStage == null)
                return GameData.CurrentLocation?.LocationConnections.VisibleLocationConnections;
            return new LocationConnection[0];
        }
    }

    public Template CurrentTemplate
    {
        get
        {
            if (GameData.CurrentLocation != null)
                return GameData.CurrentLocation.LocationType.Template;
            return TemplateLibrary[""];
        }
    }

    public CText CurrentText
    {
        get
        {
            /*if (GameData.CurrentEventStage == null)
                return GameData.CurrentLocation.Text.Text(GameData);
            else
                return GameData.CurrentEventStage.Text.Text(GameData);*/
            if (GameData.CurrentEventStage == null)
                return GameData.CurrentLocation.Text;
            else
                return GameData.CurrentEventStage.Text;
        }
    }

    public ModableTexture CurrentTexture{
        get
        {
            if (GameData.CurrentEventStage == null || GameData.CurrentEventStage.Texture == null)
                return GameData.CurrentLocation?.Texture;
            else
                return GameData.CurrentEventStage.Texture;


        }
    }

    public IEnumerable<IMainContent> MainContent
    {
        get
        {
            var result = new List<IMainContent>();

            result.Add(CurrentTexture);
            result.Add(CurrentText);

            return result;
        }
    }


    

    /*public GameObject SecondaryDisplayContent;
    private bool _secondaryDisplayActive;
    public bool SecondaryDisplayActive
    {
        get => _secondaryDisplayActive;
        set
        {
            _secondaryDisplayActive = value;
            if (_secondaryDisplayActive)
            {
                SecondaryDisplayContent.SetActive(true);
                Display.displays[1].Activate();
            }
            else
            {
                SecondaryDisplayContent.SetActive(false);
            }
        }
    }*/

    /*public CharacterScreen CharacterScreen;

    public EditWindow EditWindow;

    public UINPCsPresentContainer UINPCsPresentContainer;

    public OutfitWindow OutfitWindow;

    public UIDialogue UIDialogue;

    public UINotes UINotes;

    public UIPanelView UIServicesWindow;

    public UiShopWindow UiShopWindow;

    public Tooltip Tooltip;

    public StartMenu StartMenu;

    public GameObject LoadingScreen;


    private bool uiUpdateBlocked = false;
    private bool uiUpdatePending;
    public List<UIUpdateListener> updateListeners;*/

    

    public UISettings UISettings { get => GameData.UISettings; }

    public delegate void OnUIUpdate(GameData gameData,Preferences preferences);
    public OnUIUpdate OnUIUpdateEvent = delegate { Debug.Log("UI-Update executed"); };


    private const string _appdataPath = "BER2";
    private const string _preferencesFileName = "preferences.json";
    private readonly string _preferencesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appdataPath, _preferencesFileName);

    internal bool TryLoadGameFromPreferences()
    {
        string lastGamePath = Preferences.LastGame;

        if (File.Exists(lastGamePath))
        {
            return OpenGame(lastGamePath);
        }

        return false;
    }

    private void PreferencesLoad() => PreferencesLoad(_preferencesPath);

    private void PreferencesLoad(string path)
    {
        if (!File.Exists(path))
        {
            Preferences = new Preferences();
            PreferencesSave(path);
        }
        else
        {
            JObject jObject = File2Data(path);
            Preferences = jObject.ToObject<Preferences>();
        }

    }

    public void PreferencesSave() => PreferencesSave(_preferencesPath);

    private void PreferencesSave(string path)
    {
        Data2File(Preferences, path);
    }


    private void LoadStaticData()
    {
        IEnumerable<string> modsPaths = ModsServer.ActivatedModsPaths;

        ActivityLibrary = new ActivityLibrary();

        DialogueTopicLibrary = new DialogueTopicLibrary(PathRelative("dialogue/topics"), pathsMods(modsPaths, "dialogue/topics"));
        Thread dialogueTopicLibraryLoadThread = DialogueTopicLibrary.loadThreaded();

        FunctionsLibrary = new FunctionsLibrary(PathRelative("functions"), pathsMods(modsPaths, "functions"));
        Thread functionsLibraryLoadThread = FunctionsLibrary.loadThreaded();

        ItemsLibrary = new ItemsLibrary(PathRelative("items"), pathsMods(modsPaths, "items"));
        Thread itemsLibraryLoadThread = ItemsLibrary.loadThreaded();

        InterruptServer = new InterruptServer(PathRelative("interrupts"), pathsMods(modsPaths, "interrupts"));
        Thread interruptServerLoadThread = InterruptServer.loadThreaded();

        LocationTypeLibrary = new LocationTypeLibrary(PathRelative("locationtypes"), pathsMods(modsPaths, "locationtypes"));
        Thread locationTypeLibraryLoadThread = LocationTypeLibrary.loadThreaded();

        ProceduresLibrary = new ProceduresLibrary(PathRelative("procedures"), pathsMods(modsPaths, "procedures"));
        Thread proceduresLibraryLoadThread = ProceduresLibrary.loadThreaded();

        TemplateLibrary = new TemplateLibrary(PathRelative("templates"), pathsMods(modsPaths, "templates"));
        Thread templateLibraryLoadThread = TemplateLibrary.loadThreaded();

        NPCsLibrary = new NPCsLibrary();
        Thread rawNPCsLoadThread = NPCsLibrary.loadRawNpcsThreaded(PathRelative("npcs"), pathsMods(modsPaths, "npcs"));
        //NPCsLibrary.loadRawNpcsThreaded(path("npcs"), pathsMods(modsPaths, "npcs"));
        Misc = File2Object<Misc>(PathRelative("misc.json"));


        dialogueTopicLibraryLoadThread.Join();
        functionsLibraryLoadThread.Join();
        itemsLibraryLoadThread.Join();
        interruptServerLoadThread.Join();
        locationTypeLibraryLoadThread.Join();
        proceduresLibraryLoadThread.Join();
        rawNPCsLoadThread.Join();
        templateLibraryLoadThread.Join();
    }


    public void CharacterScreenShow()
    {
        //CharacterScreen.show(PC);
    }

    public void console(string command)
    {
        string[] commands = command.Split(new char[] { ':' }, 2);

        CommandsCollection commandsCollection = new CommandsCollection();

        string commandID = commands[0];
        string commandParam;

        if (commands.Length > 1)
            commandParam = commands[1];
        else
            commandParam = "";

        switch (commandID)
        {
            case "Event":
                CommandEvent commandEvent = new CommandEvent(commandParam);
                commandsCollection.Add(commandEvent);
                break;
        }

        commandsCollection.execute(GameData);
    }

    public void dialogueContinue(string topicID)
    {

        //UIDialogue.contin(topicID);
    }

    public void dialogueShow(NPC npc)
    {
        //UIDialogue.show(npc);
    }

    public void dialogueShow(NPC npc, DialogueTopic topic)
    {
        //UIDialogue.show(npc, topic);
    }

    public void eventEnd()
    {
        GameData.CurrentEventStage = null;
        UiUpdate();
    }

    public void eventExecute(string eventId)
    {
        EventStage eventStage = EventGroupCache.EventStage(eventId);
        EventExecute(eventStage);
    }

    public void EventExecute(string eventGroupId, string eventStageId)
    {
        EventStage eventStage = EventGroupCache.EventStage(eventGroupId, eventStageId);
        EventExecute(eventStage);
    }

    public void EventExecute(EventStage eventStage)
    {
        GameData.CurrentEventStage = eventStage;
        eventStage?.execute();

        if (GameData.CurrentEventStage != null)
            //UINPCsPresentContainer.setNPCs(new NPC[0]);

        UiUpdate();
    }

    public static void Data2File(object value, string path)
    {
        string json = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Directory.Create();

        File.WriteAllText(path, json);
    }

    public static JObject File2Data(string path, bool throwOnException = true)
    {
        if (!File.Exists(path))
            return new JObject();
        try
        {
            
            string jsonString = File.ReadAllText(path);
            var data = JObject.Parse(jsonString);
            return data;
        }
        catch(Exception e)
        {
            if (throwOnException)
                throw e;
            return new JObject();
        }
        
    }

    public static T File2Object<T>(string path, bool throwOnException = true)
    {
        return File2Data(path, throwOnException).ToObject<T>();
    }

    public void LoadSavegame()
    {
        LoadSavegame(QuicksavePath);
    }

    public void LoadSavegame(string path)
    {
        try
        {
            //uiUpdateBlocked = true;

            SaveFile saveFile = File2Object<SaveFile>(path);
            Debug.Log($"Game loaded from {path}");

            List<string> problems = new List<string>();

            if(saveFile.PlayerVersion != PlayerVersion)
            {
                problems.Add($"PlayerVersion: {saveFile.PlayerVersion} > {PlayerVersion}");
            }

            foreach (ModInfo saveModInfo in saveFile.Mods)
            {
                ModInfo installedModInfo = ModsServer.ActivatedModInfo(saveModInfo.ID);
                if (String.IsNullOrEmpty(installedModInfo.ID))
                {
                    problems.Add($"Mod {saveModInfo.ID}: {saveModInfo.Version} > not installed");
                }else if (saveModInfo.Version != installedModInfo.Version)
                {
                    problems.Add($"Mod {saveModInfo.ID}: {saveModInfo.Version} > {installedModInfo.Version}");
                }
            }

            if (problems.Count > 0)
            {

                /*var dialogSettings = new YesNoDialogSettings();

                dialogSettings.Title = "Continue Loading?";
                dialogSettings.Text = "Error while loading savegame. Load anyway?\n" + String.Join("\n",problems);

                dialogSettings.onYes = delegate { gameDataLoad(saveFile.GameData); };
                dialogSettings.onNo = delegate { StartMenu.show(); };

                DialogServer.dialogShow(DialogServer.YesNoDialog, dialogSettings);*/
            }
            else
            {
                gameDataLoad(saveFile.GameData);
            }

            


        }
        catch(Exception e)
        {
            ErrorMessage.Show("Error: "+e.Message);
            Debug.LogError(e);
        }
        finally
        {
            //uiUpdateBlocked = false;
        }
    }

    public void gameDataLoad(GameData gameData)
    {
        GameData = gameData;
        PC.id = "PC";

        npcsPresentUpdate();
        UiUpdate();
        

        //OutfitWindow.setCharacter(PC);

        
    }

    public void NewGame()
    {
        GameData = new GameData();

        PC pc = new PC();
        pc.id = "PC";
        GameData.CharacterData.PC = pc;

        GameData.NPCsActive["_PC"] = pc;

        EventExecute("start", "main");

        //OutfitWindow.setCharacter(PC);
    }

    public void SaveSavegame()
    {
        SaveSavegame(QuicksavePath);
    }

    public void SaveSavegame(string path)
    {
        SaveFile saveFile = new SaveFile();
        saveFile.GameData = GameData;
        saveFile.Mods = ModsServer.ActivatedModsInfo;
        saveFile.PlayerVersion = PlayerVersion;
        Data2File(saveFile, path);
        Debug.Log($"Game saved at {path}");

    }

    private void InitializeCaches()
    {
        ConditionCache = new ConditionCache();
        DialogueLineCache = new DialogueLineCache();
        EventGroupCache = new EventGroupCache("events");
        LocationCache = new LocationCache("locations");
        NPCTemplateCache = new NPCTemplateCache("npctemplates");
        ServicesCache = new ServicesCache("services");
        ServicepointCache = new ServicepointCache("servicepoints");
        ShopTypeCache = new ShopTypeCache("shoptypes");
        TextureCache = new TextureCache();
        WeightedStringListCache = new WeightedStringListCache();
}

    public void itemBuy(Item item, int price)
    {
        if(!PC.itemHas(item) && moneyPay(price))
            PC.itemAdd(item);
        UiUpdate();
    }



    internal bool OpenGame(string path)
    {
        try
        {

            Manifest manifest = File2Object<Manifest>(path);

            DataPath = Path.GetDirectoryName(path);

            InitializeCaches();

            ModsServer = new ModsServer(PathRelative("mods"), Preferences);

            LoadStaticData();

            Preferences.LastGame = path;

            NewGame();

            return true;
        }
        catch (FileNotFoundException)
        {
            ErrorMessage.Show($"Can't open game\nFile not found.\nPath:{path}");
        }
        catch (JsonException jsonException)
        {
            ErrorMessage.Show($"Can't open game\nError deserializing Manifest-file.\nError message:{jsonException.Message}");
        }
        catch (Exception e) {
            ErrorMessage.Show(e);
        }

        return false;
    }

    public void locationGoto(string subLocationId, bool skipOnShow = false)
    {
        SubLocation subLocation = LocationCache.SubLocation(subLocationId);
        locationGoto(subLocation, skipOnShow);
    }

    public void locationGoto(SubLocation subLocation, bool skipOnShow = false)
    {
        

        GameData.CurrentEventStage = null;
        GameData.CurrentLocation = subLocation;

        npcsPresentUpdate();

        if(!skipOnShow)
            subLocation.onShowExecute(this);

        UiUpdate();
    }

    public string npcActivity(string npcID)
    {
        return npcActivity(npcID,GameData.CurrentLocation.ID);
    }

    public string npcActivity(string npcID, string locationID)
    {
        return NPCsLibrary.npcActivity(npcID, locationID, GameData.WorldData.DateTime);
    }



    public bool npcIsPresent(string npcID)
    {
        return npcIsPresent(NPCsLibrary[npcID]);
    }

    public bool npcIsPresent(NPC npc)
    {
        foreach (NPC presentNPC in GameData.NpcsPresent)
        {
            if (presentNPC == npc)
                return true;
        }
        return false;
    }



    public bool npcIsPresent(string npcID, string locationID)
    {
        NPC npc = NPCsLibrary[npcID];
        SubLocation subLocation = LocationCache.SubLocation(locationID);
        return npcIsPresent(npc,subLocation);
    }

    public bool npcIsPresent(NPC npc, SubLocation subLocation)
    {
        IList<NPC> npcsPresent = NPCsLibrary.npcsPresent(subLocation, GameData.WorldData.DateTime);

        if (npcsPresent.Contains(npc))
            return true;
        return false;
    }

    private void npcsPresentUpdate()
    {
        IEnumerable<NPC> npcs = NPCsLibrary.npcsPresent(GameData.CurrentLocation, GameData.WorldData.DateTime);

        GameData.NpcsPresent = npcs;

        /*if (GameData.CurrentEventStage == null)
            UINPCsPresentContainer.setNPCs(GameData.NpcsPresent);
        else
            UINPCsPresentContainer.setNPCs(new NPC[0]);*/
    }

    public void locationTransfer(LocationConnection locationConnection)
    {

        CommandsCollection transferCommands = CommandGotoLocation.GotoCommandsList(locationConnection);
        transferCommands.execute();
    }

    public bool moneyPay(int amount)
    {
        GameData.CharacterData.PC.moneyCash -= amount;

        return true;
    }

    internal void noteAdd(string noteID, string text)
    {
        //GameData.Notes[noteID] = new Note(text, GameData.WorldData.DateTime);
    }

    internal void noteRemove(string noteID)
    {
        GameData.Notes.Remove(noteID);
    }

    public void notesHide()
    {
        //UINotes.hide();
    }

    public void notesShow()
    {
        //UINotes.showNotes(GameData.Notes.Values);
    }

    

     
    public void outfitWindowShow(OutfitRequirement outfitRequirement, CommandsCollection onClose)
    {
        //OutfitWindow.show(outfitRequirement, onClose);
        OutfitWindow outfitWindow = new OutfitWindow(PC);
        outfitWindow.Owner = Application.Current.MainWindow;
        outfitWindow.ShowDialog();
    }

    /// <summary>
    /// Returns a path in the current DATA-folder.
    /// </summary>
    public string PathRelative(string p)
    {
        return Path.Combine(DataPath,p);
    }

    public IEnumerable<string> pathsMods(string target)
    {
        return pathsMods(ModsServer.ActivatedModsPaths, target);
    }

    public IEnumerable<string> pathsMods(IEnumerable<string> modBasePaths, string target)
    {
        var result = new List<string>();

        foreach (string modBasePath in modBasePaths)
        {
            result.Add(Path.Combine(modBasePath, target));
        }

        return result;
    }

    public void ProcedureExecute(string procedureID, IEnumerable<dynamic> parameters,Data data)
    {
        ProceduresLibrary.procedureExecute(procedureID, data, parameters);
    }

    /*public void Quit()
    {
        Application.Quit();
    }*/


    /// <summary>
    /// Reloads Static Data, updates UI, clears caches.
    /// Use after modlist has changed.
    /// </summary>
    public void Refresh()
    {
        CachesReset();
        LoadStaticData();

        npcsPresentUpdate();
        UiUpdate();
    }

    public void shopShow(string shopId)
    {
        Shop shop = GameData.ShopData[shopId];
        shopShow(shop);
    }

    public void shopShow(Shop shop)
    {
        //UiShopWindow.show(shop);
    }

    public void servicepointShow(string id)
    {
        //UIServicesWindow.setCategories(ServicepointCache[id].ServiceCategories);
        //UIServicesWindow.show();
    }

    public void timeAdd(int duration)
    {
        GameData.WorldData.DateTime += new TimeSpan(0,0,duration);
        UiUpdate();
    }

    public int timeAgeYears(DateTime dateTime)
    {
        DateTime now = GameData.WorldData.DateTime;

        int age = now.Year - dateTime.Year;

        if (dateTime.Month > now.Month || (dateTime.Month == now.Month && dateTime.Day > now.Day))
            age--;
        return age;
    }

    public void timePass(int seconds, string activityId = "default")
    {
        timeAdd(seconds);

        Activity activity = ActivityLibrary[activityId];

        PC.timePass(seconds, activity);
    }

    public int timeSecondsTils(int targetTime,bool sameTimeAllowed=true)
    {
        DateTime now = GameData.WorldData.DateTime;

        int targetHour = targetTime / 10000;
        int targetMinute = (targetTime / 100) % 100;
        int targetSecond = targetTime % 100;

        int diff = (targetHour - now.Hour) * 3600 + (targetMinute - now.Minute) * 60 + (targetSecond - now.Second);

        if (diff < 0)
            diff += 86400;

        if(diff == 0 && !sameTimeAllowed)
            diff = 86400;

        return diff;
    }

    public DateTime timeWithAge(int age)
    {
        DateTime todayMN = GameData.WorldData.DateTime.Date;

        DateTime ageYearsAgo = new DateTime(todayMN.Year - age, todayMN.Month, todayMN.Day);
        DateTime ageP1YearsAgo = new DateTime(todayMN.Year - age - 1, todayMN.Month, todayMN.Day);
        ageP1YearsAgo += new TimeSpan(1, 0, 0, 0);

        int days = (ageYearsAgo - ageP1YearsAgo).Days;

        //ageP1YearsAgo += new TimeSpan(UnityEngine.Random.Range(0,days),0,0,0);

        return ageP1YearsAgo;

    }

    public void UiUpdate()
    {
        //uiUpdatePending = true;
        OnUIUpdateEvent(GameData,Preferences);
    }

    private void _uiUpdate()
    {
        try
        {
            /*DataCache.Reset();

            Debug.Log($"DayNight: {Misc.dayNightState(GameData.WorldData.DateTime)}");


            uiUpdatePending = false;

            foreach (UIUpdateListener listener in updateListeners)
            {
                listener.uiUpdate(this);
            }*/
        }
        catch(Exception e)
        {
            ErrorMessage.Show($"Error performing UI-Update:\n{e}");
        }
    }

    
}
