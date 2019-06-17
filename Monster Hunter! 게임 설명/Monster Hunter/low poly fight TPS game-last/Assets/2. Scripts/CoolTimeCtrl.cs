using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoolTimeCtrl : MonoBehaviour
{

    public float coolTime = 15.0f;
    public Image skillButton;

    void Update()
    {
        if (skillButton.fillAmount != 0f)
        {
            skillButton.fillAmount -= Time.deltaTime / coolTime;
        }
    }

    public void UseSkill()
    {
        if (skillButton.fillAmount == 0)
        {
            skillButton.fillAmount = 1.0f;
        }
    }
}

