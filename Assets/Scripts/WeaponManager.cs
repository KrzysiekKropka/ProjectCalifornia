using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    int[] weaponDamage = new int[5];
    int[] maxAmmo = new int[5];
    string[] weaponName = new string[5];
    void Start()
    {
        weaponName[0] = "Pistol";
        weaponName[1] = "Deagle";
        weaponName[2] = "MP5";
        weaponName[3] = "Shotgun";
        weaponName[4] = "AK-47";

        weaponDelay[0] = 0.375f;
        weaponDelay[1] = 0.75f;
        weaponDelay[2] = 0.1f;
        weaponDelay[3] = 1f;
        weaponDelay[4] = 0.733f;

        reloadTime[0] = 2f;
        reloadTime[1] = 2f;
        reloadTime[2] = 3f;
        reloadTime[3] = 4f;
        reloadTime[4] = 3f;

        weaponDamage[0] = 10;
        weaponDamage[1] = 30;
        weaponDamage[2] = 10;
        weaponDamage[3] = 20;
        weaponDamage[4] = 20;

        maxAmmo[0] = 17;
        maxAmmo[1] = 7;
        maxAmmo[2] = 50;
        maxAmmo[3] = 5;
        maxAmmo[4] = 30;
    }
}
