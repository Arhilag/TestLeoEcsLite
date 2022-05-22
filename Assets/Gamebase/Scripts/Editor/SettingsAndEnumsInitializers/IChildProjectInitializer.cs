// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public interface IChildProjectInitializer
    {
        void OnChildProjectInit();
        void OnChildProjectReset();
    }
}