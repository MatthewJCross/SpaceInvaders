using NAudio.Wave;
using System.Diagnostics;
using System.IO;

namespace SpaceInvaders.Audio
{
    public class AudioEngine
    {
        private readonly Dictionary<string, SoundEffect> _sounds = new();
        private readonly List<SoundEffectInstance> _playing = new();

        public void Load(string key, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            _sounds[key] = new SoundEffect(filename);
        }

        public SoundEffectInstance? Play(string key, bool loop = false)
        {
            if (!_sounds.TryGetValue(key, out var sound))
                return null;

            SoundEffectInstance instance;

            if (loop)
            {
                var loopProvider = new LoopingSampleProvider(sound.FilePath);
                instance = new SoundEffectInstance(loopProvider, loopProvider, true);
            }
            else
            {
                var reader = new AudioFileReader(sound.FilePath);
                instance = new SoundEffectInstance(reader.ToSampleProvider(), reader, false);
            }

            _playing.Add(instance);
            instance.Play();
            return instance;
        }

        public void Stop(SoundEffectInstance? instance)
        {
            if (instance == null)
                return;

            instance.Stop();
            instance.Dispose();

            _playing.Remove(instance);
        }

        public void StopAll()
        {
            foreach (var sound in _playing)
            {
                sound.Stop();
                sound.Dispose();
            }

            _playing.Clear();
        }

        public void Update()
        {
            for (int i = _playing.Count - 1; i >= 0; i--)
            {
                var sound = _playing[i];

                if (!sound.IsLooping && sound.State == PlaybackState.Stopped)
                {
                    sound.Dispose();
                    _playing.RemoveAt(i);
                }
            }
        }
    }
}
