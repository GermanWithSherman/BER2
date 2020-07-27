using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NPCTemplateCache : Cache<NPCTemplate>
{

    public NPCTemplateCache(string folder)
    {
        Folder = folder;
    }


protected override NPCTemplate create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "npctemplates", key + ".json");

        JObject deserializationData = GameManager.File2Data(path);

        NPCTemplate template = deserializationData.ToObject<NPCTemplate>();

        return template;
    }

    protected override NPCTemplate getInvalidKeyEntry(string key)
    {
        return null;
    }
}
