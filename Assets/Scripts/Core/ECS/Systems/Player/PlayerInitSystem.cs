using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;

public class PlayerInitSystem : IEcsInitSystem
{
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var sharedData = systems.GetShared<SharedData>();
        var player = sharedData.SceneLinker.Player;
        var weapon = sharedData.GlobalStorageConfig.WeaponConfig.Weapons.FirstOrDefault(wpn => wpn.WeaponID == sharedData.GlobalStorageConfig.PlayerConfig.DefaultWeaponID);

        var entity = world.NewEntity();
        ref var playerComponent = ref world.GetPool<PlayerComponent>().Add(entity);
        ref var animatorComponent = ref world.GetPool<AnimatorComponent>().Add(entity);
        ref var healthComponent = ref world.GetPool<HealthComponent>().Add(entity);
        ref var weaponComponent = ref world.GetPool<WeaponComponent>().Add(entity);
        ref var delayComponent = ref world.GetPool<DelayComponent>().Add(entity);

        playerComponent.PlayerView = player;
        animatorComponent.Animator = player.PlayerAnimator;
        healthComponent.Health = healthComponent.MaxHealth = sharedData.GlobalStorageConfig.PlayerConfig.PlayerMaxHealth;
        weaponComponent.WeaponID = weapon.WeaponID;
        weaponComponent.Damage = weapon.Damage;
        weaponComponent.AttackDelay = weapon.AttackDelay;
        weaponComponent.CriticalMultiplier = weapon.CriticalMultiplier;
        weaponComponent.AttackRange = weapon.AttackRange;

        var hpBarSpawnRequest = world.NewEntity();
        ref var requestEvent = ref world.GetPool<SpawnHealthBarEvent>().Add(hpBarSpawnRequest);
        requestEvent.HealthbarAnkerPos = player.HealthbarPosition;
        requestEvent.Owner = world.PackEntity(entity);
    }
}