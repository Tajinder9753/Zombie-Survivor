using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float timeToWait = 4f;
    public void ShowGameOverPanel()
    {
        StartCoroutine(OpenUpGameOverPanel());
    }

    //waits a few seconds before opening up the game over panel
    private IEnumerator OpenUpGameOverPanel()
    {
        yield return new WaitForSeconds(timeToWait);
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
