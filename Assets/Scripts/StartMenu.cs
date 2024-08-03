using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public InputField nameInputField;
    public GameObject startMenuCanvas;

    public void OnStartButtonClicked()
    {
        GameData.PlayerName = nameInputField.text;
        startMenuCanvas.SetActive(false); // Start-Menü ausblenden

        // Rufe die StartGame-Methode des MainManagers auf
        MainManager mainManager = FindObjectOfType<MainManager>();
        if (mainManager != null)
        {
            mainManager.StartGame();
        }
        else
        {
            Debug.LogError("MainManager not found!");
        }
    }
}
