using Core.Audio;
using Debugging;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Services.Debugging
{
    public class DebuggingTestScript : MonoBehaviour
    {
        [Inject] AudioService _audioService;

        public void Test()
        {
            _audioService.PlaySfx("Laser");
        }
    }
}
