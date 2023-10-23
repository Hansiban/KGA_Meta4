using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammopack : MonoBehaviour, IItem
{
    public int ammo = 30;

    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();
        if (playerShooter != null && playerShooter.gun != null)
        {
            playerShooter.gun.ammoRemain += ammo;
            //ÃÑ¾Ë¾÷µ«
            UIController.instance.Update_Ammotext(playerShooter.gun.Magammo, playerShooter.gun.ammoRemain);
        }
        Destroy(gameObject);
    }
}
