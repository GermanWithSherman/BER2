﻿using BER2.GameObjects.Effects;
using BER2.Util.Math;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class PC : NPC
{

    public ItemsCollection items = new ItemsCollection();

    public Dictionary<string,Outfit> outfits = new Dictionary<string, Outfit>();

    public Outfit CurrentOutfit = new Outfit();

    public string GenderBorn;

    public string GenderDress
    {
        get => CurrentOutfit.Gender;
    }

    public string GenderPerceived
    {
        get
        {
            string visible = BodyData.GenderVisible;
            string dress = GenderDress;

            if (visible == "f")
            {
                if (dress == "f" || dress == "fm" || dress == "None")
                    return "f";
                else
                    return "f2m";
            }
            else
            {
                if (dress == "m" || dress == "fm" || dress == "None")
                    return "m";
                else
                    return "m2f";
            }


        }
    }


    public Int64 moneyCash = 0;

    public int statCalories;
    public int statCaloriesBurnt;

    public int statHunger;
    private int _statHunger { get => statHunger; set => statHunger = Mathb.Clamp(value, 0, 1000000); }

    public int statHygiene;
    private int _statHygiene { get => statHygiene; set => statHygiene = Mathb.Clamp(value, 0, 1000000); }

    public int statSleep;
    private int _statSleep { get => statSleep; set => statSleep = Mathb.Clamp(value, 0, 1000000); }

    public int StatSatisfaction;
    private int _statSatisfaction { get => StatSatisfaction; set => StatSatisfaction = Mathb.Clamp(value, 0, 1000000); }

    /*[JsonIgnore]
    public int Distress
    {
        get => _distress;
    }
    [JsonProperty("Distress")]
    private int _distress;*/

    public int Attractiveness { get; private set; }
    public int Distress { get; private set; }
    

    public override bool ShouldSerializeTexturePath() => true;

    private static GameManager gameManager { get => GameManager.Instance; }



    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "moneyCash":
                return moneyCash;
            case "statHunger":
                return _statHunger;
            case "statHygiene":
                return _statHygiene;
            case "StatSatisfaction":
                return _statSatisfaction;
            case "statSleep":
                return statSleep;
            case "GenderBorn": return GenderBorn;
            case "GenderDress": return GenderDress;
            case "GenderPerceived": return GenderPerceived;
        }

        string[] keyparts = key.Split(new char[] { '.' }, 2);

        if (keyparts.Length == 2)
        { 
            switch (keyparts[0])
            {
                case "currentOutfit":
                    return CurrentOutfit.getDynamic(keyparts[1]);
            }
        }


        return base.get(key);
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "moneyCash":
                moneyCash = value;
                return;
            case "statHunger":
                _statHunger = (int)value;
                return;
            case "statHygiene":
                _statHygiene = (int)value;
                return;
            case "StatSatisfaction":
                _statSatisfaction = (int)value;
                return;
            case "statSleep":
                _statSleep = (int)value;
                return;
            case "GenderBorn":
                GenderBorn = (string)value;return;
        }

        setNPCData(key, value);
    }

    public void EffectDeactivate(Effect effect)
    {
        if (!Effects.Contains(effect))
            return;

        Effects.Remove(effect);
        Attractiveness -= effect.Attractiveness;
        Distress -= effect.Distress;
    }

    public void EffectActivate(Effect effect)
    {
        if (Effects.Contains(effect))
            return;

        Effects.Add(effect);
        Attractiveness += effect.Attractiveness;
        Distress += effect.Distress;
    }


    public ModableTexture GetClothingslotTexture(string slot)
    {
        string path = "";
        switch (slot)
        {
            case "Bra":
                Item bra = CurrentOutfit["Bra"];
                if (bra == null)
                {
                    path = gameManager.FunctionsLibrary.FunctionExecute("s_PcBodyBreast", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return bra.Texture;
            case "Clothes":
                Item clothes = CurrentOutfit["Clothes"];
                if (clothes == null)
                    return GetClothingslotTexture("Bra");
                return clothes.Texture;
            case "Panties":
                Item panties = CurrentOutfit["Panties"];
                if (panties == null) {
                    path = gameManager.FunctionsLibrary.FunctionExecute("s_PcBodyLap", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return panties.Texture;
            case "Shoes":
                Item shoes = CurrentOutfit["Shoes"];
                if (shoes == null)
                {
                    path = gameManager.FunctionsLibrary.FunctionExecute("s_PcBodyFeet", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return shoes.Texture;
        }

        return gameManager.TextureCache[path];

    }

    public void itemAdd(Item item)
    {
        if(!items.Contains(item))
            items.setItem(item.id,item);
    }

    public void itemRemove(Item item)
    {
        items.removeItem(item);
        foreach(Outfit outfit in outfits.Values)
        {
            outfit.removeItem(item);
        }
    }

    public void itemsRemove(ItemsFilter itemsFilter)
    {
        /*List<Item> itemsToRemove = new List<Item>();
        foreach(Item item in ((Dictionary<string, Item>)items).Values)
        {
            if(itemsFilter.filter)
        }*/
        IEnumerable<Item> itemsToRemove = itemsFilter.filter(items);

        foreach (Item item in itemsToRemove)
        {
            itemRemove(item);
        }
    }

    public bool itemHas(Item item)
    {
        return items.Contains(item);
    }

    private IEnumerable<string> itemIdsGet()
    {
        return items.Values;
    }

    public void moneyPay(long amount)
    {
        moneyCash -= amount;
        //GameManager.Instance.Update();
    }

    public void timeDayPass()
    {
        //TODO: actual calculation required
        int caloriesRequired = 2000;

        int caloriesRequiredNet = caloriesRequired + statCaloriesBurnt;

        int caloriesToLoseWeightFast = Mathb.RoundToInt(caloriesRequiredNet * 0.6f);
        int caloriesToLoseWeight = Mathb.RoundToInt(caloriesRequiredNet * 0.8f);

        int caloriesToGainWeight = Mathb.RoundToInt(caloriesRequiredNet * 1.25f);
        int caloriesToGainWeightFast = Mathb.RoundToInt(caloriesRequiredNet * 1.67f);

        int weightChange = 0;

        if (statCalories > caloriesToGainWeightFast)
            weightChange = 140;
        else if (statCalories > caloriesToGainWeight)
            weightChange = 70;
        else if (statCalories < caloriesToLoseWeightFast)
            weightChange = -140;
        else if (statCalories < caloriesToLoseWeight)
            weightChange = -70;

        BodyData.Weight += weightChange;

        statCalories = 0;
        statCaloriesBurnt = 0;

    }


    public void timePass(int seconds, Activity activity)
    {
        _statHunger += seconds * activity.statHunger;
        _statHygiene += seconds * activity.statHygiene;
        _statSleep += seconds * activity.statSleep;
    }

    public bool tryRemoveOutfitAndItems(string outfitId="")
    {
        Outfit outfit;

        if (String.IsNullOrWhiteSpace( outfitId))
            outfit = CurrentOutfit;
        else {
            if (!outfits.TryGetValue(outfitId, out outfit))
                return false;
            else
                outfits.Remove(outfitId);
        }

        Item[] itemsToRemove =  outfit.Items.ToArray();
        foreach (Item item in itemsToRemove)
        {
            itemRemove(item);
        }

        

        return true;
    }

    public bool trySaveOutfit(string outfitId)
    {
        outfits[outfitId] = Modable.copyDeep(CurrentOutfit);
        return true;
    }

    public bool trySetOutfit(string outfitId)
    {
        /*if(outfits.TryGetValue(outfitId, out Outfit outfit)){

        }*/
        if (outfits.TryGetValue(outfitId, out Outfit outfit))
        {
            CurrentOutfit = Modable.copyDeep(outfit);
            return true;
        }
        return false;
    }

}
