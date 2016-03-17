#region



#endregion

namespace StandardWeb.Infrastructure
{
    public class Helper
    {
        #region Methods

        #region Public Methods

        public static bool HasPermission(string role)
        {
            //var context = HttpContext.Current;
            //var hasPermission = string.IsNullOrEmpty(role) || context.User.IsInRole(role);

            //if (!hasPermission)
            //{
            //    var validRole = GlobalCachingData.Instance.GetList<Role>().Any(d => d.Name == role);
            //    if (!validRole)
            //    {
            //        hasPermission = true;
            //    }
            //}

            //return hasPermission;
            return true;
        }

        #endregion

        #endregion
    }
}