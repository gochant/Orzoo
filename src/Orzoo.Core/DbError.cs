namespace Orzoo.Core
{
    /// <summary>
    /// ���ݿ����
    /// </summary>
    public class DbError : IDbError
    {
        /// <summary>
        /// �����������ͣ����飩
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ���������ֶΣ����룩
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string Description { get; set; }
    }
}