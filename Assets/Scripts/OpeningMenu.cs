using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OpeningMenu : Menus
{
    public void OnPlayClicked()
    {
        //When play is clicked it toggles the background and loads the Game Menu
        mainMenu.ToggleBackground();
        mainMenu.LoadMenu(1);
    }

    public void OnOptionsClicked()
    {
        //When options is clicked it loads the Options Menu
        mainMenu.LoadMenu(2);
    }

    public void OnQuitClicked()
    {
        //When quit is clicked it closes the Application
        Application.Quit();
    }
}
