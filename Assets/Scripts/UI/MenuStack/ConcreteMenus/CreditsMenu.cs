using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : BaseMenu
{
    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        menuState = MenuState.CreditsMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in MainMenu. Please assign buttons in the inspector or ensure that they are children of the MainMenu GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Settings")) button.onClick.AddListener(() => context.JumpTo(MenuState.SettingsMenu));
            else if (button.name.Contains("Back")) button.onClick.AddListener(JumpBack);
        }
    }
}