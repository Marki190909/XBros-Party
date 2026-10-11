using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OpeningMenu : Menus
{
    public void OnPlayClicked()
    {
        //When play is clicked it toggles the background and loads the Game Menu
        mainMenu.ToggleBackground();
        mainMenu.LoadGameMenu();
    }

    public void OnOptionsClicked()
    {
        //When options is clicked it loads the Options Menu
        mainMenu.LoadOptionsMenu();
    }

    public void OnQuitClicked()
    {
        //When quit is clicked it closes the Application
        Application.Quit();
    }
}
