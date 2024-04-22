using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Runtime.CompilerServices;


public class LobbyManager : MonoBehaviour {

    public static LobbyManager Instance { get; private set; }

    private MenuController menuController;
    public GameObject menuControllerObject;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdatetimer;
    private string playerName;


    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
  
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    //SET LIMIT FOR PLAYERS WITHIN A LOBBY
    int MaxPlayers = 4;

    [SerializeField] private TextMeshProUGUI PlayersJoinedText;
    [SerializeField] private TMP_InputField JoinCodeInputField;

    public GameObject ErrorMsgWindow;

    private IEnumerator LobbyCodecoroutine;

    public enum PlayerColour
    {
        Blue,
        Red,
        Green,
        Yellow
    }

    string KEY_START_GAME = "JoinCode";
    string KEY_PLAYER_NAME = "PlayerName";
    string KEY_PLAYER_COLOUR = "PlayerColour";

    private async void Start() {
        /*
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Player" + UnityEngine.Random.Range(100, 999);
        Debug.Log(playerName);
    */
        MenuController menuController = menuControllerObject.GetComponent<MenuController>();
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    
    private async void HandleLobbyPollForUpdates()
    {
            if (joinedLobby != null)
            {
                lobbyUpdatetimer -= Time.deltaTime;
                if (lobbyUpdatetimer < 0f)
                {
                    float lobbyUpdatetimerMax = 1.1f;
                    lobbyUpdatetimer = lobbyUpdatetimerMax;

                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                    joinedLobby = lobby;

                    OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                    if (!IsPlayerInLobby())
                    {
                        // Player was kicked out of this lobby
                        Debug.Log("Kicked from Lobby!");

                        OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                        joinedLobby = null;
                    }

                    if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                    {
                        if (!IsLobbyHost())
                        {
                        MenuController.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                        }
                      
                    }
                }
            }
     }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            { KEY_PLAYER_COLOUR, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerColour.Blue.ToString()) }
        });
    }

    public async void CreateLobby(){
        try {
            string lobbyName = "TestLob";

            Debug.Log("Starting new game as host");
            Debug.Log("Creating Relay Connection");
            string relayCode = await MenuController.Instance.CreateRelay(MaxPlayers);
            Debug.Log("This is the relay code: " +  relayCode);
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                }
            };
            Debug.Log("Creating Lobby Service");
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MaxPlayers, createLobbyOptions);
         
            hostLobby = lobby;
            joinedLobby = lobby;
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            PrintPlayers(hostLobby);
            UpdateLobbyCode(lobby.LobbyCode);

        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    private void UpdateLobbyCode(string LobbyCode)
    {
        LobbyCodecoroutine = WaitandChangeLobbyCode(3, LobbyCode);
        StartCoroutine(LobbyCodecoroutine);
    }

    private IEnumerator WaitandChangeLobbyCode(float waitTime, string LobbyCode)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject LobbyCodeDisp = GameObject.FindGameObjectWithTag("LobbyCode");
        TextMeshProUGUI textComp = LobbyCodeDisp.GetComponent<TextMeshProUGUI>();
        textComp.text = LobbyCode;
    }

    private async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public string GetLobbyCode()
    {
        Lobby lobby = GetJoinedLobby();
        return lobby.LobbyCode;
    }

    private async void JoinLobbyByCode(string lobbycode)
    {
         try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbycode, joinLobbyByCodeOptions);
            Debug.Log("Joined Lobby with code " + lobbycode);
            PrintPlayers(joinedLobby);
            string relayCode = joinedLobby.Data[KEY_START_GAME].Value;
            MenuController.Instance.JoinRelay(relayCode);
            UpdateLobbyCode(joinedLobby.LobbyCode);

        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            MenuController.Instance.ActivateUIWindow(ErrorMsgWindow);
        }
    }

    /*--------- For Testing ONLY -----------
    private void Awake()
    {
        CreateLobbyBtn.onClick.AddListener(() =>
        {
            CreateLobby(4);
         
        });
        ShowLobbiesBtn.onClick.AddListener(() =>
        {
            PrintPlayers(hostLobby);
         
        });
        JoinLobbyBtn.onClick.AddListener(() =>
        {
            string lobbycode = CodeInputField.text;
            Debug.Log(lobbycode);
            JoinLobbyByCode(lobbycode);
            
        });
    }
    */
    public async void UpdatePlayerColour(PlayerColour playerColour)
    {
        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_COLOUR, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerColour.ToString())
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data[KEY_PLAYER_NAME].Value);
        }
    }

    public async void StartGame()
    {
        try
        {
            Debug.Log("Starting new game as host");

            string relayCode = await MenuController.Instance.CreateRelay(MaxPlayers);

            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
                }
            });

            joinedLobby = lobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void JoinGame()
    {
        string joinCode = JoinCodeInputField.text;
        Debug.Log(joinCode);
        JoinLobbyByCode(joinCode);
    }
}
