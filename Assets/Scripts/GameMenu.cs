using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
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
    AI AI;
    [SerializeField]
    DataManager DataManager;
    Character[] Characters = new Character[4];
    [SerializeField]
    int ReadyAmount = 0;
    bool[] PlayerList = new bool[4] { true, true, false, false };
    Corner[] Corners;
    private void Start()
    {
        CharacterCreation();
    }

    public void SetReadyAmount(int Number)
    {
        ReadyAmount += Number;
    }

    public void FinalReady()
    {
        if (ReadyAmount == 4)
        {
            DataManager.NewScene();
        }
    }

    private void CharacterCreation()
    {
        Corners = new Corner[4] { TLCorner, TRCorner, BLCorner, BRCorner };
        for (int i = 0; i < Corners.Length; i++)
        {
            if (PlayerList[i] == true)
            {
                Character.transform.position = Corners[i].GetSpawnLocaiton();
                Characters[i] = Instantiate(Character, Corners[i].transform);
                Corners[i].SetCharacter(Characters[i]);
                
            }
            else
            {
                AI.transform.position = Corners[i].GetSpawnLocaiton();
                Characters[i] = Instantiate(AI, Corners[i].transform);
                Corners[i].SetCharacter(Characters[i]);
                Characters[i].UpdateCharacter(Random.Range(0, 9));
                Corners[i].DisableReady();
            }
            
        }
        DataManager.SetCharacters(Characters);
    }

    public void OnBackCicked()
    {
        mainMenu.LoadMenu(0);
        mainMenu.ToggleBackground();
        for (int i = 0; i < Corners.Length; i++)
        {
            if (Corners[i].GetReady() && PlayerList[i])
            {
                Corners[i].ReadyClicked();
            }
        }
    }

}
