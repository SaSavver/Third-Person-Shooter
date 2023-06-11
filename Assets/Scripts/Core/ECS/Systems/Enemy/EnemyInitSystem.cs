using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitSystem : IEcsInitSystem
{
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var sharedData = systems.GetShared<SharedData>();

        var enemyVariants = GameObject.FindObjectsOfType<EnemyView>();
        

        foreach(var enemy in enemyVariants)
        {
            var request = world.NewEntity();

            ref var enemyRegisterRequest = ref world.GetPool<EnemyRegisterRequest>().Add(request);

            enemyRegisterRequest.EnemyView = enemy;
        }
        
    }
}
