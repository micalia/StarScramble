using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    GameObject[] playerChk;
    Camera playerCam;
    void OnEnable()
    {

        playerChk = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerChk.Length; i++)
        {
            if (playerChk[i].layer == 12)
            {//플레이어 레이어층이라면
                playerCam = playerChk[i].GetComponentInChildren<Camera>();
            }
        }
    }

    void LateUpdate()
    {
        if(playerCam != null)
        {
            transform.LookAt(playerCam.transform);
            transform.Rotate(0, 180, 0);

        }
    }
}
