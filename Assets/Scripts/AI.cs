using UnityEngine;
using UnityEngine.InputSystem;

public class AI : Character
{
    private void Start()
    {
        IsAI = true;
    }

    public override void StartTurn()
    {
        ThisMenu.SetCharacter(this);
        ThisMenu.gameObject.SetActive(true);

    }

}
