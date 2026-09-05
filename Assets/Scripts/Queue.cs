using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Queue
{
    Character[]? Characters;
    int HeadPointer = 0;
    int TailPointer = 4;


    private Character Dequeue()
    {
        Character temp = Characters[HeadPointer];
        Characters[HeadPointer] = null;
        HeadPointer += 1;
        return temp;
    }

    private void Enqueue(Character Player)
    {
        Characters[TailPointer] = Player;
        TailPointer += 1;
    }

    public Character NextTurn()
    {
        Enqueue(Dequeue());
        return Characters[HeadPointer];
    }

    public int PositionOverlapCheck(int Position)
    {
        int temp = 0;
        int value = -1;
        for (int i = 0; i < 3; i++)
        {
            temp = HeadPointer + i;

            if (Characters[temp].GetPosition() == Position)
                value = temp;
                break;
        }
        return value;
        
    }

    public Character[]? GetCharacters()
    {
        return Characters;
    }
}