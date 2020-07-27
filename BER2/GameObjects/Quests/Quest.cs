using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Quest : DataExtended
{
    public int State;
    public bool Started;
    public bool Completed;

    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "Active":      return (Started && !Completed);
            case "State":       return State;
            case "Started":     return Started;
            case "Completed":   return Completed;
        }

        return getExtendedData(key);
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "State":       State       = parseInt(value); if (State > 0) Started = true; return;
            case "Started":     Started     = parseBool(value); return;
            case "Completed":   Completed   = parseBool(value); return;
        }

        setExtendedData(key, value);
        /*if (value is string)
            setExtendedData(key, value);
        else if(value is int || value is long)

        else
            throw new NotImplementedException();*/
    }
}

