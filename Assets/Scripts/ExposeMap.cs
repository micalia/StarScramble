using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
public class ExposeMap : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView PV;
    
    List<Player> playerList;
    FogCoverable fogCoverable;
    float rank1Timer;
    float rank2Timer;
    float rank3Timer;
    float rank4Timer;
    private void Start()
    {
        if(gameObject.GetComponent<FogCoverable>().enabled == true)
        {
            fogCoverable = gameObject.GetComponent<FogCoverable>();
        }
        rank1Timer = NetworkManager.NM.Rank1Timer;
        rank2Timer = NetworkManager.NM.Rank2Timer;
        rank3Timer = NetworkManager.NM.Rank3Timer;
        rank4Timer = NetworkManager.NM.Rank4Timer;

    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        playerList = NetworkManager.NM.SortAndChange();
        PlayerScript myObj = gameObject.transform.parent.GetComponent<PlayerScript>();
        if (myObj.Owner.CustomProperties["Ranking"].Equals(1) || myObj.Owner.CustomProperties["Ranking"].Equals(2))
        {
            myObj.applySpeed = myObj.walkSpeed + (myObj.walkSpeed * 0.15f);
        }
        else
        {
            myObj.applySpeed = myObj.walkSpeed;
        }

        if (gameObject.name.Equals("MinimapIcon"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                        if (myObj.Owner.NickName == playerList[i].NickName)
                        {
                            if (playerList[i].CustomProperties["Ranking"].Equals(1))
                            {
                                //myObj.applySpeed = myObj.walkSpeed + (myObj.walkSpeed * 0.1f);
                                if ((bool)playerList[i].CustomProperties["RankingChangeCheck"] == true)
                                {
                                    StopAllCoroutines();
                                    playerList[i].CustomProperties["RunningCheck"] = false;
                                    playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                }
                                playerList[i].CustomProperties["TimeTerm"] = rank1Timer;
                                print(playerList[i].NickName +"1등 실행, 시간 텀 : " + playerList[i].CustomProperties["TimeTerm"]);
                                if (playerList[i].CustomProperties["RunningCheck"].Equals(false))   //코루틴이 실행중이 아니라면 실행
                                {//실행중이 아니라면 시작, 기존 실행중인 코루틴이 있다면 패스
                                    print(playerList[i].NickName + "가 랭킹 체인지되고, 코루틴 1실행");
                                    playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                    StartCoroutine(TermExpose(rank1Timer, playerList[i]));
                                }
                            }
                            else if (playerList[i].CustomProperties["Ranking"].Equals(2))
                            {
                                //myObj.applySpeed = myObj.walkSpeed + (myObj.walkSpeed * 0.1f);
                                if ((bool)playerList[i].CustomProperties["RankingChangeCheck"] == true)
                                {
                                    StopAllCoroutines();
                                    playerList[i].CustomProperties["RunningCheck"] = false;
                                    playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                }
                                playerList[i].CustomProperties["TimeTerm"] = rank2Timer;
                                print(playerList[i].NickName + "2등 실행");
                                if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                                {
                                    print(playerList[i].NickName + "가 랭킹 체인지되고, 코루틴 2실행");
                                    playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                    StartCoroutine(TermExpose(rank2Timer, playerList[i]));
                                }
                            }
                            else if (playerList[i].CustomProperties["Ranking"].Equals(3))
                            {
                                myObj.applySpeed = myObj.walkSpeed;
                                if ((bool)playerList[i].CustomProperties["RankingChangeCheck"] == true)
                                {
                                    StopAllCoroutines();
                                    playerList[i].CustomProperties["RunningCheck"] = false;
                                    playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                }
                                playerList[i].CustomProperties["TimeTerm"] = rank3Timer;
                                print(playerList[i].NickName + "3등 실행");
                                if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                                {
                                    print(playerList[i].NickName + "가 랭킹 체인지되고, 코루틴 3실행");
                                    playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                    StartCoroutine(TermExpose(rank3Timer, playerList[i]));
                                }
                            }
                            else if (playerList[i].CustomProperties["Ranking"].Equals(4))
                            {
                                myObj.applySpeed = myObj.walkSpeed;
                                if ((bool)playerList[i].CustomProperties["RankingChangeCheck"] == true)
                                {
                                    StopAllCoroutines();
                                    playerList[i].CustomProperties["RunningCheck"] = false;
                                    playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                }
                                playerList[i].CustomProperties["TimeTerm"] = rank4Timer;
                                print(playerList[i].NickName + "4등 실행");
                                if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                                {
                                    print(playerList[i].NickName + "가 랭킹 체인지되고, 코루틴 4실행");
                                    playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                    StartCoroutine(TermExpose(rank4Timer, playerList[i]));
                                }
                            }
                            else if (playerList[i].CustomProperties["Ranking"].Equals(5))
                            {
                                myObj.applySpeed = myObj.walkSpeed;
                                if ((bool)playerList[i].CustomProperties["RankingChangeCheck"] == true)
                                {
                                    StopAllCoroutines();
                                    playerList[i].CustomProperties["RunningCheck"] = false;
                                    playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                }
                                playerList[i].CustomProperties["TimeTerm"] = 0f;
                                print(playerList[i].NickName + "5등 실행");
                                StopAllCoroutines();
                            }
                    }
                }
            }
        }
    }

    IEnumerator TermExpose(float termTime, Player player)
    {        
        print("플레이어 닉네임 : " + player.NickName + ", 랭킹 : " + player.CustomProperties["Ranking"] + ", 노출 시간 텀 : " + termTime);        
        yield return new WaitForSeconds(termTime);
        StartCoroutine(ExposeTime(termTime, player));
    }
    IEnumerator ExposeTime(float termTime, Player player)
    {
        player.CustomProperties["ExposeMap"] = true;
        PV.RPC("ExposeTrue", RpcTarget.All);
        float _exposeTimer = NetworkManager.NM.exposeSec;
        yield return new WaitForSeconds(_exposeTimer);
        player.CustomProperties["ExposeMap"] = false;
        PV.RPC("ExposeFalse", RpcTarget.All);
        StartCoroutine(TermExpose(termTime, player));
    }
    [PunRPC]
    void ExposeTrue()
    {
        if (gameObject.GetComponent<FogCoverable>().enabled == true)
        {
            fogCoverable.ExposeTrue();
        }
    }
    [PunRPC]
    void ExposeFalse()
    {
        if (gameObject.GetComponent<FogCoverable>().enabled == true)
        {
            fogCoverable.ExposeFalse();
        }
    }
}