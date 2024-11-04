using Assets.Scripts.Gameplay.Popup;
using Game.Services.Debugging.Gameplay.Services;
using Zenject;

namespace Assets.Scripts.Gameplay.Services
{
    public class PopupServiceInstaller:MonoInstaller
    {
        public PopupNoUI PrefPopupNoUI;
        public override void InstallBindings()
        {
            PopupService popupService = new PopupService(PrefPopupNoUI);
            Container.Bind<PopupService>().FromInstance(popupService).AsSingle();
        }
    }

}
