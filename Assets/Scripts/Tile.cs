using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.Progress;
using static UnityEditor.ShaderData;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
	protected int[] Connections = new int[3];
	protected string Colour;
	public Board board;
	public bool IsMonkey = false;
	public Character MonkeyOwner = null;
	public virtual void TileMethod()
	{ }

	public int[] GetConnections()
	{
		return Connections;
	}

	public void SetConnection(int index, int value)
	{
		Connections[index] = value;
 	} 

	public Character GetMonkeyOwner()
	{
		return MonkeyOwner;
	}

	public void SetMonkeyOwner(Character character)
	{
		MonkeyOwner = character;
	}

    public bool GetIsMonkey()
	{
		return IsMonkey;
	}

	public void SetIsMonkey(bool State)
	{
		IsMonkey = State;
	}

}

public class BlankTile: Tile
{
}

public class GainCoins : Tile
{
	public override void TileMethod()
	{
		int[] amount = new[] { 0, 1, 1, 1, 1, 3, 3, 3, 3, 5, 5, 10 };
		int number = amount[Random.Range(0, amount.Length)];
		board.GetCurrentCharacter().AddGold(+number) ;
	}
}

public class LoseCoins: Tile
{
			
	public override void TileMethod()
	{ 
		int[] amount = new[] { 0, 1, 1, 1, 1, 3, 3, 3, 3, 5, 5, 10 };
		int number = amount[Random.Range(0, amount.Length)];
		board.GetCurrentCharacter().AddGold(-number);
	}
}

public class TeleportTile : Tile
{	
	public override void TileMethod()
	{
		board.GetCurrentCharacter().SetPosition(Random.Range(1, board.GetTileBoard().GetBoardArray().Length));
	}
}

public class StatTile : Tile
{
	public override void TileMethod()
	{
		int multiplier = 1;
		if (board.GetCurrentCharacter().AbilityCheck(3, 1))
		{
			multiplier = 2;
		}
		int Stat = Random.Range(0, 5);
		switch (Stat)
		{
			case 0:
				print($"You gained Constitution!");
				break;
			case 1:
				print($"You gained Strength!");
				break;
			case 2:
				print($"You gained Dexterity!");
				break;
			case 3:
				print($"You gained Intelligence!");
				break;
			case 4:
				print($"You gained Luck!");
				break;
		}
		board.GetCurrentCharacter().Stats[Stat] = Math.Min(10, (board.GetCurrentCharacter().Stats[Stat] += (1 * multiplier)));
		board.GetTileBoard().UpdateStatTile();
	}
}

public class FightTile : Tile
{
	public override void TileMethod()
	{
		//board.GenerateMinigame(board.Queue[board.Queue.HeadPointer + Random.Range(1, 3)], board.CurrentPC);
	}
}

public class OrbTile : Tile
{

	public override void TileMethod()
	{
		//board.GenerateMinigame(board.CurrentPC, board.Queue[board.Queue.HeadPointer + 1)], board.Queue[board.Queue.HeadPointer + 2], board.Queue[board.Queue.HeadPointer + 3], "Orb");
	}
}

public class ShopTile : Tile
{
    Item[] ItemsToBuy = new Item[3];
	int[] Price = new int[3];
    public override void TileMethod()
	{
		
		for (int i = 0; i < ItemsToBuy.Length; i++)
		{
			int ItemNumber = Random.Range(1, 7);
			switch (ItemNumber)
			{
				case 1:
					ItemsToBuy[i] = new DarkDice();
					break;
				case 2:
					ItemsToBuy[i] = new LightDice();
					break;
				case 3:
					ItemsToBuy[i] = new DoubleDice();
					break;
				case 4:
					ItemsToBuy[i] = new Monkey();
					break; 
				case 5:
					ItemsToBuy[i] = new Yoyo();
					break;
				case 6:
					ItemsToBuy[i] = new BlackHole();
					break;
			}
            Price[i] = Random.Range(5, 21);
        }
			



		//An input of 4 would be the x, and indicates which box was clicked
		
	}

	public void ItemToPurchase(int Number)
    {
		while (Number != 4)
		{
			if (Price[Number] <= board.GetCurrentCharacter().GetGold())
			{
				board.GetCurrentCharacter().GetInventory().Add(ItemsToBuy[Number]);
				board.GetCurrentCharacter().AddGold(-Price[Number]);
			}
		}
    }
}
