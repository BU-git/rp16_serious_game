using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Routing;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class RazorViewComposer : IViewComposer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RazorViewComposer(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor contextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _httpContextAccessor = contextAccessor;
        }

        /// <summary>
        /// Renders the view specified at <see cref="path"/> as a string
        /// </summary>
        /// <param name="path">Path to the view on server (may be full or related)</param>
        /// <param name="model">Model for the specified view to be rendered</param>
        /// <returns>The string representation of view</returns>
        public async Task<string> RenderView(string path, object model)
        {
            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());

            viewDataDictionary.Model = model;

            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var viewResult = _viewEngine.FindView(actionContext, path);

                    var viewContext = new ViewContext(actionContext,
                        viewResult.View, viewDataDictionary,
                        new TempDataDictionary(_httpContextAccessor, _tempDataProvider),
                        stringWriter, new HtmlHelperOptions()
                        );

                    await viewResult.View.RenderAsync(viewContext);
                    stringWriter.Flush();

                    if (viewContext.ViewData != viewDataDictionary)
                    {
                        var keys = viewContext.ViewData.Keys.ToArray();
                        foreach (var key in keys)
                        {
                            viewDataDictionary[key] = viewContext.ViewData[key];
                        }
                    }

                    return stringWriter.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
