using System;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using Object = System.Object;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "TileBoard", menuName = "Scriptable Objects/TileBoard")]
public class TileBoard
{


	int StatLocation = 0;
	Tile[] BoardArray = new Tile[80];


	public TileBoard()
	{
		//InitializeArray(BoardArray);
	}


	void InitializeArray(Tile[] array)
	{
		string[] Lines = File.ReadAllLines(Application.dataPath + "/TextFiles/PreMÁap.txt");

        for (int i = 0; i < array.Length; i++)
		{
            string[] Line = Lines[i].Split(',');
            array[i] = RandomTile();
			array[i].SetConnection(0, Convert.ToInt32(Line[1]));
            array[i].SetConnection(1, Convert.ToInt32(Line[2]));
            array[i].SetConnection(2, Convert.ToInt32(Line[3]));
        }
		//There will also have to be prepicked spots for the boss fights
		//in order to guarantee there will be a boss fight in each zone
		//--->array[x, 0] = new BossFight()
		//There will also have to be one random spot picked for the first
		//stat tile


		UpdateStatTile();

	}
	private Tile RandomTile()
	{

		int[] TileSway = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 5, 8 };;
		switch (TileSway[Random.Range(0,TileSway.Length)])
		{
			case 1:
				return new BlankTile();
			case 2:
				return new GainCoins();
			case 3:
				return new LoseCoins();
			case 4:
				return new TeleportTile();
			case 5:
				return new FightTile();
			case 8:
				return new ShopTile();
			default:
				return new BlankTile();
		}
	}


    public void UpdateStatTile()
	{
		//Work here due to new BoardArray declaration
		BoardArray[StatLocation] = RandomTile();


		StatLocation = Random.Range(1, 80);
		while (BoardArray[StatLocation].ToString() == "1")
		{
			StatLocation = Random.Range(1, 80);
		}

		BoardArray[StatLocation] = new StatTile();
	}

	public int GetStatLocation()
	{
		return StatLocation;
	}

	public Tile[] GetBoardArray()
	{
		return BoardArray;
	}
}
