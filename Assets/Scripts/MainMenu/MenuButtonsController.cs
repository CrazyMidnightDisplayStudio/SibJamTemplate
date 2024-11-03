using Core.Services.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class MenuButtonsController : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _settingButton;

        private SceneTransitionService _sceneTransitionService;

        [Inject]
        private void Construct(SceneTransitionService sceneTransitionService)
        {
            _sceneTransitionService = sceneTransitionService;
        }

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(() => _sceneTransitionService.LoadScene("Gameplay").Forget() );
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(() => _sceneTransitionService.LoadScene("Gameplay").Forget() );
        }
    }
}
