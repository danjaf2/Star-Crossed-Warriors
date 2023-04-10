using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI playerStats; 
    public TextMeshProUGUI specialAbilityStats;
    public TextMeshProUGUI primaryFireStats;

    public PlayerManager player;



    void Start()
    {
        player = GetComponentInParent<PlayerManager>(); 
    }

    
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        //Add relevant calls here

        playerStats.text = "SHIP STATUS - " + player.playerClass.GetType()+" \r\n HEALTH: "+ player.playerClass.Health +"\r\n ENERGY: " + player.playerClass.Energy; 

        if(player.playerClass.isStunned)
        {
            playerStats.text += "\r\n STUNNED";
        }
       
        primaryFireStats.text = "PRIMARY FIRE: " + ((player.playerClass.GetPrimaryFireStatus() <= 0)? "READY" : "COOLDOWN");

        specialAbilityStats.text = "SPECIAL ABILITY: " + ((player.playerClass.GetSpecialFireStatus() <= 0) ? "READY" : "COOLDOWN");
        
    }
}
