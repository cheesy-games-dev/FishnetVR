using UnityEngine;

public class FishyManagerHud : MonoBehaviour
{
    private int maxPlayers = 16;
    private string serverCode;

    private void OnGUI() {    
        if (GUILayout.Button("Create Server")) {
            FishyManager.Manager.CreateServer(maxPlayers);
        }
        if (GUILayout.Button("Join Server")) {
            FishyManager.Manager.JoinServer(serverCode);
        }
        if (GUILayout.Button("Start Client")) {
            FishyManager.networkManager.ClientManager.StartConnection();
        }
        if (GUILayout.Button("Disconnect")) {
            FishyManager.networkManager.ClientManager.StopConnection();
            FishyManager.networkManager.ServerManager.StopConnection(false);
        }
        GUILayout.Label("Max Players");
        int.TryParse(GUILayout.TextField(maxPlayers.ToString()), out maxPlayers);
        GUILayout.Label("Server Code");
        serverCode = GUILayout.TextField(serverCode);
    }
}
