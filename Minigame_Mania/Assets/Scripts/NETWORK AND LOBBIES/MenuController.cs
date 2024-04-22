using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;


using System.Threading.Tasks;
using Unity.Services.Lobbies;


public class MenuController : MonoBehaviour
{
    //Add Lobby Mananger to networkmanger so that lobby functions can be called from the network manager game object.
    public GameObject LobbyManagerObject;
    private LobbyManager lobbyManager;

    public GameObject ErrorMsgWindow;
   

    [SerializeField] private TMP_InputField JoinCodeInputField;
    [SerializeField] private TextMeshProUGUI LobbyCodeText;
    [SerializeField] private TextMeshProUGUI RelayCodeText;

    private IEnumerator coroutine;

    public static MenuController Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    // Signs in user anonymously to unity services.
    private async void Start()
    {
        LobbyManager lobbyManager = LobbyManagerObject.GetComponent<LobbyManager>();
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //Starts a relay server request and transfers player to lobby scene
    public async Task<string> CreateRelay(int maxPlayers)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("RELAY CODE: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
           
            JoinGameLobby(relayServerData, true);
            return joinCode;
         
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }
        
    //Delays instantiation of player until scene is loaded
    private IEnumerator WaitandSpawn(float waitTime, RelayServerData relayServerData, bool isHost)
    {
            yield return new WaitForSeconds(waitTime);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            if (isHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                NetworkManager.Singleton.StartClient();
            }
        
    }


    //Joins a relay server for P2P using a given joinCode
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            JoinGameLobby(relayServerData, false);

        }
        catch (RelayServiceException e)
        {
            ActivateUIWindow(ErrorMsgWindow);
            Debug.Log(e);
        }
    }
    

    private void JoinGameLobby(RelayServerData relayServerData, bool isHost) {
        coroutine = WaitandSpawn(3, relayServerData, isHost);
        StartCoroutine(coroutine);
        SceneManager.LoadScene("Lobby");
    }


    public void ActivateUIWindow(GameObject activateThisObject)
    {
        activateThisObject.SetActive(true);
    }
    
    public void CloseGame()
    {
        Application.Quit();
    }
}
