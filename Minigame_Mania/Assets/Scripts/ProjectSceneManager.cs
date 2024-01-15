using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class ProjectSceneManager : NetworkBehaviour
{
    [SerializeField] private Button LevelBtn;
    [SerializeField] private string m_SceneName;
    private string m_previousSceneName;

    private void Awake() {
        LevelBtn.onClick.AddListener
        (
            () => {StartCoroutine(LoadSceneAsync());}
        );
    }

    #if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset;
    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            m_SceneName = SceneAsset.name;
        }
    }
    #endif

    public void LoadSceneAsync()
    {
        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {m_SceneName} " +
                        $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }
}
