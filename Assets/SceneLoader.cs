using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Text progressText;
    private AsyncOperation operation;
    private Canvas loadingScreen;

    private void Awake() {
        loadingScreen = GetComponentInChildren<Canvas>(true);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName) {
        UpdateProgressText(0);
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(BeginLoad(sceneName));
    }

    private IEnumerator BeginLoad(string sceneName) {
        operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            UpdateProgressText(operation.progress);
            yield return null;
        }
        UpdateProgressText(operation.progress);
        operation = null;
        loadingScreen.gameObject.SetActive(false);
    }

    private void UpdateProgressText(float progress) {
        progressText.text = (int)(progress * 100f) + "%";
    }
}
