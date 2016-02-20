using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Reflection;
using Orzoo.Core;
using Orzoo.Core.Extensions;
using Orzoo.Core.Utility;

namespace Orzoo.Office
{
    /// <summary>
    /// DataTable Entity 转换器
    /// </summary>
    /// <typeparam name="T">转换的实体类型</typeparam>
    public class DataTableConverter<T> where T : class, new()
    {
        public MappingConfigs NameMapping { get; set; } = new MappingConfigs();

        public bool AutoResolveColumnName { get; set; } = false;

        public DataTableConverter(MappingConfigs mapper)
        {
            NameMapping = mapper;
        }

        public DataTableConverter()
        {
        }

        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns></returns>
        public string GetMappingColumnName(PropertyInfo property)
        {
            // 获取显示名称

            var columnName = string.Empty;
            if (NameMapping != null)
            {
                columnName = NameMapping.GetValueByKey(property.Name);
            }

            // 从 Attribute 中自动获取对应的列名
            if (string.IsNullOrEmpty(columnName) && AutoResolveColumnName)
            {
                var descriptionAttribute = property.GetAttribute<DescriptionAttribute>();
                if (descriptionAttribute == null)
                {
                    var displayAttr = property.GetAttribute<DisplayAttribute>();
                    if (displayAttr != null)
                    {
                        columnName = displayAttr.Name;
                    }
                }
                else
                {
                    columnName = descriptionAttribute.Description;
                }
            }

            return columnName;
        }

        /// <summary>
        /// 获取附加的数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns></returns>
        public virtual Dictionary<string, object> GetAdditionalData(DataRow row)
        {
            return null;
        }

        /// <summary>
        /// 根据实体属性信息获取数据行中单元格的值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="property">属性信息</param>
        /// <param name="additionalData">额外的数据</param>
        /// <returns></returns>
        public virtual object GetPropertyValue(DataRow row, PropertyInfo property, Dictionary<string, object> additionalData)
        {
            var display = GetMappingColumnName(property);
            object value = null;
            if (!string.IsNullOrEmpty(display))
            {
                value = row.CellValue(display);
            }
            return value;
        }

        /// <summary>
        /// DataRow 转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">数据行</param>
        /// <param name="errors">错误我</param>
        /// <returns></returns>
        public T GetEntity(DataRow row, out List<KeyValuePair<string, string>> errors)
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();
            errors = new List<KeyValuePair<string, string>>();
            var additionalData = GetAdditionalData(row);

            // 对实体的每个属性赋值
            foreach (var property in properties)
            {
                var columnName = GetMappingColumnName(property);  // 列名
                try
                {
                    var value = GetPropertyValue(row, property, additionalData);
                    var safeValue = ValueHelper.GetSafeValue(value, property.PropertyType);
                    if (value != null && safeValue != null)
                    {
                        property.SetValue(entity, safeValue, null);
                    }
                }
                catch (LogicException ex)
                {
                    string key;
                    if (string.IsNullOrEmpty(ex.Key))
                    {
                        key = columnName;
                    }
                    else
                    {
                        key = NameMapping.GetValueByKey(ex.Key);
                        if (string.IsNullOrEmpty(key))
                        {
                            key = ex.Key;
                        }
                    }

                    var error = new KeyValuePair<string, string>(key, ex.Message);
                    errors.Add(error);
                }
                catch (Exception ex)
                {
                    var key = columnName;

                    var error = new KeyValuePair<string, string>(key, ex.Message);
                    errors.Add(error);
                }
            }

            return entity;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public BundleResult<T> GetCollection(DataTable table)
        {
            var list = new List<T>();
            var rows = table.Rows;
            var result = new BundleResult<T>();
            for (int i = 0; i < rows.Count; i++)
            {
                try
                {
                    List<KeyValuePair<string, string>> myError;

                    var dataRow = rows[i];
                    var item = GetEntity(dataRow, out myError);

                    result.Errors.Add(myError);
                    result.Data.Add(item);

                    if (myError.Count > 0)
                    {
                        result.HasError = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new LogicException(string.Format("第{0}行错误：", i + 1) + ex.Message);
                }
            }

            return result;
        }

        public BundleResult<T> Read(string fileName)
        {
            // var set = Read(fileName);
            var dt = ExcelHandler.ToDataTable(fileName);
            return GetCollection(dt);
        }

        public BundleResult<T> Read(Stream stream, string fileName)
        {
            //   var set = Read(file);
            var dt = ExcelHandler.ToDataTable(stream, ExcelHandler.GetExcelVersion(fileName));
            return GetCollection(dt);
        }

        //public BundleResult<T> Read(HttpPostedFileBase file)
        //{
        //    //   var set = Read(file);
        //    var dt = ExcelHandler.ToDataTable(file.InputStream, ExcelHandler.GetExcelVersion(file.FileName));
        //    return GetCollection(dt);
        //}

    }

}
