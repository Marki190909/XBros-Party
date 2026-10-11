using System.Collections;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private Character[] Characters;
    public static DataManager instance;
    [SerializeField]
    private bool GameType;
    private int[] TempCharacters;
    PlayerInput[] PlayerInputs;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetInputs(PlayerInput[] playerInputs)
    {
        PlayerInputs = playerInputs;
        foreach (PlayerInput playerInput in PlayerInputs)
        {
            if (playerInput != null)
            {
                playerInput.transform.SetParent(this.transform);
            }
        }
    }

    public void SetCharacters(Character[] characters)
    {
        Characters = characters;
        CloneCharacters(this.transform);
    }

    public void CloneCharacters(Transform transform)
    {
        TempCharacters = new int[Characters.Length];
        for(int i = 0; i < TempCharacters.Length; i++)
        {
            TempCharacters[i] = Characters[i].GetStats();
        }
    }
    public bool GetInfo()
    {
        return GameType;
    }
    public int[] GetCharacters()
    {
        return TempCharacters;
    }

    public PlayerInput[] GetInputs()
    {
        return PlayerInputs;
    }

    public void NewScene(bool Type)
    {
        GameType = Type;
        StartCoroutine(LoadAndSetupScene(1));
    }

    private IEnumerator LoadAndSetupScene(int sceneIndex)
    { 
        var async = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        if (async == null)
            yield break;

        while (!async.isDone)
            yield return null;

        var loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.SetActiveScene(loadedScene);
    }


}
