using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private Character[] Characters;
    private static DataManager instance;
    [SerializeField]
    private Board BoardPrefab;
    [SerializeField]
    private CharacterMenu CharacterMenuPrefab;
    private Board BoardObject;
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

    public void NewScene()
    {
        SceneManager.LoadScene(1);
        CharacterMenu ReferenceMenu = Instantiate(CharacterMenuPrefab);
        ReferenceMenu.gameObject.SetActive(false);

        foreach (Character character in Characters)
        {
            character.SetMenu(ReferenceMenu);
        }
        BoardObject = Instantiate(BoardPrefab);
        BoardObject.Initialize(Characters);

        
    }

}
