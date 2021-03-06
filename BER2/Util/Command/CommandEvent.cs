﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

public class CommandEvent : Command
{
    [JsonProperty("Event")]
    private EventStage _event;


    [JsonIgnore]
    public EventStage Event
    {
        get
        {
            if (_event == null)
                return null;
            if (!_event.isInheritanceResolved())
                _event.inherit();
            return _event;
        }
    }

    public string EventID = "";
    public string EventGroup = "";
    public string EventStage = "";

    public CommandEvent() { }

    public CommandEvent(string eventID):this() {
        this.EventID = eventID;
    }

    public override void execute(Data data)
    {
        if (Event != null)
        {
            GameManager.Instance.EventExecute(Event);
        }
        else if (!String.IsNullOrEmpty(EventID))
        {
            GameManager.Instance.eventExecute(EventID);
        }
        else
        {
            GameManager.Instance.EventExecute(EventGroup, EventStage);
        }
    }

    public override IModable copyDeep()
    {
        var result = new CommandEvent();
        result._event = Modable.copyDeep(_event);
        result.EventID = Modable.copyDeep(EventID);
        result.EventGroup = Modable.copyDeep(EventGroup);
        result.EventStage = Modable.copyDeep(EventStage);
        return result;
    }

    private void mod(CommandEvent original, CommandEvent mod)
    {
        _event = Modable.mod(original._event, mod._event);
        EventID = Modable.mod(original.EventID, mod.EventID);
        EventGroup = Modable.mod(original.EventGroup, mod.EventGroup);
        EventStage = Modable.mod(original.EventStage, mod.EventStage);

    }

    public void mod(CommandEvent modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandEvent modCommand = modable as CommandEvent;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
