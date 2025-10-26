using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string nextSceneName;

    public void StartCoopMode()
    {
        GameManager.Instance.isCoopMode = true;
        GameManager.Instance.playerCount = 2;
        GameManager.Instance.InitializeScores();
        SceneManager.LoadScene(nextSceneName);
    }

    public void StartBattleMode()
    {
        GameManager.Instance.isCoopMode = false;
        GameManager.Instance.playerCount = 2;
        GameManager.Instance.InitializeScores();
        SceneManager.LoadScene(nextSceneName);
    }
}
