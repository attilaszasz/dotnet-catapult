
namespace Interfaces
{
    public interface IConfigurationService
    {
        T Get<T>(string name) where T : new();
        string GetString(string name);
    }
}
