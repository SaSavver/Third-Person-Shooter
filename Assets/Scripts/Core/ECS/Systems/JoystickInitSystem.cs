using Leopotam.EcsLite;
public class JoystickInitSystem : IEcsInitSystem
{
    public void Init(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<SharedData>();
        var world = systems.GetWorld();
        var newEntity = world.NewEntity();
        ref var joystickViewComponent = ref world.GetPool<JoystickViewComponent>().Add(newEntity);
        var battleScreenFilter = world.Filter<BattleScreenViewComponent>().End();
        foreach(var id in battleScreenFilter)
        {
            ref var view = ref world.GetPool<BattleScreenViewComponent>().Get(id);
            joystickViewComponent.JoystickView = view.BattleScreen.SosokView;
        }
    }
}