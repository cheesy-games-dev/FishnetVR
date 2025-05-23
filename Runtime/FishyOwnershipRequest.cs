using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class FishyOwnershipRequest : NetworkBehaviour
{
    public bool requestAvailable = true;

    public void RequestOwnership() {
        OnRequestOwnership(LocalConnection);
    }

    public void RemoveLocalOwnership() {
        OnRequestOwnership(LocalConnection);
    }

    [ServerRpc]
    protected virtual void OnRequestOwnership(NetworkConnection connection) {
        if (!requestAvailable)
            return;
        RemoveOwnership();
        GiveOwnership(connection);
    }

    [ServerRpc]
    protected virtual void OnRequestRemoveOwnership(NetworkConnection connection) {
        if (connection != Owner)
            return;
        RemoveOwnership();
    }

    [Server]
    public void RemoveOwnershipFromObject() {
        NetworkObject.RemoveOwnership();
    }
}
