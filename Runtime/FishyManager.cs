using FishNet.Component.Spawning;
using FishNet.Managing;
using FishNet.Managing.Observing;
using FishNet.Transporting.FishyEOSPlugin;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(NetworkManager)), RequireComponent(typeof(PlayerSpawner)), RequireComponent(typeof(ObserverManager)), RequireComponent(typeof(FishyEOS))]
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
    public bool autoCreate = true;
    public int maxPlayers = 16;

    void Awake() {
        Manager = this;
        networkManager = GetComponent<NetworkManager>();
        transport = GetComponent<FishyEOS>();
        LoadPlayerPref();
    }

    private void Start() {
        Invoke(nameof(CreateServer), 5f);
    }

    private void LoadPlayerPref() {
        nickname = PlayerPrefs.GetString(nicknameKey, "Player" + Random.Range(1111, 9999));
        string colortext = PlayerPrefs.GetString(colorKey, ColorUtility.ToHtmlStringRGB(Random.ColorHSV()));
        ColorUtility.TryParseHtmlString(colortext, out color);
    }

    void Update()
    {
        SavePlayerPref();
        serverCode = transport.RemoteProductUserId;
    }

    private void SavePlayerPref() {
        PlayerPrefs.SetString(nicknameKey, nickname);
        PlayerPrefs.SetString(colorKey, ColorUtility.ToHtmlStringRGB(color));
    }

    public string serverCode;

    public void CreateServer() {
        CreateServer("", 16);
    }

    public static bool CreateServer(string serverCode = "", int maxPlayers = 16) {
        if (string.IsNullOrEmpty(serverCode))
            serverCode = Random.Range(1111, 9999).ToString();
        transport.SetMaximumClients(maxPlayers);
        bool server = networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
        return server;
    }

    public static bool JoinServer(string serverCode) {
        transport.RemoteProductUserId = serverCode;
        return networkManager.ClientManager.StartConnection();
    }
    public static void ChangeNickname(string name) {
        Manager.nickname = name;
    }
    public static void ChangeColor(Color color) {
        Manager.color = color;
    }
}