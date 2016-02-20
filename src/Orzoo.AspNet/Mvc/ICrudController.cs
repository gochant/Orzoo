using System.Threading.Tasks;
using System.Web.Mvc;
using Orzoo.AspNet.Infrastructure;
using Orzoo.Core.Data;
using Orzoo.Core.Linq;

namespace Orzoo.AspNet.Mvc
{
    public interface ICrudController<T, TKey, TDto, TItemDto, TService>
        where T : class, IEntity<TKey>, new()
        where TDto : class, IEntity<TKey>, IDto, new()
        where TItemDto : class, IEntity<TKey>, IDto, new()
        where TService : BaseService<T, TKey>
    {
        #region Methods

        #region Public Methods

        ActionResult Index();
        ActionResult List(AdvanceDataSourceRequest request);
        Task<ActionResult> Entity(string id = null, T data = null, bool isNew = false);
        ActionResult Entity(T data);

        ActionResult Template(string id = null);
        ActionResult EditTemplate(string id = null, T data = null);
        ActionResult DetailTemplate(string id);

        Task<ActionResult> CreateSave(TDto data);
        Task<ActionResult> ModifySave(TDto data);
        Task<ActionResult> DeleteSave(TKey id);

        #endregion

        #endregion
    }
}