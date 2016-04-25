using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Razor;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.ViewFeatures;
using RazorEngine;
using RazorEngine.Templating;

namespace WebUI.Infrastructure.Concrete
{
    public class RazorComposer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public RazorComposer(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderView(string path, ViewDataDictionary viewDataDictionary, ActionContext actionContext, IHttpContextAccessor contextAccessor)
        {
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = _viewEngine.FindView(actionContext, path);

                var viewContext = new ViewContext(actionContext, viewResult.View, viewDataDictionary, new TempDataDictionary(contextAccessor, _tempDataProvider), sw, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                sw.Flush();

                if (viewContext.ViewData != viewDataDictionary)
                {
                    var keys = viewContext.ViewData.Keys.ToArray();
                    foreach (var key in keys)
                    {
                        viewDataDictionary[key] = viewContext.ViewData[key];
                    }
                }

                return sw.ToString();
            }
        }
    }
}
