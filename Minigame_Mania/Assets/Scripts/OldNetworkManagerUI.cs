using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ClientBtn;
    [SerializeField] private TextMeshProUGUI PlayersJoinedText;
    [SerializeField] private GameManager gameManager;

    private void Awake() 
    {
        // create host if host button is clicked
        HostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
       });

        // create client session if client button is clicked
        ClientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }

}
