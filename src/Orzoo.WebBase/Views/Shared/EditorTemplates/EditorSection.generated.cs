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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/EditorSection.cshtml")]
    public partial class _Views_Shared_EditorTemplates_EditorSection_cshtml_ : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared_EditorTemplates_EditorSection_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
  

    TemplateHelper.RemoveModelFromVisitedObjects(ViewData);

    var templateParams = ViewData["TemplateParams"] as TemplateParams ?? new TemplateParams();

    var labelClass = templateParams.GetAdditionalViewData("LabelClass");
    var controlClass = templateParams.GetAdditionalViewData("ControlClass");
    var controlTemplate = templateParams.GetAdditionalViewData("UIHint")?.ToString() ?? ViewData.ModelMetadata.TemplateHint;
    //var dataBind = templateParams.GetAdditionalViewData("DataBind");

    labelClass = labelClass + (ViewData.ModelMetadata.IsRequired ? " required" : "");

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
 if (controlTemplate == "Hidden")
{
    
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
Write(Html.EditorFor(m => m, controlTemplate, templateParams.HtmlFieldName, templateParams.AdditionalViewData));

            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
                                                                                                             
}
else
{
    if (templateParams.Features.Contains(TemplateFeature.GridLayout))
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 23 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
       Write(Html.LabelFor(m => m, new {@class = labelClass, title = Html.DisplayNameFor(m => m)}));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1000), Tuple.Create("\"", 1023)
            
            #line 25 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
, Tuple.Create(Tuple.Create("", 1008), Tuple.Create<System.Object, System.Int32>(controlClass
            
            #line default
            #line hidden
, 1008), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 26 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
           Write(Html.EditorFor(m => m, controlTemplate, templateParams.HtmlFieldName, templateParams.AdditionalViewData));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n");

            
            #line 29 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
    }
    else
    {
        
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
   Write(Html.LabelFor(m => m, new {@class = labelClass, title = Html.DisplayNameFor(m => m)}));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
                                                                                              
        
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
   Write(Html.EditorFor(m => m, controlTemplate, templateParams.HtmlFieldName, templateParams.AdditionalViewData));

            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Shared\EditorTemplates\EditorSection.cshtml"
                                                                                                                 
    }
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
