using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Abstract
{
    public interface IPropertyConfigurator
    {
        T Get<T>(params string[] keys);
    }
}
