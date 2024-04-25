using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class ProjectSceneManager : NetworkBehaviour
{
    [System.Serializable]
    public struct Level
    {
        public Button levelBtn;
        public string sceneName;
    }

    private string m_previousSceneName;
    [SerializeField] Level[] m_Levels;

    private void Awake() 
    {    
        foreach (Level level in m_Levels)
        {
            level.levelBtn.onClick.AddListener(() => {NetworkManager.SceneManager.LoadScene(level.sceneName, LoadSceneMode.Single);});
        }
    }
}
