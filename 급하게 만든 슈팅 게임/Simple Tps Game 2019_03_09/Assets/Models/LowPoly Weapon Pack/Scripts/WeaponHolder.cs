using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject[] weapons;
    private int index;

    void Start()
    {
        weapons = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i).gameObject;
        }

        foreach (GameObject go in weapons)
            go.SetActive(false);

        if (weapons[0])
            weapons[0].SetActive(true);
    }

    public void ToogleLeft()
    {
        weapons[index].SetActive(false);

        index--;
        if (index < 0)
            index = weapons.Length - 1;

        weapons[index].SetActive(true);
    }

    public void ToogleRight()
    {
        weapons[index].SetActive(false);

        index++;
        if (index == weapons.Length)
            index = 0;

        weapons[index].SetActive(true);
    }
}
