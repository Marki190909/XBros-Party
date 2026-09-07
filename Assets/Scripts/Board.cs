using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    Character CurrentPC = new();

    TileBoard aTileBoard = new();

    Queue aQueue;



    public Queue GetQueue()
    {
        return aQueue;
    }
    public Character GetCurrentCharacter()
    {
        return CurrentPC;
    }

    public TileBoard GetTileBoard()
    {
        return aTileBoard;
    }

    public void Initialize(Character[] characters)
    {
        aQueue = new Queue(new Character[80]);
        int[] Rolls = new int[4];
        for(int i = 0; i < 3; i++)
        {
            characters[i].transform.position = new Vector2(6,7);
        }
        for (int i = 0; i < 3; i++)
        {
            Rolls[i] = characters[i].RollDice(1,20);
        }

        Character LargestCharacter;

        int LargestNumber = 0;

        for (int i = 0; i < 3; i++)
        {
            LargestCharacter = null;

            LargestNumber = 0;

            for(int y = 0; i < 3; i++)
            {
                if (Rolls[y] > LargestNumber || characters[y] != null)
                {
                    LargestNumber = Rolls[y];
                    LargestCharacter = characters[y] ; 
                }
                characters[y].SetBoard(this);
                aQueue.GetCharacters()[i] = characters[y];
                characters[y] = null;
            }
        }

        CurrentPC = aQueue.GetCharacters()[0];
    }

    public void OnNumberClicked(int Value)
    {
        if (Value != -1)
        {
            Locations(CurrentPC.RollDice(1, 6, Value));
        }
        else
        {
            Locations(CurrentPC.RollDice(1, 6));
        }
        UpdatePC();
    }




    


    



    public void UpdatePC()
    {
        if (aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].GetIsMonkey() == true)
        {
            int LosingAmount = Convert.ToInt32(Math.Round(CurrentPC.GetGold() / 2d));
            aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].GetMonkeyOwner().SetGold(+LosingAmount);
            CurrentPC.SetGold(-LosingAmount);
        }
        aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].TileMethod();
        PlayerDone();
    }


    public void PlayerDone()
    {
        CurrentPC.SetInputting(false);
        //This Stops the Inputs from the Character that was just done and makes it so the new Character is
        //able to do their inputs###
        CurrentPC = aQueue.NextTurn();
        CurrentPC.StartTurn();
    }

    public void Locations(int roll)
    {
        if (roll > 0)
        {

            List<int> options = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                if (aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].GetConnections()[i] != CurrentPC.GetPreviousPosition() || aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].GetConnections()[i] != 0)
                {
                    options.Add(i);
                }
            }
            if (options.Count > 1)
            {
                //Option = Display(options);
            }
            else
            {
                Move(options[0], roll);
            }
        }
    }

    public void Move(int Location, int roll)
    {
        if(CurrentPC.AbilityCheck(1, 2))
        {
            int QueueSpot = aQueue.PositionOverlapCheck(Location);

            if (QueueSpot != -1)
            {
                aQueue.GetCharacters()[QueueSpot].SetStunned(true);
                CurrentPC.Stats[1] -= 11;
            }
        }
        CurrentPC.SetPosition(Convert.ToInt16(aTileBoard.GetBoardArray()[CurrentPC.GetPosition()].GetConnections()[Location]));
        //It will then move the Character on the Screen Itself to that spot
        Locations(roll - 1);
        if (CurrentPC.Stats[1] < 0)
        {
                CurrentPC.Stats[1] += 11;
        }
    }
}
