using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Character[] Characters;
    private static DataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCharacters(Character[] characters)
    {
        Characters = characters;
    }


}
