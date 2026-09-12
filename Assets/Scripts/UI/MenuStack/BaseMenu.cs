using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    MainMenu,
    SettingsMenu,
    CreditsMenu,
    None
}

public abstract class BaseMenu : MonoBehaviour
{
    public Button[] allButtons;
    [HideInInspector] public MenuState menuState;

    protected MenuController context;

    public virtual void Initialize(MenuController context)
    {
        this.context = context;
        allButtons = GetComponentsInChildren<Button>(true);
    }

    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }

    public void JumpBack()
    {
        context.JumpBack();
    }

    public void JumpTo(MenuState newState)
    {
        context.JumpTo(newState);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}