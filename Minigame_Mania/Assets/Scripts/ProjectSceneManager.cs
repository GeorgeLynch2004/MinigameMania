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

    private void Awake() 
    {    
        LevelBtn.onClick.AddListener(() => {

                NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
        });     
    }
}
