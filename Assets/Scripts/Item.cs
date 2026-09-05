using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEditor.Progress;
public class Item : MonoBehaviour
{
	string Image;
	protected Board board;
	protected string TileType;
    public virtual void ItemMethod()
	{ }
}

class LightDice : Item
{
    public override void ItemMethod()
	{
		board.RollDice(4, 6);
	}
}

class DarkDice : Item
{
	

	public override void ItemMethod()
	{
		board.RollDice(1, 3);	
	}
}

class DoubleDice : Item
{
	public override void ItemMethod()
	{
		board.RollDice(1, 6);
		board.RollDice(1, 6);
	}
}
		
class Monkey: Item
{ 
	
	public override void ItemMethod()
	{
		int input = 1;
		//The player will be able to click the tile they want to set as the Monkey
		board.GetTileBoard().GetBoardArray()[input].SetIsMonkey(true);
		board.GetTileBoard().GetBoardArray()[input].SetMonkeyOwner(board.GetCurrentCharacter());
		board.RollDice(1, 6);
	}
}

class BlackHole : Item
{
	public override void ItemMethod()
	{
		board.GetCurrentCharacter().SetPosition(Random.Range(1, board.GetTileBoard().GetBoardArray().Length));
		//Will play a unique teleporting animation
	}
}
		
class Yoyo: Item
{
	public override void ItemMethod()
	{
		int input = 1;
		//The player will be able to click on a player to swap with
		int temp = board.GetQueue().GetCharacters()[board.GetQueue().PositionOverlapCheck(input)].GetPosition();
		board.GetQueue().GetCharacters()[board.GetQueue().PositionOverlapCheck(input)].SetPosition(board.GetCurrentCharacter().GetPosition());
		board.GetCurrentCharacter().SetPosition(temp);
		//Will play a unique switching animation
	}
}

	
