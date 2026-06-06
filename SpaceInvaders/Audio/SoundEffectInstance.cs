using NAudio.Wave;

namespace SpaceInvaders.Audio
{
    public class SoundEffectInstance : IDisposable
    {
        private readonly WaveOutEvent _output;
        private readonly IDisposable? _source;

        public bool IsLooping { get; }

        public PlaybackState State => _output.PlaybackState;

        public SoundEffectInstance(ISampleProvider provider, IDisposable? source, bool looping)
        {
            IsLooping = looping;
            _source = source;

            _output = new WaveOutEvent
            {
                DesiredLatency = 80
            };

            _output.Init(provider);
        }

        public void Play()
        {
            _output.Play();
        }

        public void Stop()
        {
            _output.Stop();
        }

        public void Dispose()
        {
            _output.Stop();
            _output.Dispose();
            _source?.Dispose();
        }
    }
}
