using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public static string current_scene = "00_Login";
    public static void ChangeScene(string scene_name) {
        if (scene_name == "00_Login") {
            PlayerManager.Reset();
        } else if (scene_name == "03_Clothes") {
            ItemManager.FetchClothes();
        } else if (scene_name == "07_Wellness") {
            NetworkManager.FetchEndurance(PlayerManager.GetName());
        }
        SceneManager.LoadScene(scene_name);
        current_scene = scene_name;
    }
    public static void Logout() {
        ChangeScene("00_Login");
    }
    public static void Exit() {
        Application.Quit();
    }
}