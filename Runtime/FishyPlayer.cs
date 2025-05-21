using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct CosmeticSlot {
    public string slotKey;
    public bool hideLocalPlayer;
    public List<CosmeticPiece> cosmeticPieces;
}

[System.Serializable]
public struct CosmeticPiece {
    public string pieceKey;
    public GameObject cosmeticObject;
}

public class FishyPlayer : NetworkBehaviour
{
    public static FishyPlayer LocalPlayer {
    get; private set; }
    [SyncVar] public string Nickname;
    [SyncVar] public Color Color;

    public Transform Head, LeftHand, RightHand;

    public Renderer[] Renderer;

    public TMP_Text NicknameTMP;
    public List<CosmeticSlot> cosmeticSlots;

    public bool hideLocalPlayer;

    [ServerRpc]
    public void ChangeCosmetic(string slotKey, string pieceKey) {
        ChangeCosmeticClients(slotKey, pieceKey);
    }

    [ObserversRpc]
    private void ChangeCosmeticClients(string slotKey, string pieceKey) {
        foreach (var slot in cosmeticSlots) {
            if (slot.slotKey == slotKey) {
                foreach (var piece in slot.cosmeticPieces) {
                    if (piece.pieceKey == pieceKey) {
                        piece.cosmeticObject.SetActive(true);
                    }
                    else {
                        piece.cosmeticObject.SetActive(false);
                    }
                    if(hideLocalPlayer && IsOwner) piece.cosmeticObject.SetActive(false);
                }
                return;
            }
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        if (IsOwner) {
            LocalPlayer = this;
            SetupPlayer(FishyManager.Manager.nickname, FishyManager.Manager.color);
        }
    }

    [ServerRpc]
    private void SetupPlayer(string nn, Color c) {
        Nickname = nn;
        Color = c;
    }

    private void Update() {
        foreach (var r in Renderer) r.material.color = Color;
        if (!IsOwner)
            return;
        foreach (var r in Renderer) if(hideLocalPlayer) r.enabled = false;
        SyncTransform(Head, FishyManager.Manager.Head);
        SyncTransform(LeftHand, FishyManager.Manager.LeftHand);
        SyncTransform(RightHand, FishyManager.Manager.RightHand);
    }

    private void SyncTransform(Transform networked, Transform local) {
        networked.SetPositionAndRotation(local.position, local.rotation);
    }
}
