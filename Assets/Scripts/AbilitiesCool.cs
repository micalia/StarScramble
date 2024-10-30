using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesCool : MonoBehaviour
{
    public Image abilityImage1;
    public float cooldown1 = 2f;
    public bool isCooldown1 = false;
    public bool skill1Active = false;
    
    public Image abilityImage2;
    public float cooldown2 = 3f;
    public bool isCooldown2 = false;
    public bool skill2Active = false;

    public Image abilityImage3; //dash
    public float cooldown3 = 9f;
    public bool isCooldown3 = false;
    public bool skill3Active = false;
    public bool EvadeEnd = false;
    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 1;
    }

    void Update()
    {       
        Ability1();        
        Ability2();
        Dash();
    }

    public void Ability1()
    {
        if(skill1Active && isCooldown1 == false)
        {
            isCooldown1 = true;
            abilityImage1.fillAmount = 1;
        }

        if (isCooldown1)
        {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown1 = false;
                skill1Active = false;
            }
        }
    }

    public void Ability2()
    {
        if (skill2Active && isCooldown2 == false)
        {
            isCooldown2 = true;
            abilityImage2.fillAmount = 1;
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
                skill2Active = false;
            }
        }
    }

    public void Dash()
    {
        if (isCooldown3)
        {
            if (abilityImage3.fillAmount <= (float)1 / 3)
            {
                skill3Active = true;
                //abilityImage3.fillAmount = 0;
                //isCooldown3 = false;
            }
            else
            {
                skill3Active = false;
            }

            if (EvadeEnd)
            {
                abilityImage3.fillAmount += 1 / cooldown3 * Time.deltaTime;
            }
            if(abilityImage3.fillAmount >= 1)
            {
                isCooldown3 = false;
            }
            //계속 풀로 올림
        }
    }

}
