using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    Character CurrentPC;
    TileBoard aTileBoard = new TileBoard();
    [SerializeField]
    private CharacterMenu CharacterMenuPrefab;
    [SerializeField]
    Character CharacterPrefab;
    [SerializeField]
    Camera camera;
    [SerializeField]
    AI AIPrefab;
    Vector2 MoveCameraValue;
    float TimeOfValueX;
    float TimeOfValueY;

    Queue aQueue;

    private void Awake()
    {
        if (DataManager.instance.GetInfo())
        {
            //Load Save
        }
        else
        {
            Initialize(DataManager.instance.GetCharacters(), DataManager.instance.GetInputs());
        }
    }

    private void FixedUpdate()
    { 
        CameraCheck();
    }

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

    public void Initialize(int[] list, PlayerInput[] playerInputs)
    {
        aQueue = new Queue();
        Character[] characters = new Character[4];
        int[] Rolls = new int[4];
        for(int i = 0; i < 4; i++)
        {
            if (playerInputs[i] != null)
            {
                characters[i] = playerInputs[i].GetComponent<Character>();
                characters[i].transform.SetParent(this.transform);
                characters[i].SetupInput(playerInputs[i]);
            }
            else
            {
                characters[i] = Instantiate(AIPrefab, this.transform);
            }
            characters[i].UpdateCharacter(list[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            Rolls[i] = characters[i].RollDice(1,20);
        }
        CharacterMenu ReferenceMenu = Instantiate(CharacterMenuPrefab);
        ReferenceMenu.SetCamera(camera);
        ReferenceMenu.gameObject.SetActive(false);

        foreach (Character character in characters)
        {
            character.SetMenu(ReferenceMenu);
        }

        Character LargestCharacter = null;
        int LargestNumber = 0;

        for (int i = 0; i < 4; i++)
        {
            LargestCharacter = null;

            LargestNumber = 0;

            int NumIndex = 0;

            for(int y = 0; y < 4; y++)
            {
                if (Rolls[y] > LargestNumber && characters[y] != null)
                {
                    LargestNumber = Rolls[y];
                    LargestCharacter = characters[y]; 
                    NumIndex = y;
                }
                
            }
            characters[NumIndex].SetBoard(this);
            aQueue.SetCharacters(i,characters[NumIndex]);
            characters[NumIndex] = null;
        }

        CurrentPC = aQueue.GetCharacters()[0];
        CurrentPC.StartTurn();
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

    public void MoveCamera(Vector2 move)
    {
        if (camera == null)
        {
            camera = Camera.main;
            if (camera == null) return;
        }

        move.x = move.x * TimeOfValueX;
        move.y = move.y * TimeOfValueY;
        Rigidbody2D camerabody = camera.GetComponent<Rigidbody2D>();

        // Calculate half extents of the camera view
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        // Get board bounds
        Bounds boardBounds = this.GetComponent<Renderer>().bounds;

        // Compute allowed center range so the camera viewport stays inside the board
        float minX = boardBounds.min.x + halfWidth;
        float maxX = boardBounds.max.x - halfWidth;
        float minY = boardBounds.min.y + halfHeight;
        float maxY = boardBounds.max.y - halfHeight;

        Vector3 pos = camera.transform.position;

        // Predict next position
        float predictedX = pos.x + move.x;
        float predictedY = pos.y + move.y;

        // If predicted position is outside bounds make it so it only moves the amount that would make it hit the bound
        if (predictedX > maxX && move.x > 0)
        {
            move.x = (maxX - pos.x);
        }
        else if (predictedX < minX && move.x < 0)
        {
            move.x = (minX - pos.x);
        }

        if (predictedY > maxY && move.y > 0)
        {
            move.y = (maxY - pos.y);
        }
        else if (predictedY < minY && move.y < 0)
        {
            move.y = (minY - pos.y);
        }

        camerabody.linearVelocity = move;
    }

    public void SetCameraMoveInput(Vector2 move)
    {
        MoveCameraValue = move;
        if (MoveCameraValue == Vector2.zero)
        {
            TimeOfValueX = 1f;
            TimeOfValueY = 1f;

            if (camera == null)
            {
                camera = Camera.main;
            }

            if (camera != null)
            {
                Rigidbody2D camerabody = camera.GetComponent<Rigidbody2D>();
                if (camerabody != null)
                {
                    camerabody.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    private void CameraCheck()
    {
        if (MoveCameraValue != Vector2.zero)
        {
            MoveCamera(MoveCameraValue);

            TimeOfValueX += 0.2f;
            TimeOfValueY += 0.2f;
        }
    }
}
