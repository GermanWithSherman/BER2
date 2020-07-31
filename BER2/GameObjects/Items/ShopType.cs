using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class ShopType : IModable
{
    [JsonIgnore]
    public string ID;

    [JsonIgnore]
    public ItemsFilter Filter
    {
        get
        {
            if (_filter == null)
                _filter = new ItemsFilter();
            return _filter;
        }
        set => _filter = value;
    }

    [JsonProperty("Filter")]
    private ItemsFilter _filter;

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
