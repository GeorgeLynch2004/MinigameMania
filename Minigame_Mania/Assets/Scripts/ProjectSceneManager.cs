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
        
        LevelBtn.onClick.AddListener(() => {

            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Is host");
                NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            }
            else Debug.Log("Is not host");
        });     

          
    }

    
   
}
