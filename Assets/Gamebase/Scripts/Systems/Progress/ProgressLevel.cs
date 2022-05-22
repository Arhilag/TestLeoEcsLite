// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase
{
    /// <summary>
    /// Тип ресурса - прогресс открытых уровней. Постоянный, значение хранится в PlayerPrefs.
    /// </summary>
    public class ProgressLevel : PlayerPrefsIntResource
    {
        public override ResourceType Type => ResourceType.ProgressLevel;
        protected override int DefaultValue => 0;

        public ProgressLevel(ResourcesSystem resourcesSystem) : base(resourcesSystem)
        {
        }
    }
}