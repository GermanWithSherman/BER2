using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.GameObjects.Effects
{
    public class EffectsFile : IModable
    {
        public ModableObjectHashDictionary<Effect> Effects = new ModableObjectHashDictionary<Effect>();

        public IModable copyDeep()
        {
            var result = new EffectsFile();
            result.Effects = Modable.copyDeep(Effects);
            return result;
        }

        public void mod(IModable modable)
        {
            throw new System.NotImplementedException();
        }
    }
}
