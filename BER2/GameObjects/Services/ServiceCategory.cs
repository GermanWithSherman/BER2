using System.Collections;
using System.Collections.Generic;

public class ServiceCategory //: IPanelData
{
    public string Title;

    public List<Service> Services = new List<Service>();

    public ServiceCategory(string title)
    {
        Title = title;
    }

    public string title()
    {
        return Title;
    }
}
