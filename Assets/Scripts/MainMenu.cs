using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject Background;
    [SerializeField]
    OpeningMenu openingMenu;
    [SerializeField]
    GameMenu gameMenu;
    Menus[] menus;
    Menus currentMenu;

    private void Start()
    {
        

        //On start it will set the menus Array to contain all menus and set them all to 
        //Not be active and then load the opening menu (first index of menus).
        menus = new Menus[2] { openingMenu, gameMenu };
        foreach (var menu in menus)
        { menu.gameObject.SetActive(false); }
        LoadMenu(0);
    }

    public void LoadMenu(int Index)
    {
        //If there is a current menu it will set it to false, and then look through the Menus
        //Array which contains all the Menus, and use the parameter that was passed and set
        //that Menu to be active and replace the new current Menu.
        if (currentMenu != null)
        {
            currentMenu.gameObject.SetActive(false);
        }
        menus[Index].mainMenu = this;
        menus[Index].gameObject.SetActive(true);
        currentMenu = menus[Index];
    }
    
    public void ToggleBackground()
    {
        //This will toggle the background by swapping its bool state for SetActive
        if(Background.activeSelf == true)
        {
                Background.SetActive(false);  
        }
        else {Background.SetActive(true);}
    }

}

