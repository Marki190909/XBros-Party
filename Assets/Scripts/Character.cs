using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.AppUI.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{

    public int[] Stats = new int[5];
    Sprite sprite;
    int[] HiddenRecords = new int[8];
    int Gold = 0;
    bool Inputting = false;
    bool Stunned = false;
    public bool IsAI = false;
    int Position = 0;

    int PreviousPosition = 0;

    List<Item> Inventory = new List<Item>();



    public bool AbilityCheck(int stat, int number)
    {
        if(Stats[3] >= 8)
        {
            if (Stats[stat] >= number * 4) { return true; }   
            else { return false; }
        }
        else if(Stats[stat] >= number * 5) { return true; }
        else { return false; }
    }
	
	public void StartTurn()
    {
        Inputting = true;
    }

    public void UpdateCharacter(int lineIndex)
    {
        //It takes in an integer as a parameter, then finds the text file containing the stats
        //for the characters, reads it all and at the line of the index specified, splits it
        //so that the stats can be updated, and at index 5 it contains the sprite so updates
        //that as well.
        string[] lines = File.ReadAllLines(Application.dataPath + "/TextFiles/Stats.txt");
        string[] line = lines[lineIndex].Split(',');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 5)
            {
                sprite = Resources.Load<Sprite>("Images/" + line[i]);

                GetComponent<SpriteRenderer>().sprite = sprite;

            }
            else
            {
                Stats[i] = int.Parse(line[i]);
            }
        }
    }

    public string GetInfo()
    {
        //Returns the Stats of the Player so they can be displayed in Character Selection
        return $"Strength: {Stats[0]} \n" +
               $"Vitality: {Stats[1]} \n" +
               $"Dexterity: {Stats[2]} \n" +
               $"Intelligence: {Stats[3]} \n" +
               $"Luck: {Stats[4]} \n";
 
    }

    public int GetPosition()
    {
        return Position;
    }

    public void SetPosition(int Number)
    {
        Position = Number;
    }

    public int GetGold()
    {
        return Gold;
    }
    public void SetGold(int Number)
    {
        Gold = Number;
    }

    public void AddGold(int Number)
    {
        Gold += Number;
    }

    public void SetStunned(bool  stunned)
    {
        Stunned = stunned;
    }

    public bool GetStunned()
    {
        return Stunned;
    }

    public void SetInputting(bool inputting)
    {
        Inputting = inputting; 
    }

    public int GetPreviousPosition()
    {
        return PreviousPosition;
    }

    public List<Item> GetInventory()
    {
        return Inventory;
    }
}