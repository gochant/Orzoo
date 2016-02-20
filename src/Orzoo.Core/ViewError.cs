namespace Orzoo.Core
{
    /// <summary>
    /// UI端的错误
    /// </summary>
    public class ViewError
    {
        public ViewError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
        public AlertType Level { get; set; }
    }
}
