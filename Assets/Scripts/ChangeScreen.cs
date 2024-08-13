using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScreen : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Debug.Log("Button Pressed, attempting to change scene to: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
