using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public ScreenBase CurrentScreen { get; private set; }
    public ScreenBase CurrentPopup { get; private set; }


    private Dictionary<string, ScreenBase> _screens = new Dictionary<string, ScreenBase>();
    private Dictionary<string, ScreenBase> _popups = new Dictionary<string, ScreenBase>();


    public ScreenBase ShowPopup(Type type, IScreenData screenData)
    {
        if(CurrentPopup != null)
        {
            CurrentPopup.Close();
        }

        var tName = type.Name;
        if (_popups.TryGetValue(tName, out var res))
        {
            EnableCachedScreen(res, screenData);
            CurrentPopup = res;
            return res;
        }
        else
        {
            var instance = SpawnPrefabByPath($"UI/Popups/{tName}");
            ApplyScreenDataOnSpawn(instance, screenData);
            _popups.Add(tName, instance);
            CurrentPopup = instance;
            return instance;
        }
    }

    public ScreenBase ShowScreen(Type type, IScreenData screenData)
    {
        if (CurrentScreen != null)
        {
            CurrentScreen.Close();
        }

        var tName = type.Name;
        if (_screens.TryGetValue(tName, out var res))
        {
            EnableCachedScreen(res, screenData);
            CurrentScreen = res;
            return res;
        }
        else
        {
            var instance = SpawnPrefabByPath($"UI/Screens/{tName}");
            ApplyScreenDataOnSpawn(instance, screenData);
            _screens.Add(tName, instance);
            CurrentScreen = instance;
            return instance;
        }
    }

    private void ApplyScreenDataOnSpawn(ScreenBase screen, IScreenData screenData)
    {
        screen.Init(screenData);
        screen.OnShow(screenData);
    }

    private void EnableCachedScreen(ScreenBase screen, IScreenData screenData)
    {
        screen.gameObject.SetActive(true);
        var rt = screen.transform as RectTransform;
        rt.transform.SetAsLastSibling();
        screen.OnShow(screenData);
    }
    
    private ScreenBase SpawnPrefabByPath(string path)
    {
        var screenPrefab = Resources.Load<ScreenBase>(path);
        var instance = Instantiate(screenPrefab, transform);
        return instance;
    }
}