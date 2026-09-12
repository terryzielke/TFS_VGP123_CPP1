using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public BaseMenu[] allMenus;
    public MenuState initState = MenuState.MainMenu;

    public BaseMenu currentMenu => _currentMenu;
    private BaseMenu _currentMenu;

    private Dictionary<MenuState, BaseMenu> menuDictionary = new Dictionary<MenuState, BaseMenu>();
    private Stack<MenuState> menuHistory = new Stack<MenuState>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (allMenus.Length == 0)
        {
            allMenus = GetComponentsInChildren<BaseMenu>(true);
        }

        foreach (BaseMenu menu in allMenus)
        {
            if (menu == null) continue;

            menu.Initialize(this);

            if (!menuDictionary.ContainsKey(menu.menuState))
            {
                menuDictionary.Add(menu.menuState, menu);
            }
            else
            {
                Debug.LogWarning($"Duplicate menu state found: {menu.menuState}. Only the first instance will be used.");
            }
        }

        //jump to the inital state
        JumpTo(initState);
    }

    public void JumpBack()
    {
        if (menuHistory.Count > 0)
        {
            menuHistory.Pop();
            JumpTo(menuHistory.Peek(), false);
        }
        else
        {
            Debug.LogWarning("No previous menu state to jump back to.");
        }
    }

    public void JumpTo(MenuState newState, bool isBackNav = false)
    {
        if (!menuDictionary.ContainsKey(newState))
        {
            Debug.LogWarning($"Menu state {newState} not found in the menu dictionary.");
            return;
        }
        if (_currentMenu == menuDictionary[newState])
        {
            Debug.LogWarning($"Already in the menu state {newState}. No action taken.");
            return;
        }

        if (_currentMenu != null)
        {
            _currentMenu.Exit();
        }

        _currentMenu = menuDictionary[newState];
        _currentMenu.Enter();

        if (!isBackNav)
        {
            if (menuHistory.Count > 0 && menuHistory.Contains(newState))
            {
                List<MenuState> tempList = new List<MenuState>();
                while (menuHistory.Peek() != newState)
                {
                    tempList.Add(menuHistory.Pop());
                }

                menuHistory.Pop(); // Remove the newState from history

                for (int i = tempList.Count - 1; i >= 0; i--)
                {
                    menuHistory.Push(tempList[i]);
                }
            }
            menuHistory.Push(newState);
        }
    }
}