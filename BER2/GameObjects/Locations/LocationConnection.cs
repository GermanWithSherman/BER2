﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LocationConnection : IModable
{
    [JsonIgnore]
    public SubLocation TargetLocation
    {
        get => GameManager.Instance.LocationCache.SubLocation(TargetLocationId);
    }


    /*private string _targetLocationId = "";
    public string targetLocationId
    {
        get => _targetLocationId;
        set => _targetLocationId = value;
    }*/

    public string TargetLocationId;

    public Conditional<string> TexturePath;
    public Conditional<bool> Visible;

    public ModableTexture Texture {
        get{
            if (TexturePath != null)
                return GameManager.Instance.TextureCache[TexturePath];
            return TargetLocation.TexturePreview;
        }
    }

    public int? Duration;
    [JsonProperty("Label")]
    private string _label;

    [JsonIgnore]
    public string Label
    {
        get
        {
            if (String.IsNullOrEmpty(_label))
                return TargetLocation.Label;
            return _label;
        }
        set => _label = value;
    }

    public string Type = "Walk";
    public bool interruptible = true;

    public bool IsEnabled()
    {
        if (!TargetLocation.isOpen())
            return false;
        return true;
    }

    public OutfitRequirement OutfitRequirement = new OutfitRequirement();

    [JsonProperty("Condition")]
    public string ConditionString;

    [JsonIgnore]
    public Condition Condition { get => GameManager.Instance.ConditionCache[ConditionString]; }


    public LocationConnection() { }

    public void execute()
    {
        if (!OutfitRequirement.isValid(GameManager.Instance.PC.CurrentOutfit))
        {
            ErrorMessage.Show(OutfitRequirement.Instruction);
            return;
        }
        GameManager.Instance.locationTransfer(this);
    }

    internal void linkIds(string locationId)
    {
        if (!String.IsNullOrEmpty(TargetLocationId) && TargetLocationId[0] == '.')
        {
            TargetLocationId = locationId + TargetLocationId;
        }
    }

    private void mod(LocationConnection original, LocationConnection mod)
    {
        Duration = Modable.mod(original.Duration, mod.Duration);
        interruptible = Modable.mod(original.interruptible, mod.interruptible);
        _label = Modable.mod(original._label, mod._label);
        TexturePath = Modable.mod(original.TexturePath, mod.TexturePath);
        Visible = Modable.mod(original.Visible, mod.Visible);
        TargetLocationId = Modable.mod(original.TargetLocationId, mod.TargetLocationId);
        OutfitRequirement = Modable.mod(original.OutfitRequirement, mod.OutfitRequirement);
        Type = Modable.mod(original.Type, mod.Type);
        ConditionString = Modable.mod(original.ConditionString, mod.ConditionString);

    }

    public void mod(LocationConnection modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((LocationConnection)modable);
    }

    public IModable copyDeep()
    {
        var result = new LocationConnection();

        result.Duration = Modable.copyDeep(Duration);
        result.interruptible = Modable.copyDeep(interruptible);
        result._label = Modable.copyDeep(_label);
        result.TexturePath = Modable.copyDeep(TexturePath);
        result.Visible = Modable.copyDeep(Visible);
        result.TargetLocationId = Modable.copyDeep(TargetLocationId);
        result.OutfitRequirement = Modable.copyDeep(OutfitRequirement);
        result.Type = Modable.copyDeep(Type);
        result.ConditionString = Modable.copyDeep(ConditionString);
        return result;
    }
}
