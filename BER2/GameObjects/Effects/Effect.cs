using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BER2.GameObjects.Effects
{
    [Modable(ModableAttribute.FieldOptions.OptOut)]
    public class Effect : IModable, IModableAutofields
    {
        [JsonIgnore]
        public string ID;
        public bool PCOnly;

        public enum EffectTrigger { All, Event, State, Body }

        [JsonConverter(typeof(StringEnumConverter))]
        public EffectTrigger TriggerStart = EffectTrigger.All;

        [JsonConverter(typeof(StringEnumConverter))]
        public EffectTrigger TriggerEnd = EffectTrigger.All;

        public bool AutoDeactivate = true;

        [JsonIgnore]
        public Condition StartCondition { get => GameManager.Instance.ConditionCache[_startCondition]; }
        [JsonProperty("StartCondition")]
        private string _startCondition;

        [JsonIgnore]
        public Condition EndCondition { get => GameManager.Instance.ConditionCache[_endCondition]; }
        [JsonProperty("EndCondition")]
        private string _endCondition;

        public string Label;
        public enum EffectColor { green, yellow, red}

        [JsonConverter(typeof(StringEnumConverter))]
        public EffectColor Color = EffectColor.green;
        

        public int Attractiveness;
        public int Distress;

        public IModable copyDeep() => throw new NotImplementedException();
        public void mod(IModable modable)=>throw new NotImplementedException();
    }
}
