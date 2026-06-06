using NAudio.Wave;

namespace SpaceInvaders.Audio
{
    public sealed class LoopingSampleProvider : ISampleProvider, IDisposable
    {
        private readonly AudioFileReader _reader;

        public LoopingSampleProvider(string fileName)
        {
            _reader = new AudioFileReader(fileName);
        }

        public WaveFormat WaveFormat => _reader.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int totalRead = 0;

            while (totalRead < count)
            {
                int read = _reader.Read(buffer, offset + totalRead, count - totalRead);

                if (read == 0)
                {
                    _reader.Position = 0;
                    continue;
                }

                totalRead += read;
            }

            return totalRead;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
