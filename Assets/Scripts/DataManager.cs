using System.Collections;
using Unity.VectorGraphics;
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
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetCharacters(Character[] characters)
    {
        Characters = characters;
        //big man is going to make them here and then set their positions over there!!
    }

    public void NewScene()
    {
        StartCoroutine(LoadAndSetupScene(1));
    }

    private IEnumerator LoadAndSetupScene(int sceneIndex)
    {
        if (Characters != null)
        {
            foreach (var c in Characters)
            {
                if (c != null)
                    DontDestroyOnLoad(c.gameObject);
            }
        }

        var async = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        if (async == null)
            yield break;

        while (!async.isDone)
            yield return null;

        var loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.SetActiveScene(loadedScene);

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
