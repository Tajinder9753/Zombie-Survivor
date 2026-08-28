using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject instructionsCanvas;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private float timeToWait = 4f;
    [SerializeField] private AudioClip buttonSound;
    private Sound_Manager soundManager;
    private void Awake()
    {
        soundManager = FindAnyObjectByType<Sound_Manager>();
    }
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
        StartCoroutine(OpenLevelAfterDelay());
    }

    private IEnumerator OpenLevelAfterDelay()
    {
        soundManager.PlaySoundEffect(buttonSound, this.transform, 1f);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        soundManager.PlaySoundEffect(buttonSound, this.transform, 1f);
        Application.Quit();
    }

    public void OpenInstructionsPanel()
    {
        soundManager.PlaySoundEffect(buttonSound, this.transform, 1f);
        mainCanvas.SetActive(false);
        instructionsCanvas.SetActive(true);
    }

    public void CloseInstructionsPanel()
    {
        soundManager.PlaySoundEffect(buttonSound, this.transform, 1f);
        instructionsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
}
