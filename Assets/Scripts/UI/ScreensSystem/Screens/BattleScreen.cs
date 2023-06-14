using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScreen : ScreenBase
{
    [SerializeField] private SosokView _sosokView;
    [SerializeField] private Transform _healthbarSpawnRoot;
    [SerializeField] private ExpBarView _expBarView;

    public Transform HealthbarSpawnRoot => _healthbarSpawnRoot;
    public SosokView SosokView => _sosokView;
    public ExpBarView ExpBarView => _expBarView;


    public override void Init(IScreenData screenData)
    {
    }

    public override void OnClose()
    {
     
    }

    public override void OnShow(IScreenData screenData)
    {

    }
}
