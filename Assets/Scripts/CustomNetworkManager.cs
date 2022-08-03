using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        var nextPosition = NetworkServer.connections.Count != 0 ? startPositions[1].position : startPositions[0].position;
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        player.GetComponent<NetworkPlayerController>().startSpawnPosition = nextPosition;
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
