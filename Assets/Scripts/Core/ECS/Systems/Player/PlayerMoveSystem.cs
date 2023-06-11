using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
    }

    public void Run(IEcsSystems systems)
    {
        var inputFilter = _world.Filter<InputComponent>().End(1);
        foreach(var inputEntity in inputFilter)
        {
            ref var inputComponent = ref _world.GetPool<InputComponent>().Get(inputEntity);
            var input = inputComponent.Input;
            var inputNull = input == new Vector3(0, 0, 0);

            var playerFilter = _world.Filter<PlayerComponent>().End(1);
            foreach (var playerEntity in playerFilter)
            {
                ref var playerComponent = ref _world.GetPool<PlayerComponent>().Get(playerEntity);
                var player = playerComponent.PlayerView;
                var speed = _sharedData.GlobalStorageConfig.PlayerConfig.PlayerMoveSpeed;
                player.PlayerRigidbody.velocity = Vector3.Normalize(input) * speed * Time.fixedDeltaTime;
            }
        }
    }
}
