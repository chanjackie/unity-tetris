using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class MPPlayer : NetworkBehaviour
{
    public Board playerBoard;
    public Tilemap playerTilemap;
    public override void OnNetworkSpawn()
    {
        if (IsOwner) {
            SendTilemap();
        }
    }

    public void SendTilemap() {
        if (NetworkManager.Singleton.IsServer) {
            playerTilemap = playerBoard.tilemap;
        } else {
            SubmitPositionRequestServerRpc();
        }
    }
    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
        playerTilemap = playerBoard.tilemap;
    }

    void Update() {
        SendTilemap();
    }

}
