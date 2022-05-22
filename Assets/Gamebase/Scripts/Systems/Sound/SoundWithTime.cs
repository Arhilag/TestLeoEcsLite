// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Gamebase
{
    public class SoundWithTime
    {
        public Sound sound;
        public float time;
        public bool unscaled;

        public SoundWithTime(Sound sound, float time)
        {
            this.sound = sound;
            this.time = time;
        }

        public SoundWithTime(Sound sound)
        {
            this.sound = sound;
            unscaled = true;
        }
    }
}