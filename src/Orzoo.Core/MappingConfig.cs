using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Orzoo.Core
{
    /// <summary>
    /// 映射配置
    /// </summary>
    /// <typeparam name="TK">键类型</typeparam>
    /// <typeparam name="TV">值类型</typeparam>
    [Serializable]
    [XmlType(TypeName = "mapping")]
    public class MappingConfig<TK, TV>
    {
        [XmlAttribute("key")]
        public TK Key { get; set; }

        [XmlAttribute("value")]
        public TV Value { get; set; }
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
