using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats
{
    public float maxHealth;
    public float maxEnergy;
    public float speed;
    public float primaryFireRate;
    public float lockOnRate;
    public GameObject specialUtility; 
}

public class Scout: PlayerStats
{
    public Scout(GameObject abilityPrefab)
    {
        maxHealth = 150;
        maxEnergy = 300;
        speed = 2;
        primaryFireRate = 7;
        lockOnRate = 100;
        specialUtility = abilityPrefab;
}
}

public class Demoman : PlayerStats
{
    public Demoman(GameObject abilityPrefab)
    {
        maxHealth = 225;
        maxEnergy = 300;
        speed = 1.0f;
        primaryFireRate = 5;
        lockOnRate = 100;
        specialUtility = abilityPrefab;
    }
}

public class Heavy : PlayerStats
{
    public Heavy(GameObject abilityPrefab)
    {
        maxHealth = 350;
        maxEnergy = 400;
        speed = 0.5f;
        primaryFireRate = 5;
        lockOnRate = 100;
        specialUtility = abilityPrefab;
    }
}

public enum ClassType { SCOUT, DEMO, HEAVY, NONE}; 
public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject vehicle;
    public float currentEnergyPercentage = 100;

    public ClassType type;
    public PlayerStats playerClass;

    public Dictionary<ClassType, GameObject> specialAbilityPrefabs;

    void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Plane");

        if(type == ClassType.DEMO)
        {
            playerClass = new Demoman(specialAbilityPrefabs[type]); 
        }
        else if (type == ClassType.SCOUT)
        {
            playerClass = new Scout(specialAbilityPrefabs[type]);
        }
        else if (type == ClassType.HEAVY)
        {
            playerClass = new Heavy(specialAbilityPrefabs[type]);
        }
        else
        {
            playerClass = null; 
        }
    }

    public void AdjustVehicleBasedOnType()
    {
        //Add code for speed adjustment here
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = vehicle.transform.position;
        //transform.rotation = vehicle.transform.rotation;
    }

    public void Recharge(float amount)
    {
        currentEnergyPercentage += amount; 

        if(currentEnergyPercentage > 100)
        {
            currentEnergyPercentage = 100;
        }
    }

    public void Decharge(float amount)
    {
        currentEnergyPercentage -= amount;

        if (currentEnergyPercentage < 0)
        {
            currentEnergyPercentage = 0;
        }
    }
}
