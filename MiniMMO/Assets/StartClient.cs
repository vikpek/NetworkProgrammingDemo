using Unity.Netcode;
using UnityEngine;

public class StartClient : MonoBehaviour
{
    public void Start()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.StartClient();
    }
}