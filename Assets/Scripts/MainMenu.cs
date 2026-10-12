using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

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
    [SerializeField]
    AI AI;
    [SerializeField]
    Graphic CursorImage;
    Menus[] menus;
    Menus currentMenu;
    Character[] Characters = new Character[4];
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
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i] = Instantiate(AI);
        }
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
        gameMenu.SetPlayers(Characters);
        gameMenu.SetInputs(playerInputs);
        LoadMenu(1);
        gameMenu.CharacterSetup();
    }

    public void LoadOpeningMenu()
    {
        LoadMenu(0);
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

    public void OnPlayerJoined(PlayerInput player)
    {
        int slot = 0;
        for (int i = 0; i < playerInputs.Length; i++)
        {
            if (Characters[i] is AI)
            {
                slot = i;
                break;
            }
        }

        if (currentMenu != openingMenu)
        {
            if (currentMenu == gameMenu)
            {
                foreach (Character character in Characters)
                {
                    character.SetVisible(false);
                }
                gameMenu.OnBackCicked();
            }
            ConfirmationScreen.SetActive(true);
            currentMenu.gameObject.SetActive(false);
        }
        Destroy(Characters[slot].gameObject);
        playerInputs[slot] = player;
        Characters[slot] = player.GetComponent<Character>();
        Characters[slot].GetComponent<VirtualMouseInput>().cursorGraphic = CursorImage;
        Debug.Log(Characters[slot].GetComponent<VirtualMouseInput>().cursorGraphic);
        Inputs++;
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        for (int i = 0; i < playerInputs.Length; i++)
        {
            if (playerInputs[i] == player)
            {
                playerInputs[i] = null;

                if (Characters[i] != null)
                {
                    Destroy(Characters[i].gameObject);
                }

                Characters[i] = Instantiate(AI);
                Inputs--;
                return;
            }
        }
    }

    public void OnUnderstoodClicked()
    {
        ConfirmationScreen.SetActive(false);
        LoadMenu(0);
    }
}

