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
        player = gameObject.GetComponentInParent<PlayerManager>(); 
    }

    
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        float health;
        float energy=10000;
        //Add relevant calls here
        if (player.playerClass.playerControlled)
        {
            energy = player.playerClass._energy.Value;
            //print(energy);
        }
        playerStats.text = "SHIP STATUS - " + player.playerClass.GetType()+" \r\n HEALTH: "+ player.playerClass._health.Value.ToString("F0") +"\r\n ENERGY: " + energy; 

        if(player.playerClass.isStunned)
        {
            playerStats.text += "\r\n STUNNED";
        }
       
        primaryFireStats.text = "PRIMARY FIRE: " + ((player.playerClass.GetPrimaryFireStatus() <= 0)? "READY" : "COOLDOWN");

        specialAbilityStats.text = "SPECIAL ABILITY: " + ((player.playerClass.GetSpecialFireStatus() <= 0) ? "READY" : "COOLDOWN");
        
    }
}
