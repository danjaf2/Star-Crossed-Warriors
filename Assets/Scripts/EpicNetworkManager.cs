using UnityEngine;
using Unity.Netcode;

public class EpicNetworkManager : NetworkManager {
    public bool ClientConnected { get => _clientConnected; }
    bool _clientConnected;

    public static EpicNetworkManager Instance;

    private void Awake() {
        Instance = this;
        OnClientConnectedCallback += (o) => { _clientConnected = true; };
    }
}