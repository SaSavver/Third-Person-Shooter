using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsPool<InputComponent> _inputPool;
    private SharedData _sharedData;
    private int _inputEntity;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _inputEntity = _world.NewEntity();
        _inputPool = _world.GetPool<InputComponent>();
        _inputPool.Add(_inputEntity);
    }

    public void Run(IEcsSystems systems)
    {
        var joystickFilter = _world.Filter<JoystickViewComponent>().End(1);
        foreach(var joystickId in joystickFilter)
        {
            ref var joystick = ref _world.GetPool<JoystickViewComponent>().Get(joystickId);  
            var input = joystick.JoystickView.Input;
            var inputVector = new Vector3(input.x, input.y, 0f);
            ref var inputComponent = ref _inputPool.Get(_inputEntity);
            inputComponent.Input = inputVector;
        }
    }
}
