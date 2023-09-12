using NAudio.Wave;
using System;

namespace Player
{
    internal class AudioPlayer : IDisposable
    {
        private AudioFileReader _audioFileReader;
        private WaveOutEvent _output;

        public float Volume
        {
            get => _output != null ? _output.Volume : 1;
            set
            {
                if (_output != null)
                {
                    _output.Volume = value;
                }
            }
        }

        public event Action? PlaybackResumed;
        public event Action? PlaybackStopped;
        public event Action? PlaybackPaused;

        public AudioPlayer(string fileName)
        {
            _audioFileReader = new AudioFileReader(fileName);
            _output = new WaveOutEvent();

            _output.PlaybackStopped += OnPlaybackStopped;

            _output.Init(_audioFileReader);
        }

        public void Play(PlaybackState playbackState)
        {
            if (playbackState == PlaybackState.Stopped || playbackState == PlaybackState.Paused)
            {
                _output?.Play();
            }
            PlaybackResumed?.Invoke();
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            Dispose();
            PlaybackStopped?.Invoke();
        }

        public void Stop()
        {
            _output?.Stop();
        }

        public void Pause()
        {
            if (_output != null)
            {
                _output.Pause();
                PlaybackPaused?.Invoke();
            }
        }

        public void TogglePlayPause()
        {
            if (_output != null)
            {
                if (_output.PlaybackState == PlaybackState.Playing)
                {
                    Pause();
                }
                else
                {
                    Play(_output.PlaybackState);
                }
            }
            else
            {
                Play(PlaybackState.Stopped);
            }
        }

        public void Dispose()
        {
            _output?.Dispose();
            _output = null!;
            _audioFileReader?.Dispose();
            _audioFileReader = null!;
        }

        public double GetLenghtInSeconds()
        {
            return _audioFileReader != null ? _audioFileReader.TotalTime.TotalSeconds : 0;
        }

        public double GetPositionInSeconds()
        {
            return _audioFileReader != null ? _audioFileReader.CurrentTime.TotalSeconds : 0;
        }
        public void SetPositionInSeconds(double value)
        {
            if (_audioFileReader != null)
            {
                _audioFileReader.CurrentTime = TimeSpan.FromSeconds(value);
            }
        }
    }
}
