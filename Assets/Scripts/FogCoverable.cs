using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
public class FogCoverable : MonoBehaviourPunCallbacks
{
    public Renderer renderer;
    [SerializeField] SkinnedMeshRenderer[] skinRenderer;
    [SerializeField] MeshRenderer swordMesh;
    [SerializeField] GameObject healthCanvas;
    public bool exposeMap;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        FieldOfView.OnTargetsVisibilityChange += FieldOfViewOnTargetsVisibilityChange;
    }

    void OnDestroy()
    {
        FieldOfView.OnTargetsVisibilityChange -= FieldOfViewOnTargetsVisibilityChange;
    }
    #region 주석
    /*public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        playerList = NetworkManager.NM.SortAndChange();
        if (gameObject.name.Equals("MinimapIcon"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
    print("함수를 실행하는 유저 닉네임 : " + PhotonNetwork.LocalPlayer.NickName);
                print("게임오브젝트 이름 : " + gameObject.name);
                print("player리스트 카운트 : " + playerList.Count);*//*
                for (int i = 0; i < playerList.Count; i++)
                {
                    //print(i + "번째 유저 닉네임 : " + playerList[i].NickName);
                    if (gameObject.transform.parent.GetComponent<PlayerScript>().Owner.NickName == playerList[i].NickName)
                    {
                        if (playerList[i].CustomProperties["Ranking"].Equals(1))
                        {
                            print("-------------------------");
                            print("1등 실행");
                            print(playerList[i].CustomProperties["RankingChangeCheck"] + " : RankingChangeCheck");
                            print(playerList[i].CustomProperties["RunningCheck"] + " : RunningCheck");
                            if (playerList[i].CustomProperties["RankingChangeCheck"].Equals(true))
                            {//랭킹이 바뀌었으면 실행중인 모든 코루틴 멈추고 새로시작
                                StopAllCoroutines();
                                playerList[i].CustomProperties["RankingChangeCheck"] = false; //랭킹 바뀐거 확인했으니까, 다시 false
                                playerList[i].CustomProperties["RunningCheck"] = false;   //기존 코루틴을 모두 멈춘 상태이므로 Running은 false여야함.
                            }
                            if (playerList[i].CustomProperties["RunningCheck"].Equals(false))   //코루틴이 실행중이 아니라면 실행
                            {//실행중이 아니라면 시작, 기존 실행중인 코루틴이 있다면 패스
                                print("코루틴 1실행");
                                playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                StartCoroutine(TermExpose((float)playerList[i].CustomProperties["TimeTerm"], playerList[i]));
                            }
                        }
                        else if (playerList[i].CustomProperties["Ranking"].Equals(2))
                        {
                            print("-------------------------");
                            print("2등 실행");
                            print(playerList[i].CustomProperties["RankingChangeCheck"] + " : RankingChangeCheck");
                            print(playerList[i].CustomProperties["RunningCheck"] + " : RunningCheck");
                            if (playerList[i].CustomProperties["RankingChangeCheck"].Equals(true))
                            {
                                StopAllCoroutines();
                                playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                playerList[i].CustomProperties["RunningCheck"] = false;
                            }
                            if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                            {
                                print("코루틴 2실행");
                                playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                StartCoroutine(TermExpose((float)playerList[i].CustomProperties["TimeTerm"], playerList[i]));
                            }
                        }
                        else if (playerList[i].CustomProperties["Ranking"].Equals(3))
                        {
                            print("-------------------------");
                            print("3등 실행");
                            print(playerList[i].CustomProperties["RankingChangeCheck"] + " : RankingChangeCheck");
                            print(playerList[i].CustomProperties["RunningCheck"] + " : RunningCheck");
                            if (playerList[i].CustomProperties["RankingChangeCheck"].Equals(true))
                            {
                                StopAllCoroutines();
                                playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                playerList[i].CustomProperties["RunningCheck"] = false;
                            }
                            if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                            {
                                print("코루틴 3실행");
                                playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                StartCoroutine(TermExpose((float)playerList[i].CustomProperties["TimeTerm"], playerList[i]));
                            }
                        }
                        else if (playerList[i].CustomProperties["Ranking"].Equals(4))
                        {
                            if (playerList[i].CustomProperties["RankingChangeCheck"].Equals(true))
                            {
                                StopAllCoroutines();
                                playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                playerList[i].CustomProperties["RunningCheck"] = false;
                            }
                            if (playerList[i].CustomProperties["RunningCheck"].Equals(false))
                            {
                                print("코루틴 4실행");
                                playerList[i].CustomProperties["RunningCheck"] = true;  //코루틴 실행 체크
                                StartCoroutine(TermExpose((float)playerList[i].CustomProperties["TimeTerm"], playerList[i]));
                            }
                        }
                        else if (playerList[i].CustomProperties["Ranking"].Equals(5))
                        {
                            if (playerList[i].CustomProperties["RankingChangeCheck"].Equals(true))
                            {
                                StopAllCoroutines();
                                playerList[i].CustomProperties["RankingChangeCheck"] = false;
                                playerList[i].CustomProperties["RunningCheck"] = false;
                            }
                            print("코루틴 5실행");
                        }
                        //break;
                    }
                }
            }*/
    /*if (gameObject.transform.parent.GetComponent<PlayerScript>().Owner.NickName == targetPlayer.NickName)
    {
        print("owner Nick" + gameObject.transform.parent.GetComponent<PlayerScript>().Owner.NickName);
        print("target Nick" + targetPlayer.NickName);
        *//* object exposeVal;
         if(targetPlayer.CustomProperties.TryGetValue("ExposeMap", out exposeVal) == true)
         {*//*
        print(targetPlayer.CustomProperties["Ranking"]);
            if (targetPlayer.CustomProperties["Ranking"].Equals(1))
            {
                if (targetPlayer.CustomProperties["RankingChangeCheck"].Equals(true))
                {
                    StopAllCoroutines();
                    targetPlayer.CustomProperties["RankingChangeCheck"] = false;
                    targetPlayer.CustomProperties["RunningCheck"] = false;
                }
                if(targetPlayer.CustomProperties["RunningCheck"].Equals(false))
                {
                    print("코루틴 1실행");
                    StartCoroutine(TermExpose((float)targetPlayer.CustomProperties["TimeTerm"], targetPlayer));
                }
            }else if (targetPlayer.CustomProperties["Ranking"].Equals(2))
            {
                if (targetPlayer.CustomProperties["RankingChangeCheck"].Equals(true))
                {
                    StopAllCoroutines();
                    targetPlayer.CustomProperties["RankingChangeCheck"] = false;
                    targetPlayer.CustomProperties["RunningCheck"] = false;
                }
                if (targetPlayer.CustomProperties["RunningCheck"].Equals(false))
                {
                    print("코루틴 2실행");
                    StartCoroutine(TermExpose((float)targetPlayer.CustomProperties["TimeTerm"], targetPlayer));
                }
            }
            else if (targetPlayer.CustomProperties["Ranking"].Equals(3))
            {
                if (targetPlayer.CustomProperties["RankingChangeCheck"].Equals(true))
                {
                    StopAllCoroutines();
                    targetPlayer.CustomProperties["RankingChangeCheck"] = false;
                    targetPlayer.CustomProperties["RunningCheck"] = false;
                }
                if (targetPlayer.CustomProperties["RunningCheck"].Equals(false))
                {
                    print("코루틴 3실행");
                    StartCoroutine(TermExpose((float)targetPlayer.CustomProperties["TimeTerm"], targetPlayer));
                }
            }
            else if (targetPlayer.CustomProperties["Ranking"].Equals(4))
            {
                if (targetPlayer.CustomProperties["RankingChangeCheck"].Equals(true))
                {
                    StopAllCoroutines();
                    targetPlayer.CustomProperties["RankingChangeCheck"] = false;
                    targetPlayer.CustomProperties["RunningCheck"] = false;
                }
                if (targetPlayer.CustomProperties["RunningCheck"].Equals(false))
                {
                    print("코루틴 4실행");
                    StartCoroutine(TermExpose((float)targetPlayer.CustomProperties["TimeTerm"], targetPlayer));
                }
            }
            else if (targetPlayer.CustomProperties["Ranking"].Equals(5))
            {
                if (targetPlayer.CustomProperties["RankingChangeCheck"].Equals(true))
                {
                    StopAllCoroutines();
                    targetPlayer.CustomProperties["RankingChangeCheck"] = false;
                    targetPlayer.CustomProperties["RunningCheck"] = false;
                }
                    print("코루틴 5실행");
            }
        //}
    }
}*/


    /*
        IEnumerator TermExpose(float termTime, Player player)
        {
            print("플레이어 닉네임 : " + player.NickName);
            print("노출 시간 텀 : " + termTime);
            yield return new WaitForSeconds(termTime);
            StartCoroutine(ExposeTime(termTime, player));
        }
        IEnumerator ExposeTime(float termTime, Player player)
        {
            player.CustomProperties["ExposeMap"] = true;
            print(player.NickName + "의 exposeMap bool 값: " + (bool)player.CustomProperties["ExposeMap"]);
            exposeMap = (bool)player.CustomProperties["ExposeMap"];
            PV.RPC("ExposeTrue", RpcTarget.All, exposeMap);
            float _exposeTimer = NetworkManager.NM.exposeSec;
            yield return new WaitForSeconds(_exposeTimer);
            exposeMap = false;
            player.CustomProperties["ExposeMap"] = exposeMap;
            PV.RPC("ExposeFalse", RpcTarget.All, exposeMap);
            print(player.NickName + "의 exposeMap bool 값: " + (bool)player.CustomProperties["ExposeMap"]);
            //exposeMap = false;
            //targetPlayer.CustomProperties["ExposeMap"] = false;
            StartCoroutine(TermExpose(termTime, player));
        }
        [PunRPC]
        void ExposeTrue(bool exposeMapVal)
        {
            exposeMap = true;
        }
        [PunRPC]
        void ExposeFalse(bool exposeMapVal)
        {
            exposeMap = exposeMapVal;
        }*/
    #endregion
    void FieldOfViewOnTargetsVisibilityChange(List<Transform> newTargets)
    {
        renderer.enabled = newTargets.Contains(transform);
        
        if (exposeMap)//등수에 따른 미니맵 노출
        {
            if (gameObject.name.Equals("MinimapIcon"))
            {
                renderer.enabled = true;
            }
        }

        if (gameObject.name.Equals("Player(Clone)"))
        {
            if (newTargets.Contains(transform) == false)
            {
                healthCanvas.SetActive(false);
                for (int i = 0; i < skinRenderer.Length; i++)
                {
                    skinRenderer[i].enabled = false;
                    swordMesh.enabled = false;
                }
            }
            else
            {
                healthCanvas.SetActive(true);
                for (int i = 0; i < skinRenderer.Length; i++)
                {
                    skinRenderer[i].enabled = true;
                    swordMesh.enabled = true;
                }
            }

        }
    }
    public void ExposeTrue()
    {
        exposeMap = true;
    }
    public void ExposeFalse()
    {
        exposeMap = false;
    }
}