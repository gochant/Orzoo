﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using MvcSiteMapProvider.Web.Html;
    using MvcSiteMapProvider.Web.Html.Models;
    using Orzoo.AspNet.Extensions;
    using Orzoo.AspNet.Html;
    using Orzoo.AspNet.Mvc;
    using Orzoo.Core.Extensions;
    using Orzoo.Core.Utility;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/SelectWithListNoEmpty.cshtml")]
    public partial class _Views_Shared_EditorTemplates_SelectWithListNoEmpty_cshtml_ : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared_EditorTemplates_SelectWithListNoEmpty_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Shared\EditorTemplates\SelectWithListNoEmpty.cshtml"
  
    var list = ViewData["List"] as List<SelectListItem>;
    var selectedItem = list?.FirstOrDefault(d => d.Value == ViewData.TemplateInfo.FormattedModelValue?.ToString());
    if (selectedItem != null)
    {
        selectedItem.Selected = true;
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\SelectWithListNoEmpty.cshtml"
Write(Html.DropDownListFor(model => model, list,
    new {@class = "form-control", data_bind = "value: data." + ViewData.ModelMetadata.PropertyName}));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
