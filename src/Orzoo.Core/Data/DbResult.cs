using System.Collections.Generic;
using System.Linq;

namespace Orzoo.Core.Data
{
    /// <summary>
    /// 数据访问操作结果
    /// </summary>
    public class DbResult
    {
        private readonly List<DbError> _errors = new List<DbError>();

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// 错误集合
        /// </summary>
        public IEnumerable<DbError> Errors => _errors;

        public static DbResult Success { get; } = new DbResult { Succeeded = true };

        public static DbResult Failed(params DbError[] errors)
        {
            var result = new DbResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// 空引用错误
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbResult NullFailed(string type)
        {
            return Failed(new DbError { Type = type, Code = "Null", Description = "对象未找到" });
        }

        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                $"{"Failed"} : {string.Join(",", Errors.Select(x => x.Code).ToList())}";
        }


        public DbResult From(int result)
        {
            return result > 0 ? Success : Failed();
        }

        public Feedback ToFeedback()
        {
            if (Succeeded)
            {
                return Feedback.Success();
            }
            var errors = Errors.Select(d => new ViewError(d.Type + d.Code, d.Description));
            return Feedback.Fail(data: errors);
        }
    }
}
