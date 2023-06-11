using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldStarter : MonoBehaviour
{
    private EcsWorld _world;
    private IEcsSystems _systems;

    [SerializeField] private SceneLinker _sceneLinker;
    [Inject] private GlobalStorageConfig _globalStorageConfig;
    [Inject] private ScreenController _screenController;

    void Start()
    {
        _world = new EcsWorld();

        var sharedData = new SharedData();
        sharedData.SceneLinker = _sceneLinker;
        sharedData.GlobalStorageConfig = _globalStorageConfig;
        sharedData.ScreenController = _screenController;

        _systems = new EcsSystems(_world, sharedData);
        _systems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Add(new BattleUIInitSystem())
            .Add(new PlayerInitSystem())
            .Add(new EnemyInitSystem())
            .Add(new JoystickInitSystem())
            .Add(new InputSystem())
            .Add(new PlayerMoveSystem())
            .Add(new PlayerTargetSearchSystem())
            .Add(new EnemyFollowSystem())
            .Add(new EnemyAttackSystem())
            .Add(new PlayerAnimationSystem())
            .Add(new HealthbarSpawnSystem())
            .Add(new WeaponSystem())
            .Add(new DamageSystem())
            .Add(new HealthbarUpdateSystem())
            .Add(new EnemyDeathSystem())
            .Add(new DropChanceSystem())
            .Add(new DropSpawnSystem())
            .Add(new DropItemMagnetSystem())
            .Add(new ApplyDropSystem())
            .Add(new PlayerHealthSystem())
            .Add(new PlayerDeathSystem())
            .Add(new EnemyRegisterSystem())
            .Add(new EnemySpawnSystem())
            .Add(new TestSystem())
            
            .Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }
        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}
