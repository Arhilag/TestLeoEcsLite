// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase
{
    /// <summary>
    /// Тип ресурса - опыт. Постоянный, значение хранится в PlayerPrefs.
    /// </summary>
    public class Xp : PlayerPrefsIntResource
    {
        public override ResourceType Type => ResourceType.Xp;
        protected override int DefaultValue => 0;

        public Xp(ResourcesSystem resourcesSystem) : base(resourcesSystem)
        {
        }
    }
}