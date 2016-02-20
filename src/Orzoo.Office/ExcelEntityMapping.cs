using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Orzoo.Core.Extensions;
using Orzoo.Core.Utility;
using Orzoo.Office.Utility;

namespace Orzoo.Office
{
    [XmlType("configs")]
    public class ConfigList<T> : List<T>
    {
        public void ReadXml(XmlReader reader)
        {
            //GenericListDeSerializer<ExcelEntityPropertyMappingConfig> dslzr =
            //                   new GenericListDeSerializer<IProjectMember>();
            //dslzr.Deserialize(reader, this);
        }

        public void WriteXml(XmlWriter writer)
        {
            //GenericListSerializer<T> serializers =
            //                   new GenericListSerializer<T>(this);
            //serializers.Serialize(writer);
        }

    }


    //public class CustomCachingProvider : DefaultCachingProvider
    //{
    //    public void AddDynamic(ITemplateKey key, ITemplateSource source)
    //    {
    //        _dynamicTemplates.AddOrUpdate(key, source, (k, oldSource) =>
    //        {
    //            // This code is from the original template manager implementation.
    //            //if (oldSource.Template != source.Template)
    //            //{
    //            //source.Template 
    //            //throw new InvalidOperationException("The same key was already used for another template!");
    //            //}
    //            return source;
    //        });
    //    }
    //}

    public class ExcelEntityMapping : List<ExcelEntityPropertyMapping>
    {

        /// <summary>
        /// 创建列表属性映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">属性映射配置</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ExcelEntityPropertyMapping CreateListPropertyMapping<T>(ExcelEntityPropertyMappingConfig config, T data)
        {
            // Func<string, int> getIncrease = null
            var dir = config.Direction;
            var propName = config.PropertyName;
            var originCell = config.CalculateCellName(data);
            var lineLimit = RazorTemplateHelper.RunRazorSnippet<int>(config.LineLimit, data);
            var properties = config.ItemProperties;
            var listCount = data.GetPropertyValue<IList>(propName).Count;

            var result = new ExcelEntityPropertyMapping(propName, originCell);
            var originLocation = ExcelHandler.CellNameToLocation(originCell);

            var currLocation = originLocation;

            result.Items = new List<ExcelEntityMapping>();

            for (int i = 0; i < listCount; i++)
            {
                var itemMapping = new ExcelEntityMapping();
                var max = 0;

                // 为每个属性赋值
                for (int j = 0; j < properties.Count; j++)
                {
                    var itemPropName = properties[j].PropertyName;
                    var increase = j == 0 ? 0 : 1;

                    if (!string.IsNullOrEmpty(properties[j].Increase))
                    {

                        increase = RazorTemplateHelper.RunRazorSnippet<int>(properties[j].Increase, data);
                    }

                    if (dir == RenderDirection.Horizontal)
                    {
                        currLocation.ColumnIndex += increase;
                        max = Math.Max(max, currLocation.ColumnIndex);
                    }
                    else
                    {
                        currLocation.RowIndex += increase;
                        max = Math.Max(max, currLocation.RowIndex);
                    }

                    var cellName = ExcelHandler.LocationToCellName(currLocation.ColumnIndex, currLocation.RowIndex);

                    var mapping = new ExcelEntityPropertyMapping(itemPropName, cellName, properties[j].Value);

                    itemMapping.Add(mapping);
                }

                result.Items.Add(itemMapping);

                if (dir == RenderDirection.Horizontal)
                {
                    // 超出限制
                    if (lineLimit != 0 && (currLocation.RowIndex - originLocation.RowIndex + 1) >= lineLimit)
                    {
                        currLocation.RowIndex = originLocation.RowIndex;
                        originLocation.ColumnIndex = currLocation.ColumnIndex = max + 1;

                    }
                    else
                    {
                        currLocation.RowIndex += 1;
                        currLocation.ColumnIndex = originLocation.ColumnIndex;
                    }

                }
                else
                {
                    if (lineLimit != 0 && (currLocation.ColumnIndex - originLocation.ColumnIndex + 1) >= lineLimit)
                    {
                        currLocation.ColumnIndex = originLocation.ColumnIndex;
                        originLocation.RowIndex = currLocation.RowIndex = max + 1;
                    }
                    else
                    {
                        currLocation.ColumnIndex += 1;
                        currLocation.RowIndex = originLocation.RowIndex;
                    }
                }
            }

            return result;
        }

        public static ExcelEntityMapping GenerateMapping<T>(List<ExcelEntityPropertyMappingConfig> configs, T data)
        {
            var result = new ExcelEntityMapping();
            foreach (var item in configs)
            {
                ExcelEntityPropertyMapping mapping;
                if (item.ItemProperties != null && item.ItemProperties.Count != 0)
                {
                    mapping = CreateListPropertyMapping(item, data);
                }
                else
                {
                    mapping = new ExcelEntityPropertyMapping(item.PropertyName, item.CalculateCellName(data), item.Value);
                }
                result.Add(mapping);
            }
            return result;
        }
    }
}
