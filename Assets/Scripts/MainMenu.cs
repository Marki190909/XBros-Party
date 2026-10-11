using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject Background;
    [SerializeField]
    OpeningMenu openingMenu;
    [SerializeField]
    GameMenu gameMenu;
    [SerializeField]
    GameObject ConfirmationScreen;
    Menus[] menus;
    Menus currentMenu;
    bool[] inputsList = new bool[4] { false, false, false, false };
    int Inputs = 0;
    private PlayerInputManager inputManager;

    private PlayerInput[] playerInputs = new PlayerInput[4];

    private void Start()
    {
        //On start it will set the menus Array to contain all menus and set them all to 
        //Not be active and then load the opening menu (first index of menus).
        inputManager = GetComponent<PlayerInputManager>();
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

    public void LoadGameMenu()
    {
        gameMenu.SetPlayers(inputsList);
        gameMenu.SetInputs(playerInputs);
        LoadMenu(1);
    }
    public void LoadOptionsMenu()
    {

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

    private void OnPlayerJoined(PlayerInput player)
    {
        int slot = 0;
        for (int i = 0; i < playerInputs.Length; i++)
        {
            if (playerInputs[i] == null)
            {
                slot = i;
                break;
            }
        }

        if (currentMenu != openingMenu)
        {
            ConfirmationScreen.SetActive(true);
            currentMenu.gameObject.SetActive(false);
        }

        playerInputs[slot] = player;
        inputsList[slot] = true;
        Inputs++;
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        for (int i = 0; i < playerInputs.Length; i++)
        {
            if (playerInputs[i] == player)
            {
                playerInputs[i] = null;
                inputsList[i] = false;
                Inputs--;
                return;
            }
        }
    }

    public void OnUnderstoodClicked()
    {
        gameMenu.OnBackCicked();
        ConfirmationScreen.SetActive(false);
    }
}

