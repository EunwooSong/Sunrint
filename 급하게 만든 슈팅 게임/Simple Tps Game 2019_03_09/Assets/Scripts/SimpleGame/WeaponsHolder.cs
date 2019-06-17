using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHolder : MonoBehaviour {

    public GunCtrl currentWeapon;
    public GameObject[] weapons;

	// Use this for initialization
	void Start () {
        foreach (GameObject weapon in weapons)
            weapon.SetActive(false);

        weapons[0].SetActive(true);
        currentWeapon = weapons[0].GetComponent<GunCtrl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[0].SetActive(true);
            currentWeapon = weapons[0].GetComponent<GunCtrl>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[1].SetActive(true);
            currentWeapon = weapons[1].GetComponent<GunCtrl>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[2].SetActive(true);
            currentWeapon = weapons[2].GetComponent<GunCtrl>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[3].SetActive(true);
            currentWeapon = weapons[3].GetComponent<GunCtrl>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[4].SetActive(true);
            currentWeapon = weapons[4].GetComponent<GunCtrl>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            foreach (GameObject weapon in weapons)
                weapon.SetActive(false);

            weapons[5].SetActive(true);
            currentWeapon = weapons[5].GetComponent<GunCtrl>();
        }
    }
}
