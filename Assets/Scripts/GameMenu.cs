using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

public class GameMenu : Menus
{
    [SerializeField]
    Corner TLCorner;
    [SerializeField]
    Corner TRCorner;
    [SerializeField]
    Corner BLCorner;
    [SerializeField]
    Corner BRCorner;
    [SerializeField]
    Character Character;
    [SerializeField]
    DataManager DataManager;
    Character[] Characters;
    [SerializeField]
    int ReadyAmount = 0;
    bool[] PlayerList;
    Corner[] Corners;

    PlayerInput[] PlayerInputs;

    public void SetInputs(PlayerInput[] playerInputs)
    {
        PlayerInputs = playerInputs;
    }

    public void SetPlayers(Character[] characters)
    {
        Characters = characters;
    }

    public void SetReadyAmount(int readyAmount)
    {
        ReadyAmount += readyAmount;
    }

    public void FinalReady()
    {
        if (ReadyAmount == 4)
        {
            DataManager.SetInputs(PlayerInputs);
            DataManager.NewScene(false);
        }
    }

    public void CharacterSetup()
    {
        foreach (Character character in Characters)
        {
            character.SetVisible(true);
        }
        Corners = new Corner[4] { TLCorner, TRCorner, BLCorner, BRCorner };
        for (int i = 0; i < Corners.Length; i++)
        {
            Characters[i].transform.position = Corners[i].transform.TransformPoint(Corners[i].GetSpawnLocaiton());
            Characters[i].transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);           
            if (Characters[i] is not AI)
            {
                Characters[i].SetupInput(PlayerInputs[i]);
            }
            Corners[i].SetCharacter(Characters[i]);
            Characters[i].UpdateCharacter(Random.Range(0, 9));
            if ((Characters[i] is AI && !Corners[i].GetReady()) || (Characters[i] is not AI && Corners[i].GetReady()))
            {
                Corners[i].ReadyClicked();
                Corners[i].ToggleButton();
            }

        }
        DataManager.SetCharacters(Characters);
    }

    public void OnBackCicked()
    {
        foreach (Character character in Characters)
        {
            character.SetVisible(false);
        }
        mainMenu.LoadMenu(0);
        mainMenu.ToggleBackground();
        for (int i = 0; i < Corners.Length; i++)
        {
            if (Corners[i].GetReady() && Characters[i] is not AI)
            {
                Corners[i].ReadyClicked();
            }
        }
    }

}
