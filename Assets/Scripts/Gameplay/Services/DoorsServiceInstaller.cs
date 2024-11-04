using Zenject;

namespace Game.Services.Debugging.Gameplay.Services
{
    public class DoorsServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DoorsService>().AsSingle().NonLazy();
        }
    }
}