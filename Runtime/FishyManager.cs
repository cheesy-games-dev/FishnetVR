using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using FishNet.Component.Spawning;
using FishNet.Managing;
using FishNet.Managing.Observing;
using FishNet.Managing.Scened;
using FishNet.Plugins.FishyEOS.Util;
using FishNet.Transporting.FishyEOSPlugin;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Tugboat;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(NetworkManager)), RequireComponent(typeof(PlayerSpawner)), RequireComponent(typeof(ObserverManager)), RequireComponent(typeof(FishyEOS)), RequireComponent(typeof(EOSManager))]
public class FishyManager : MonoBehaviour
{
    public static FishyManager Manager {
        get; private set;
    }
    public Transform Head, LeftHand, RightHand;
    public Color color;
    public string nickname;
    public static NetworkManager networkManager;
    public static FishyEOS transport;
    
    public const string nicknameKey = "f_nickname";
    public const string colorKey = "f_color";
    public bool CreateServerOnStart = true;
    public int maxPlayers = 16;

    void Awake() {
        Manager = this;
        networkManager = GetComponent<NetworkManager>();
        transport = GetComponent<FishyEOS>();
        LoadPlayerPref();
    }

    private void Start() {
        if (CreateServerOnStart) Invoke(nameof(CreateServer), 1f);
    }

    private void LoadPlayerPref() {
        nickname = PlayerPrefs.GetString(nicknameKey, "Player" + Random.Range(1111, 9999));
        string colortext = PlayerPrefs.GetString(colorKey, ColorUtility.ToHtmlStringRGB(Random.ColorHSV()));
        ColorUtility.TryParseHtmlString(colortext, out color);
    }

    void Update()
    {
        SavePlayerPref();
    }

    private void SavePlayerPref() {
        PlayerPrefs.SetString(nicknameKey, nickname);
        PlayerPrefs.SetString(colorKey, ColorUtility.ToHtmlStringRGB(color));
    }

    [ContextMenu("Start Server")]
    public void CreateServer() {
        CreateServer(maxPlayers);
    }

    public bool CreateServer(int maxPlayers = 16) {
        transport.SetMaximumClients(maxPlayers);
        bool server = networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
        return server;
    }

    public bool JoinServer(string serverCode) {
        transport.RemoteProductUserId = serverCode;
        return networkManager.ClientManager.StartConnection();
    }

    public void ChangeNickname(string name) {
        Manager.nickname = name;
    }
    public void ChangeColor(Color color) {
        Manager.color = color;
    }
    public void ChangeMaxPlayers(int maxPlayers = 16) {
        Manager.maxPlayers = maxPlayers;
        transport.SetMaximumClients(maxPlayers);
    }

    public void ServerChangeScene(string scene) {
        if (!networkManager.IsServer)
            return;
        var data = new SceneLoadData(scene);
        data.ReplaceScenes = ReplaceOption.All;
        data.PreferredActiveScene = new(scene);
        networkManager.SceneManager.LoadGlobalScenes(data);
    }
}
