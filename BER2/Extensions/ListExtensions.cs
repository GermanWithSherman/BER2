
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ListExtensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[BER2.Util.Random.Random.Range(0, list.Count)];
    }
}

