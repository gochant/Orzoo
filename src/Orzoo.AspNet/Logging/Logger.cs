namespace Orzoo.AspNet.Logging
{
    public class Logger
    {
        #region Methods

        #region Public Methods

        public void Log(string component, string message)
        {
            System.Diagnostics.Debug.WriteLine("Component: {0} Message: {1} ", component, message);
        }

        #endregion

        #endregion
    }
}