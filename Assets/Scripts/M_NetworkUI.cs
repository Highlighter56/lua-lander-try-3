using UnityEngine;
using Unity.Netcode; // Required for Netcode!

public class NetworkUI : MonoBehaviour
{
    public void ClickStartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void ClickStartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}