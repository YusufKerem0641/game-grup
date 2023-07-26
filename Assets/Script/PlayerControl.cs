using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    void playerDamage(float damage)
    {
        print("hasar");
        playerData.Hp -= damage;
        if (playerData.Hp <= 0)
        {
            dead();
        }
    }

    internal void playerFight(GameObject gameObject)
    {
        gameObject.GetComponent<PlayerControl>().playerDamage(playerData.Damage);
    }

    void dead()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
