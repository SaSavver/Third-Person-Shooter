using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GlobalStorageConfig _globalStorageConfig;
    [SerializeField] private ScreenController _screenController;

    public override void InstallBindings()
    {
        Container.BindInstance(_globalStorageConfig);
        Container.Bind<ScreenController>().FromComponentInNewPrefab(_screenController).AsSingle().NonLazy();
    }
}