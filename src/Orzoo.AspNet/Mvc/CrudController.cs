using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Orzoo.AspNet.Attributes;
using Orzoo.AspNet.Infrastructure;
using Orzoo.AspNet.Logging;
using Orzoo.Core;
using Orzoo.Core.Data;
using Orzoo.Core.Linq;
using Orzoo.Core.Utility;

namespace Orzoo.AspNet.Mvc
{
    /// <summary>
    /// 基础增删改查控制器
    /// </summary>
    /// <typeparam name="T">POCO实体类型</typeparam>
    /// <typeparam name="TDto">用于交换的实体类型</typeparam>
    /// <typeparam name="TItemDto">用于列表的实体类型</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TService"></typeparam>
    public class CrudController<T, TKey, TDto, TItemDto, TService> : BaseController,
        ICrudController<T, TKey, TDto, TItemDto, TService>
        where T : class, IEntity<TKey>, new()
        where TDto : class, IEntity<TKey>, IDto, new()
        where TItemDto : class, IEntity<TKey>, IDto, new()
        where TService : BaseService<T, TKey>
    {
        #region Constructors

        public CrudController(DbContext context)
        {
            Service = (TService) Activator.CreateInstance(typeof (TService), context);
        }

        #endregion

        #region Properties, Indexers

        protected TService Service { get; set; }

        /// <summary>
        /// 是否启用安全删除
        /// </summary>
        public bool UseSafeDelete { get; set; } = false;

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryable">查询表达式</param>
        /// <returns></returns>
        protected ActionResult GetListActionResultByQuerable(AdvanceDataSourceRequest request,
            IQueryable<T> queryable = null)
        {
            var result = Service.GetListResult<TItemDto>(request, queryable);

            return Json(Feedback.List(result.Data, result.Total), JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Public Methods

        public virtual ActionResult ListTemplate(string id = null)
        {
            var m = new TItemDto();
            return id == null ? PartialView(m) : PartialView(id, m);
        }

        // 强制修改
        [HttpPost]
        [Display(Name = "强制修改")]
        public virtual async Task<ActionResult> ForceModifySave(TDto data)
        {
            var entity = Mapper.Map<TDto, T>(data);
            var feedback = await Service.ModifySave(entity, true);
            return Json(feedback);
        }

        // 强制删除
        [HttpPost]
        [Display(Name = "强制删除")]
        public virtual async Task<ActionResult> ForceDeleteSave(TKey id)
        {
            var feedback = UseSafeDelete
                ? await Service.SafeDeleteSave(id, true)
                : await Service.DeleteSave(id, true);
            return Json(feedback);
        }

        public ActionResult Metadata()
        {
            var type = typeof (T);
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, type);
            return Json(metadata, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region ICrudController<T,TKey,TDto,TItemDto,TService> Members

        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult List(AdvanceDataSourceRequest request)
        {
            return GetListActionResultByQuerable(request);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <remarks>（根据ID获取数据库中已有数据，或用户添加功能时带Id的数据的初始数据的获取）</remarks>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        [RequireRouteValues("id")]
        [ValidateModelState(Enabled = false)]
        public virtual async Task<ActionResult> Entity(string id = null, T data = null, bool isNew = false)
        {
            if (id == null || id == "null")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TDto result;
            TKey realId = ValueHelper.GetSafeValue<TKey>(id);

            // 强制是新增实体
            if (isNew)
            {
                result = Service.CreateEntity<TDto>(data);
                result.Id = realId;
                //result.AfterMap();
            }
            else
            {
                result = await Service.GetByIdAsync<TDto>(realId);
            }
            if (result == null)
            {
                throw new LogicException("数据不存在");
            }

            return Json(Feedback.Data(result, isNew.ToString()), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取实体（用于新增功能获取初始值）
        /// </summary>
        [ValidateModelState(Enabled = false)]
        public virtual ActionResult Entity(T data)
        {
            var result = Service.CreateEntity<TDto>(data);
            return Json(Feedback.Data(data: result), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取编辑模板
        /// </summary>
        /// <returns></returns>
        [ValidateModelState(Enabled = false)]
        public virtual ActionResult EditTemplate(string id = null, T data = null)
        {

            var m = id == null ? new TDto() : Service.GetById<TDto>(ValueHelper.GetSafeValue<TKey>(id));
            ModelState.Clear();
            return PartialView(m);
        }

        /// <summary>
        /// 获取详情模板
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DetailTemplate(string id = null)
        {
            var m = id == null ? new TDto() : Service.GetById<TDto>(ValueHelper.GetSafeValue<TKey>(id));
            return PartialView(m);
        }

        /// <summary>
        /// 获取主模板（列表）
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Template(string id = null)
        {
            var m = new TItemDto();
            return id == null ? PartialView(m) : PartialView(id, m);
        }

        /// <summary>
        /// 创建保存
        /// </summary>
        /// <param name="data">DTO</param>
        /// <returns></returns>
        [HttpPost]
        [Display(Name = "新增")]
        [UserActivity]
        public virtual async Task<ActionResult> CreateSave(TDto data)
        {
            var entity = Mapper.Map<TDto, T>(data);
            var result = await Service.CreateSave(entity, false);
            return Json(result);
        }

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Display(Name = "修改")]
        [UserActivity]
        public virtual async Task<ActionResult> ModifySave(TDto data)
        {
            var entity = Mapper.Map<TDto, T>(data);
            var result = await Service.ModifySave(entity, false);
            return Json(result);
        }

        /// <summary>
        /// 删除保存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Display(Name = "删除")]
        [UserActivity]
        public virtual async Task<ActionResult> DeleteSave(TKey id)
        {
            var feedback = UseSafeDelete
                ? await Service.SafeDeleteSave(id, false)
                : await Service.DeleteSave(id, false);
            return Json(feedback);
        }

        #endregion
    }
}