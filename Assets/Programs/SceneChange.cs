using UnityEngine;
using UnityEngine.SceneManagement; // ← これが必要

public class SceneChange : MonoBehaviour
{
    // インスペクターで指定できるように public にする
    public string nextSceneName;

    // ボタンに登録して呼び出す関数
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("次のシーン名が指定されていません。");
        }
    }
}
