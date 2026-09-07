using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    Character Owner;
    
    public void SetCharacter(Character _character)
    {
        Owner = _character;
    }

    public void RollClicked()
    {
        Owner.OnRollClicked();
    }
}
 