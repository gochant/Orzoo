#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Orzoo.Core.Extensions;
using Orzoo.Core.Utility;

#endregion

namespace Orzoo.AspNet.Html
{
    public static class HtmlHelperExtensions
    {
        #region Methods

        #region Public Methods

        public static void SetDefaultOptions(this TemplateParams tplParams, string templateName = "EditorSection")
        {
            tplParams.TemplateName = tplParams.TemplateName ?? templateName;

            if (templateName == "DisplaySection")
            {
                if (tplParams.Features.Contains(TemplateFeature.GridLayout))
                {
                    var labelCol = tplParams.GetAdditionalViewData("LabelCol") ?? "5";
                    var controlCol = tplParams.GetAdditionalViewData("ControlCol") ?? "7";

                    tplParams.SetOptions("LabelClass", $"control-label col-xs-{labelCol}");
                    tplParams.SetOptions("ControlClass", $"form-control-static col-xs-{controlCol}");
                }
                else
                {
                    // 默认参数
                    tplParams.SetOptions("LabelClass", "control-label");
                    tplParams.SetOptions("ControlClass", "form-control ellipsis-block");
                }
            }
            else
            {
                if (tplParams.Features.Contains(TemplateFeature.GridLayout))
                {
                    tplParams.SetOptions("LabelClass", "control-label col-xs-3");
                    tplParams.SetOptions("ControlClass", "col-xs-9");
                }
                else
                {
                    // 默认参数
                    tplParams.SetOptions("LabelClass", "control-label");
                    tplParams.SetOptions("ControlClass", "");
                }
            }
        }

        public static MvcHtmlString EditorSectionGridLayoutFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression
            , TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.Features.Add(TemplateFeature.GridLayout);

            return html.EditorSectionFor(expression, tplParams);
        }

        public static MvcHtmlString EditorSectionGridLayout(this HtmlHelper html, string expression,
            TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.Features.Add(TemplateFeature.GridLayout);

            return html.EditorSection(expression, tplParams);
        }

