using System.Threading.Tasks;

namespace WebUI.Infrastructure.Abstract
{
    public interface IViewComposer
    {
        Task<string> RenderView(string path, object model);
    }
}
