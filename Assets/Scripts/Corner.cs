using Microsoft.Unity.VisualStudio.Editor;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Corner : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Button Button;
    [SerializeField]
    GameMenu GameMenu;
    [SerializeField]
    Vector2 SpawnLocation;
    [SerializeField]
    GameObject ScrollBox;
    [SerializeField]
    TMP_Text TextBox;
    Character Character;
    bool Ready = false;
    public void OnCharacterBoxClicked(int Number)
    {
        Character.UpdateCharacter(Number);
    }

    public void SetCharacter(Character character)
    {
        Character = character;
    }

    public Vector2 GetSpawnLocaiton()
    {
        return SpawnLocation;
    }

    public void ReadyClicked()
    {
        if (Ready) { GameMenu.SetReadyAmount(-1); Button.image.color = Color.white; Ready = false; ScrollBox.SetActive(true);  TextBox.gameObject.SetActive(false); }
        else { GameMenu.SetReadyAmount(+1); Button.image.color = Color.red; Ready = true; ScrollBox.SetActive(false); TextBox.text = Character.GetInfo(); TextBox.gameObject.SetActive(true); }
    }

    public void DisableReady()
    {
        Ready = true;
        GameMenu.SetReadyAmount(+1);
        Button.gameObject.SetActive(false);
        ScrollBox.SetActive(false);
        TextBox.text = Character.GetInfo();
        TextBox.gameObject.SetActive(true);
    }

    public bool GetReady()
    {  
        return Ready; 
    }
}
