using Networking;
using UnityEngine;

namespace UI
{
    public class NetworkGameUIManager : MonoBehaviour
    {
        public CustomNetworkManager customNetworkManager;
        
        public void ConnectToServer()
        {
            customNetworkManager.networkAddress = "83.69.28.185";
            customNetworkManager.StartClient();
        }
    }
}