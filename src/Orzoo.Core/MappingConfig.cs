using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Orzoo.Core
{

    [Serializable]
    [XmlType(TypeName = "mapping")]
    public class MappingConfig<K, V>
    {
        [XmlAttribute("key")]
        public K Key { get; set; }

        [XmlAttribute("value")]
        public V Value { get; set; }
    }

    [XmlType(TypeName = "mappings")]
    public class MappingConfigs : List<MappingConfig<string, string>>
    {

        public List<string> GetValuesByKey(string key)
        {
            return this.Where(d => d.Key == key).Select(d => d.Value).ToList();
        }

        public string GetValueByKey(string key)
        {
            return this.Where(d => d.Key == key).Select(d => d.Value).FirstOrDefault();
        }
    }

}
