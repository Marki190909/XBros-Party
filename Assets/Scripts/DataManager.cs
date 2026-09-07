using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private Character[] Characters;
    private static DataManager instance;
    private static Board board;
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
        board.Initialize(Characters);
        Instantiate(board);
    }

}
