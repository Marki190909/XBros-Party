using System;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    Character Owner;
    [SerializeField]
    GameObject DexterityAbilityBoxes;
    
    public void SetCharacter(Character _character)
    {
        Owner = _character;
    }

    public void RollClicked()
    {
        Owner.OnRollClicked();
    }

    public void NumberClicked(int  _number)
    {
        Owner.OnNumberClicked(_number);
    }

    public void ToggleDexterityBoxes()
    {
        if (DexterityAbilityBoxes.gameObject.activeSelf)
        { 
            DexterityAbilityBoxes.gameObject.SetActive(false);
        }
        else
        {
            DexterityAbilityBoxes.gameObject.SetActive(false);
        }

    }

    public void InventoryClicked()
    {

    }

    public void StatsClicked()
    {

    }
}
 