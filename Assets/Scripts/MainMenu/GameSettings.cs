using Core.Audio;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.MainMenu
{
    public class GameSettings: MonoBehaviour
    {
        private AudioService _audioService;

        [Inject]
        public void Construct(AudioService audioSource)
        {
            _audioService = audioSource;
        }
        #region Audio
        public void Master(float value)
        {
            _audioService.SetMasterVolume(value);
        }
        public void Music(float value)
        {
            _audioService.SetMusicVolume(value);
        }
        public void SFX(float value)
        {
            _audioService.SetSfxVolume(value);
        }
        #endregion

        #region Screen
        public void IsFullScreen(bool value) => Screen.fullScreen = value;
        #endregion
    }
}
