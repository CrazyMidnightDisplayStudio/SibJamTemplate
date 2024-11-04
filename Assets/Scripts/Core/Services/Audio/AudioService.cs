using Core.Audio.Data;
using Core.Audio.Mixer;
using Core.Audio.Playback;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Audio
{
    public class AudioService
    {
        private readonly AudioMixerController _mixerController;
        private readonly AudioPlayer _audioPlayer;
        private readonly AudioLibrary _audioLibrary;

        public AudioService(AudioMixer audioMixer, AudioLibrary audioLibrary)
        {
            _mixerController = new AudioMixerController(audioMixer);
            _audioPlayer = new AudioPlayer();
            _audioLibrary = audioLibrary;
        }

        // Методы для работы с микшером
        public void SetMusicVolume(float volume) => _mixerController.SetMusicVolume(volume);
        public void SetSfxVolume(float volume) => _mixerController.SetSfxVolume(volume);
        public void SetUiVolume(float volume) => _mixerController.SetUiVolume(volume);
        public void SetMasterVolume(float volume) => _mixerController.SetMasterVolume(volume);

        // Методы для воспроизведения аудио
        public void PlayMusic(string id)
        {
            var clip = _audioLibrary.GetClipById(id);
            if (clip != null)
            {
                _audioPlayer.PlayMusic(clip);
            }
            else
            {
                Debug.LogWarning($"Audio clip with ID {id} not found");
            }
        }

        public void PlaySfx(string id, float volume = 1.0f, AudioSource audioSource = null, bool oneShot = true)
        {
            var clip = _audioLibrary.GetClipById(id);
            if (clip != null)
            {
                _audioPlayer.PlaySfx(clip, volume, audioSource, oneShot);
            }
            else
            {
                Debug.LogWarning($"Audio clip with ID {id} not found");
            }
        }

        public AudioClip GetAudioClipById(string id) => _audioLibrary.GetClipById(id);
        public void StopMusic() => _audioPlayer.StopMusic();
    }
}
