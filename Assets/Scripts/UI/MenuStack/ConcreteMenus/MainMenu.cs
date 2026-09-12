using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        menuState = MenuState.MainMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in MainMenu. Please assign buttons in the inspector or ensure that they are children of the MainMenu GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Start")) button.onClick.AddListener(() => SceneManager.LoadScene("2.Game"));
            else if (button.name.Contains("Settings")) button.onClick.AddListener(() => context.JumpTo(MenuState.SettingsMenu));
            else if (button.name.Contains("Credits")) button.onClick.AddListener(() => context.JumpTo(MenuState.CreditsMenu));
            else if (button.name.Contains("Quit")) button.onClick.AddListener(() => QuitGame());
        }

    }
}