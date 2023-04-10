using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Collections.LowLevel.Unsafe;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button demomanBtn;
    [SerializeField] private Button heavyBtn;
    [SerializeField] private Button scoutBtn;
    [SerializeField] public GameObject characterSelectPanel;
    [SerializeField] public TextMeshProUGUI typeSelectedText; 

    public ClassType type = ClassType.SCOUT; 
    private void Awake()
    {

        serverBtn.onClick.AddListener(()=>     {
            characterSelectPanel.SetActive(false);  
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() => {
            characterSelectPanel.SetActive(false);
            NetworkManager.Singleton.StartHost();

        });
        clientBtn.onClick.AddListener(() => {
            characterSelectPanel.SetActive(false);
            NetworkManager.Singleton.StartClient();

        });

        demomanBtn.onClick.AddListener(() => {
            type = ClassType.DEMO;
            typeSelectedText.text = "DEMOMAN";

        });

        heavyBtn.onClick.AddListener(() => {
            type = ClassType.HEAVY;
            typeSelectedText.text = "HEAVY";

        });

        scoutBtn.onClick.AddListener(() => {
            type = ClassType.SCOUT;
            typeSelectedText.text = "SCOUT";

        });


    }
}
