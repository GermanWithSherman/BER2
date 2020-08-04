using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.GameObjects.Effects
{
    public class EffectsLibrary : Library<Effect>
    {
        public EffectsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
        {
            this.path = path;
            this.modsPaths = modsPaths;
            if (loadInstantly)
                load(path, modsPaths);
        }

        public IList<Effect> Effects()
        {
            return _dict.Values.ToList();

        }

        internal IList<Effect> IdsToEntrys(IEnumerable<string> ids)
        {
            var result = new List<Effect>();
            if (ids != null)
            {
                foreach (string id in ids)
                {
                    result.Add(_dict[id]);
                }
            }
            return result;
        }

        protected override ModableObjectHashDictionary<Effect> loadFromFolder(string path)
        {
            var result = new ModableObjectHashDictionary<Effect>();

            ModableObjectHashDictionary<EffectsFile> dict = loadFromFolder<EffectsFile>(path);



            foreach (KeyValuePair<string, EffectsFile> kv in dict)
            {
                foreach (KeyValuePair<string, Effect> kv2 in kv.Value.Effects)
                {
                    var effect = kv2.Value;
                    effect.ID = kv2.Key;
                    result.Add(kv2.Key, effect);
                }
            }

            return result;
        }


        public void UpdatePCEffects(PC pc, GameData gameData, Effect.EffectTrigger trigger)
        {
            foreach (KeyValuePair<string, Effect> keyValuePair in _dict)
            {
                Effect effect = keyValuePair.Value;

                if (effect.TriggerStart != Effect.EffectTrigger.All && effect.TriggerStart != trigger && effect.TriggerEnd != Effect.EffectTrigger.All && effect.TriggerEnd != trigger)
                    continue;

                if (pc.Effects.Contains(effect)) { 

                    if (effect.AutoDeactivate)
                    {
                        if (!effect.StartCondition.evaluate(gameData))
                        {
                            pc.EffectDeactivate(effect);
                            Debug.Log($"Deactivated effect {effect.ID} automatically");
                        }
                    }
                    else {
                        if (effect.EndCondition.evaluate(gameData))
                        {
                            pc.EffectDeactivate(effect);
                            Debug.Log($"Deactivated effect {effect.ID}");
                        }
                    }
                }
        
                else
                {
                    
                    if (effect.StartCondition.evaluate(gameData))
                    {
                        pc.EffectActivate(effect);
                        Debug.Log($"Activated effect {effect.ID}");
                    }

                }
            }
        }

    }

}
