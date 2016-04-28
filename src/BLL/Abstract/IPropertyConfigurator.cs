namespace BLL.Abstract
{
    public interface IPropertyConfigurator
    {
        T Get<T>(params string[] keys);
    }
}
