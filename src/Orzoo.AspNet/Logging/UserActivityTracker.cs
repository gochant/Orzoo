using System;
using System.Data.Entity;
using System.Web.Mvc;
using Orzoo.Core.Data;
using Orzoo.Core.Extensions;

namespace Orzoo.AspNet.Logging
{
    /// <summary>
    /// 用户行为跟踪
    /// </summary>
    public class UserActivityTracker
    {
        #region Methods

        #region Public Methods

        public static void Record<T, TKey>(ControllerContext context, IEntity<TKey> data, string actionName = null)
        {
            var namedData = data as INamedEntity<TKey>;
            var dataName = namedData == null ? string.Empty : namedData.Name;

            Record(context, new BasicEntity
            {
                Id = data.Id.ToString(),
                Name = dataName
            }, typeof (T), actionName);
        }

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="context">控制器上下文</param>
        /// <param name="namedData">数据</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="userName">操作者</param>
        public static void Record(ControllerContext context, INamedEntity namedData,
            Type dataType, string actionName = null, string userName = null)
        {
            var ip = context.HttpContext.Request.UserHostAddress;
            var dataName = namedData == null ? string.Empty : namedData.Name;
            var oid = namedData == null ? string.Empty : namedData.Id;

            // 在登录这个操作中，获取不到用户名，因此需要外部传入
            userName = userName ?? context.HttpContext.User.Identity.Name;
            actionName = actionName ?? context.RouteData.Values["action"].ToString();

            var entity = new UserActivity
            {
                Ip = ip,
                Date = DateTime.Now,
                Operator = userName,
                ActionName = actionName,
                ObjectId = oid,
                Name = dataName,
                ObjectType = dataType.GetDisplayName()
            };

            Record(entity);
        }

        public static void Record(UserActivity entity)
        {
            // 保存到数据库
            using (var db = DependencyResolver.Current.GetService<ILogDbContext>())
            {
                var context = (DbContext) db;
                context.Set<UserActivity>().Add(entity);
                context.SaveChanges();
            }
        }

        #endregion

        #endregion
    }
}