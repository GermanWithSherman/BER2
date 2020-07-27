using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ItemsLibrary : Library<Item>
{
    
    public ItemsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }


    protected override Item getInvalidKeyEntry(string key)
    {
        throw new KeyNotFoundException($"The item with ID {key} does not exist");
    }

    public IList<Item> items()
    {
        return _dict.Values.ToList();

    }

    protected override ModableObjectHashDictionary<Item> loadFromFolder(string path)
    {
        var result = new ModableObjectHashDictionary<Item>();

        ModableObjectHashDictionary<ItemsFile> dict = loadFromFolder<ItemsFile>(path);



        foreach (KeyValuePair<string, ItemsFile> kv in dict)
        {
            foreach (KeyValuePair<string, Item> kv2 in kv.Value.Items)
            {
                var item = kv2.Value;
                item.id = kv2.Key;
                result.Add(kv2.Key, item);
            }
        }

        return result;
    }

}
