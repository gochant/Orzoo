using Orzoo.Core;
using Orzoo.Core.Enums;
using StandardWeb.Models.PO;

namespace StandardWeb.Services
{
    public partial class PersonService
    {
        protected override bool CanBeEdited(Person data, EditMode mode)
        {
            if(mode == EditMode.Delete)
            {
                LogicContract.Assert(data.Name == "Sam", "名称为Sam的不能被删除");
            }
            return base.CanBeEdited(data, mode);
        }
    }
}