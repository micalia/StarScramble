using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

using Hashtable = ExitGames.Client.Photon.Hashtable;
public class StarCheck : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    SphereCollider SC;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SC = GetComponent<SphereCollider>();
        StartCoroutine(EnableSphere());
    }
    IEnumerator EnableSphere()
    {
        yield return new WaitForSeconds(0.3f);
        SC.enabled = true;
        yield return new WaitForSeconds(0.7f);
        SC.enabled = false;
    }

    //지형물 안에 별이 생성됐을때 다시 생성하는 처리 구현해야함


    public void DestroyObj()
    {
        PV.RPC("DestroyObjRpc", RpcTarget.All);
    }
    [PunRPC]
    void DestroyObjRpc()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


}
