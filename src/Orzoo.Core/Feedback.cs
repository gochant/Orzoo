using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orzoo.Core
{
    /// <summary>
    /// 反馈
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember(Name = "data")]
        public object Data { get; set; }

        /// <summary>
        /// 临时数据
        /// </summary>
        [DataMember(Name = "temp")]
        public Dictionary<string, object> Temp { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember(Name = "type")]
        public AlertType Type { get; set; } = AlertType.Silent;

        /// <summary>
        /// 数据总数量
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }

        [DataMember(Name = "errors")]
        public object Errors { get; set; }

        public static Feedback Create(bool success, string msg, object data, AlertType type, int total = 0, object errors = null)
        {
            return new Feedback
            {
                Success = success,
                Msg = msg,
                Data = data,
                Type = type,
                Total = total,
                Errors = errors
            };
        }

        public static Feedback CreateData(object data = null, string msg = "", AlertType type = AlertType.Silent)
        {
            return Create(true, msg, data, type);
        }

        public static Feedback CreateSuccess(string msg = null, object data = null, AlertType type = AlertType.Success)
        {
            msg = msg ?? Properties.Resources.OperationSuccessful;
            return Create(true, msg, data, type);
        }

        public static Feedback CreateFail(string msg = null, object data = null, AlertType type = AlertType.Error)
        {

            msg = msg ?? Properties.Resources.OperationFailed;

            data = data ?? "error";
            return Create(false, msg, data, type, 0, data);
        }

        public static Feedback CreateList(object data = null, int total = 0, AlertType type = AlertType.Silent)
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
            return CreateFail(Properties.Resources.UnknownException, ex.Message);
        }
    }
}