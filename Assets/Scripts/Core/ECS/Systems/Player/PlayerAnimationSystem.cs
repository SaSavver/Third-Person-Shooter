using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSystem : IEcsInitSystem, IEcsRunSystem
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
        foreach (var inputEntity in inputFilter)
        {
            ref var inputComponent = ref _world.GetPool<InputComponent>().Get(inputEntity);
            var input = inputComponent.Input;
            var inputNull = input.magnitude < .1f;

            var up = input.y > _sharedData.GlobalStorageConfig.PlayerConfig.PlayerInputVectorToTurn;
            var down = input.y < -_sharedData.GlobalStorageConfig.PlayerConfig.PlayerInputVectorToTurn;

            var playerFilter = _world.Filter<PlayerComponent>().End(1);
            foreach (var playerEntity in playerFilter)
            {
                ref var playerComponent = ref _world.GetPool<PlayerComponent>().Get(playerEntity);
                var player = playerComponent.PlayerView;

                player.PlayerAnimator.SetBool("Walk", !inputNull);
                if (!inputNull)
                {
                    player.PlayerAnimator.SetInteger("YDir", up ? 1 : (down ? -1 : 0));

                    if(!up && !down)
                        player.PlayerSprite.flipX = input.x > 0;
                }
            }
        }
    }
}
