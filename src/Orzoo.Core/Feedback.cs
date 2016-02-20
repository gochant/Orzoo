﻿using System;
using System.Collections.Generic;

namespace Orzoo.Core
{
    public class Feedback
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 临时数据
        /// </summary>
        public Dictionary<string, object> temp { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public AlertType type { get; set; } = AlertType.Silent;

        /// <summary>
        /// 数据总数量
        /// </summary>
        public int total { get; set; }

        public object errors { get; set; }

        public static Feedback Create(bool success, string msg, object data, AlertType type, int total = 0, object errors = null)
        {
            return new Feedback
            {
                success = success,
                msg = msg,
                data = data,
                type = type,
                total = total,
                errors = errors
            };
        }

        public static Feedback Data(object data = null, string msg = "", AlertType type = AlertType.Silent)
        {
            return Create(true, msg, data, type);
        }

        public static Feedback Success(string msg = "操作成功", object data = null, AlertType type = AlertType.Success)
        {
            return Create(true, msg, data, type);
        }

        public static Feedback Fail(string msg = "操作失败", object data = null, AlertType type = AlertType.Error)
        {
            data = data ?? "error";
            return Create(false, msg, data, type, 0, data);
        }

        public static Feedback List(object data = null, int total = 0, AlertType type = AlertType.Silent)
        {
            return Create(true, string.Empty, data, type, total);
        }

        //public static Feedback From(DbEntityValidationException ex)
        //{

        //    var errors = new List<ViewError>();

        //    foreach (var failure in ex.EntityValidationErrors)
        //    {
        //        var typeName = failure.Entry.Entity.GetType().ToString();

        //        foreach (var error in failure.ValidationErrors)
        //        {
        //            errors.Add(new ViewError(typeName + error.PropertyName, error.ErrorMessage));
        //        }
        //    }

        //    //var errorMessages = ex.EntityValidationErrors
        //    //        .SelectMany(x => x.ValidationErrors)
        //    //        .Select(x => x.ErrorMessage);

        //    //var fullErrorMessage = string.Join("; ", errorMessages);

        //    //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //    return Fail(@"数据库验证错误", errors);
        //}

        public static Feedback From(Exception ex)
        {
            return Fail(@"系统出现未知异常，请联系管理员", ex.Message);
        }
    }

    public class Message
    {
        public static string NotFoundData = "未找到该数据";
    }
}