using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class QuestsData : Dictionary<string,Quest>
{

    public dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length != 2)
            throw new NotImplementedException();

        string questID = keyParts[0];
        string fieldID = keyParts[1];

        if (!ContainsKey(questID))
            this[questID] = new Quest();

        return this[questID][fieldID];
    }

    public void set(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length != 2)
            throw new NotImplementedException();

        string questID = keyParts[0];
        string fieldID = keyParts[1];

        if (!ContainsKey(questID))
            this[questID] = new Quest();

        this[questID][fieldID] = value;
    }
}

