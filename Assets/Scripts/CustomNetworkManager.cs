using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public bool hasPlayer;
    public bool isOnlyServer;

    public override void Awake()
    {
        base.Awake();
        if (isOnlyServer)
        {
            StartServer();
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        var nextPosition = hasPlayer ? startPositions[1].position : startPositions[0].position;
        if (!hasPlayer) hasPlayer = true;
        var player = Instantiate(playerPrefab, nextPosition, Quaternion.identity);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