        public static MvcHtmlString EditorSectionFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, TemplateParams tplParams = null)
        {
            return html.EditorSection(
                parameters => html.EditorFor(expression, parameters.TemplateName, new {TemplateParams = parameters}),
                parameters => html.DisplayFor(expression, parameters.TemplateName, new {TemplateParams = parameters}),
                () => ModelMetadata.FromLambdaExpression(expression, html.ViewData), tplParams);
        }

        public static MvcHtmlString EditorSection(this HtmlHelper html,
            Func<TemplateParams, MvcHtmlString> editorLogic,
            Func<TemplateParams, MvcHtmlString> displayLogic,
            Func<ModelMetadata> getMetadata,
            TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();
            var isDetail = html.ViewContext.RequestContext.HttpContext.Request.Params["state"] == "detail";
            if (isDetail)
            {
                tplParams.SetOptions("LabelCol", "3");
                tplParams.SetOptions("ControlCol", "9");
            }
            var sectionType = isDetail ? "DisplaySection" : "EditorSection";
            tplParams.SetDefaultOptions(sectionType);
            if (isDetail)
            {
                tplParams.Features.Add(TemplateFeature.DataBind);
                var metadata = getMetadata();
                // 获取编辑字段对应的显示字段
                if (metadata.AdditionalValues.ContainsKey("DisplayField"))
                {
                    var displayField = metadata.AdditionalValues["DisplayField"] as string;
                    if (!string.IsNullOrEmpty(displayField))
                    {
                        return html.Display(displayField, tplParams.TemplateName, new {TemplateParams = tplParams});
                    }
                }

                return displayLogic(tplParams);
                // return html.DisplayFor(expression, tplParams.TemplateName, new { TemplateParams = tplParams });
            }

            return editorLogic(tplParams);
            // return html.EditorFor(expression, tplParams.TemplateName, new { TemplateParams = tplParams });
        }

        public static MvcHtmlString EditorSection(this HtmlHelper html, string expression,
            TemplateParams tplParams = null)
        {
            return html.EditorSection(
                parameters => html.Editor(expression, parameters.TemplateName, new {TemplateParams = parameters})
                , parameters => html.Display(expression, parameters.TemplateName, new {TemplateParams = parameters})
                , () => ModelMetadata.FromStringExpression(expression, html.ViewData)
                , tplParams);
        }

        public static MvcHtmlString DisplaySectionGridLayoutFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.Features.Add(TemplateFeature.GridLayout);

            return html.DisplaySectionFor(expression, tplParams);
        }

        public static MvcHtmlString DisplaySectionDataBindGridLayoutFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.Features.Add(TemplateFeature.GridLayout);
            tplParams.Features.Add(TemplateFeature.DataBind);

            return html.DisplaySectionFor(expression, tplParams);
        }


        public static MvcHtmlString DisplaySectionDataBoundFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.Features.Add(TemplateFeature.DataBind);

            return html.DisplaySectionFor(expression, tplParams);
        }

        public static MvcHtmlString DisplaySectionFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            TemplateParams tplParams = null)
        {
            tplParams = tplParams ?? new TemplateParams();

            tplParams.SetDefaultOptions("DisplaySection");

            return html.DisplayFor(expression, tplParams.TemplateName, new {TemplateParams = tplParams});
        }


        public static MvcHtmlString NameCheckNull(string name)
        {
            return new MvcHtmlString(name + " != null ? " + name + ": '-'");
        }

        public static MvcHtmlString NameCheckNullFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var propName = html.NameFor(expression).ToString();
            return NameCheckNull(propName);
        }

        public static MvcHtmlString NameDate(string name)
        {
            return
                new MvcHtmlString(string.Format(
                    "{0} != null ?  kendo.toString(kendo.parseDate({0}), 'yyyy/MM/dd'): '-'", name));
        }

        public static MvcHtmlString NameDateTime(string name)
        {
            return
                new MvcHtmlString(
                    $"{name} != null ?  kendo.toString(kendo.parseDate({name}), 'yyyy/MM/dd HH:mm:ss'): '-'");
        }

        public static MvcHtmlString NameBoolean(string name)
        {
            return new MvcHtmlString($"{name} ? '是': '否'");
        }

        public static MvcHtmlString NameDecimal(string name)
        {
            return new MvcHtmlString($"{name} != null ?  kendo.toString({name},'n'): '-'");
        }

        public static string JsTypeFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
            return ValueHelper.GetJsType(metadata.ModelType);
        }

        public static string JsType(this HtmlHelper html, string expression)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, new ViewDataDictionary());
            return ValueHelper.GetJsType(metadata.ModelType);
        }

        public static MvcHtmlString ThFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool sortable = false, string cssClass = "")
        {
            var jsType = html.JsTypeFor(expression);
            var propName = html.NameFor(expression).ToString();
            var displayName = html.DisplayNameFor(expression).ToHtmlString();

            return BuildTh(jsType, propName, displayName, sortable, cssClass);
        }

        public static MvcHtmlString Th(this HtmlHelper html,
            string expression, bool sortable = false, string cssClass = "")
        {
            var jsType = html.JsType(expression);
            var propName = html.Name(expression).ToString();
            var displayName = html.DisplayName(expression).ToHtmlString();

            return BuildTh(jsType, propName, displayName, sortable, cssClass);
        }

        public static MvcHtmlString Th(this HtmlHelper html,
            PropertyInfo property, bool sortable = false, string cssClass = "")
        {
            if(CanBeRendered(property))
            {
                return html.Th(property.Name, sortable, cssClass);
            }
            return new MvcHtmlString(string.Empty);
        }

        public static bool CanBeRendered(PropertyInfo property)
        {
            var hint = property.GetAttribute<UIHintAttribute>();
            var scaffold = property.GetAttribute<ScaffoldColumnAttribute>();
            if (hint != null && hint.UIHint == "Hidden" || scaffold != null && scaffold.Scaffold == true)
            {
                return false;
            }
            return true;
        }

        public static MvcHtmlString BuildTh(string jsType, string propName, string displayName, bool sortable = false,
            string cssClass = "")
        {
            var tag = new TagBuilder("th");
            if (sortable)
            {
                cssClass += " sortable";
            }
            tag.AddCssClass(cssClass + " " + jsType);
            tag.Attributes.Add("data-type", jsType);
            tag.Attributes.Add("data-prop", propName);
            tag.SetInnerText(displayName);

            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }

        public static MvcHtmlString BuildTd(string dataTypeName, string propName)
        {
            var innerHtml = NameCheckNull(propName);
            switch (dataTypeName)
            {
                case "Date":
                    innerHtml = NameDate(propName);
                    break;
                case "DateTime":
                    innerHtml = NameDateTime(propName);
                    break;
                case "Boolean":
                    innerHtml = NameBoolean(propName);
                    break;
                case "Decimal":
                    innerHtml = NameDecimal(propName);
                    break;
                case "Double":
                case "Int32":
                case "Float":
                case "String":
                default:
                    break;
            }

            var tag = new TagBuilder("td");
            var contentHtml = "#: " + innerHtml + " #";
            var attrHtml = "#: " + innerHtml + " #";
            tag.MergeAttribute("title", attrHtml);
            // tag.Attributes.Add("title", attrHtml);
            tag.InnerHtml = contentHtml;
            tag.AddCssClass("ellipsis");

            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString()));
        }

        public static MvcHtmlString TdFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string dataType = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
            dataType = dataType ?? metadata.ModelType.GetRealType()?.Name;
            var propName = html.NameFor(expression).ToString();
            return BuildTd(dataType, propName);
        }

        public static MvcHtmlString Td(this HtmlHelper html, string expression, string dataType = null)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, new ViewDataDictionary());
            dataType = dataType ?? metadata.ModelType.GetRealType()?.Name;
            var propName = html.Name(expression).ToString();
            return BuildTd(dataType, propName);
        }

        public static MvcHtmlString Td(this HtmlHelper html, PropertyInfo property, string dataType = null)
        {
            if (CanBeRendered(property))
            {
                return html.Td(property.Name, dataType);
            }
            return null;
        }

        public static string TableFormClass(this HtmlHelper html)
        {
            return "table table-bordered table-condensed table-form-horizontal static";
        }

        public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }

        /// <summary>
        /// 检查是否是 DEBUG
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static bool IsDebug(this HtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
                  return false;
#endif
        }

        #endregion

        #endregion
    }
}