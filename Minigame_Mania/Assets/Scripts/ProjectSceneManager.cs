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
        
        if (NetworkManager.IsHost)
        {
            LevelBtn.onClick.AddListener(() => {
                SceneManager.LoadSceneAsync(m_SceneName);
            });     
        }

          
    }

    
   
}
