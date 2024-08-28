using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScreen : MonoBehaviour
{
    public static void ChangeScene(string sceneName)
    {
        if (sceneName == "00_Profile")
        {
            // somehow to chyba tutaj
            
        }
        SceneManager.LoadScene(sceneName);
    }
    public static void Exit()
    {
        Application.Quit();
    }
}
