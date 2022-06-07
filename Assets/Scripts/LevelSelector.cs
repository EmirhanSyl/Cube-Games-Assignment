using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    bool sceneLoaded;
    void Update()
    {
        if (!sceneLoaded)
        {
            if (PlayerPrefs.HasKey("Level") && PlayerPrefs.GetInt("Level") != 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
            }
            else
            {
                SceneManager.LoadScene(1);
            }
            sceneLoaded = true;
        }
        
    }
}
