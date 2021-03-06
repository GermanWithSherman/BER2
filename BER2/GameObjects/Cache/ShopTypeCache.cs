﻿using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ShopTypeCache : Cache<ShopType>
{
    public ShopTypeCache(string folder)
    {
        Folder = folder;
    }

protected override ShopType create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "shoptypes", key + ".json");

        try
        {
            JObject deserializationData = GameManager.File2Data(path);

            ShopType shopType = deserializationData.ToObject<ShopType>();

            shopType.ID = key;

            return shopType;
        }catch(FileNotFoundException)
        {
            throw new GameException($"ShopType {key} does not exist");
        }
    }
}
