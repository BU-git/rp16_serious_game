using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface IViewComposer
    {
        Task<string> RenderView(string path, object model);
    }
}
