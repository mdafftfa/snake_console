using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace snake_console.Utils;

public class AudioSystem : IDisposable
{
    private readonly MixingSampleProvider _mixer;
    private readonly IWavePlayer _output;

    public AudioSystem(int sampleRate = 44100, int channels = 2)
    {
        _mixer = new MixingSampleProvider(
            WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels)
        );
        _mixer.ReadFully = true;

        _output = new WaveOutEvent();
        _output.Init(_mixer);
        _output.Play();
    }

    private ISampleProvider ConvertToMixerFormat(WaveStream stream)
    {
        ISampleProvider input = stream.ToSampleProvider();

        if (stream.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
            input = new MonoToStereoSampleProvider(input);

        if (stream.WaveFormat.SampleRate != _mixer.WaveFormat.SampleRate)
            input = new WdlResamplingSampleProvider(
                input,
                _mixer.WaveFormat.SampleRate
            );

        return input;
    }

    public void PlayMusicLoop(string path, float volume = 0.3f)
    {
        var reader = new AudioFileReader(path) { Volume = volume };
        var loopStream = new LoopStream(reader) { EnableLooping = true };
        var provider = ConvertToMixerFormat(loopStream);
        _mixer.AddMixerInput(provider);
    }

    public void PlaySoundEffect(string path, float volume = 1f)
    {
        var reader = new AudioFileReader(path) { Volume = volume };
        var provider = ConvertToMixerFormat(reader);
        _mixer.AddMixerInput(provider);
    }

    public void Dispose()
    {
        _output?.Stop();
        _output?.Dispose();
    }
}

public class LoopStream : WaveStream
{
    private readonly WaveStream _source;

    public LoopStream(WaveStream source)
    {
        _source = source;
        EnableLooping = true;
    }

    public bool EnableLooping { get; set; }

    public override WaveFormat WaveFormat => _source.WaveFormat;

    public override long Length => _source.Length;

    public override long Position
    {
        get => _source.Position;
        set => _source.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int total = 0;
        while (total < count)
        {
            int read = _source.Read(buffer, offset + total, count - total);
            if (read == 0)
            {
                if (_source.Position == 0 || !EnableLooping)
                {
                    break;
                }
                _source.Position = 0;
            }
            else
            {
                total += read;
            }
        }
        return total;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _source.Dispose();
        base.Dispose(disposing);
    }
}