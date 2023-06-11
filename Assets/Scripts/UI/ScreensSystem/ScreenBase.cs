using UnityEngine;

public abstract class ScreenBase : MonoBehaviour
{
    public abstract void Init(IScreenData screenData);
    public abstract void OnShow(IScreenData screenData);
    public abstract void OnClose();


    public void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }
}