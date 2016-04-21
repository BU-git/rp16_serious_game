using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Razor.Parser;
using RazorEngine;
using RazorEngine.Templating;

namespace WebUI.Infrastructure.Concrete
{
    public class RazorComposer
    {
        /// <summary>
        /// Compiles the razor file on <code>path</code>
        /// </summary>
        /// <typeparam name="TModel">For strongly typed view</typeparam>
        /// <param name="path"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public string ComposeStringFromRazor<TModel>(string path, TModel viewModel)
        {
            var razorServices = new TemplateService();
            return razorServices.Parse<TModel>(File.ReadAllText(path), viewModel, null, null);
        }

        /// <summary>
        /// Compiles the razor file on <code>path</code>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public string ComposeStringFromRazor(string path, object viewModel)
        {
            var razorServices = new TemplateService();
            return razorServices.Parse(File.ReadAllText(path), viewModel, null, null);
        }
    }
}
