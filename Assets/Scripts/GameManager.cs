using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public static string current_scene = "00_Login";
    public static void ChangeScene(string scene_name) {
        if (scene_name == "00_Login") {
            PlayerManager.Reset();
        }
        SceneManager.LoadScene(scene_name);
        GameManager.current_scene = scene_name;
    }
    public static void Logout() {
        GameManager.ChangeScene("00_Login");
    }
    public static void Exit() {
        Application.Quit();
    }
}