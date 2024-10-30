using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkManager NM;

    [SerializeField] Text ui_timer;
    int currentMatchTime;
    Coroutine timerCoroutine;

    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject MainCamera;
    public PhotonView PV;
    [SerializeField] GameObject ScoreRangkingObj;
    [SerializeField] GameObject StartCount;
    Text StartTxT;
    private int intTime;
    public float exposeSec; // 노출 시간
    public float Rank1Timer; // 1등 미니맵 노출 텀
    public float Rank2Timer; // 2등 미니맵 노출 텀
    public float Rank3Timer; // 3등 미니맵 노출 텀
    public float Rank4Timer; // 4등 미니맵 노출 텀
    Transform player;
    [SerializeField] int StarCount;
    bool gameStart = false;

    public int matchLength = 300;
    List<Player> playerList;
    List<Player> compareList;
    Dictionary<string, int> PreviousRank = new Dictionary<string, int>();

    public GameObject HP;
    public Image HealthImage;
    public Text hpText;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] Text WinnerUser;

    List<Player> winners;
    public enum EventCodes : byte
    {
        RefreshTimer
    }
   

    private void Awake()
    {
        NM = this;
        Screen.SetResolution(1280, 720, false);
        ///아래 두줄의 값을 높이면 동기화가 더 빨리 된다고 함.
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    //서버접속
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {//닉네임 인풋에 입력한 값을 대입
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //룸 최대 접속 5명 설정
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
        Hashtable props = new Hashtable
            {
                {"ExposeMap", false},
                {"TimeTerm", 0f },
                {"Ranking", 5 },
                {"RankingChangeCheck", false },
                {"RunningCheck", false }
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        Spawn();
        PlayerCountCheck();
        MainCamera.SetActive(false);
        ScoreRangkingObj.SetActive(true);
        HP.SetActive(true);
        ScoreRangkingObj.GetComponent<ScoreRangking>().enabled = true;
    }
    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0, 5f), 5, Random.Range(0, 5f)), Quaternion.identity);
    }

    private void PlayerCountCheck()
    {
        if(Photon.Pun.PhotonNetwork.CurrentRoom.PlayerCount == 5)
        {
            if(gameStart == false)
            {
                PV.RPC("GameStart", RpcTarget.All);
                for (int i = 0; i < playerList.Count; i++)
                {
                    PreviousRank.Add(playerList[i].NickName, 5);

                }
            }

        }
    }
    [PunRPC]
    void GameStart()
    {
        playerList = PhotonNetwork.PlayerList.ToList();
        compareList = playerList;
        StartCount.SetActive(true);
        gameStart = true;
        StartCoroutine(GameTimer());
    }
    void CreateStar()
    {
        for (int i = 0; i < StarCount; i++)
        {
            /* var ranX = Random.Range(0, 67f);
             if (Random.Range(0, 100) <= 50)
             {
                 ranX = ranX * -1f;
             }

             var ranZ = Random.Range(0, 67f);
             if (Random.Range(0, 100) <= 50)
             {
                 ranZ = ranZ * -1f;
             }
             PhotonNetwork.Instantiate("Star", new Vector3(ranX, 3f, ranZ), Quaternion.identity);*/
            
            PhotonNetwork.Instantiate("Star", new Vector3(Random.Range(0, 5f), 5, Random.Range(0, 5f)), Quaternion.identity);
        }
    }
    IEnumerator GameTimer()
    {
        StartTxT = StartCount.GetComponent<Text>();
        int StartCountVal = 3;
        for (int i = StartCountVal; i >= 0; i--)
        {
            if(i == 0)
            {
                StartTxT.text = "START";
                if (PhotonNetwork.IsMasterClient)
                {
                    CreateStar();
                    InitializeTimer();
                }
                yield return new WaitForSeconds(1);
                StartCount.SetActive(false);
            }
            else
            {
                StartTxT.text = i.ToString();

            }
            yield return new WaitForSeconds(1);
        }
    }
    void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        ui_timer.text = $"{minutes}:{seconds}";
    }
    private void InitializeTimer()
    {
        currentMatchTime = matchLength;
        RefreshTimerUI();

        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }
    [PunRPC]
    void ResultPanelOn()
    {
        WinnerUser.text = "";
        ui_timer.text = "00:00";
        List<Player> RankingList = PhotonNetwork.PlayerList.ToList();
        ResultPanel.SetActive(true);
        for (int i = 0; i < RankingList.Count; i++)
        {
            if ((int)RankingList[i].CustomProperties["Ranking"] == 1)
            {
                WinnerUser.text += RankingList[i].NickName + " ";
            }
        }
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        currentMatchTime -= 1;

        if (currentMatchTime <= 0)
        {
            //게임종료
            PV.RPC("ResultPanelOn", RpcTarget.All);
        }
        else
        {
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.RefreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return;

        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        switch (e)
        {
            case EventCodes.RefreshTimer:
                RefreshTimer_R(o);
                break;
        }
    }

    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }

    public List<Player> SortAndChange()
    {
        playerList = PhotonNetwork.PlayerList.ToList();
        playerList.Sort(SortByScores);
        playerList[0].CustomProperties["Ranking"] = 1;
        
        int i = 0;
        for (int j = i + 1; j <= playerList.Count; j++)
        {
            if (j < playerList.Count)
            {
                if (playerList[i].GetScore() == playerList[j].GetScore())
                {
                    playerList[j].CustomProperties["Ranking"] = playerList[i].CustomProperties["Ranking"];
                    i += 1;
                }
                else
                {
                    playerList[j].CustomProperties["Ranking"] = (int)playerList[i].CustomProperties["Ranking"] + 1;
                    i += 1;
                }

            }

        }
        
        for (int v = 0; v < playerList.Count; v++)
        {
            PreviousRank.TryGetValue(playerList[v].NickName, out int rank);
            if (compareList[v].NickName != playerList[v].NickName || rank != (int)playerList[v].CustomProperties["Ranking"])
            {
                playerList[v].CustomProperties["RankingChangeCheck"] = true;
                compareList[v].CustomProperties["RankingChangeCheck"] = true;
            }
        }
        
        for (int a = 0; a < playerList.Count; a++)
        {
            PreviousRank[playerList[a].NickName] = (int)playerList[a].CustomProperties["Ranking"];
        }
        compareList = playerList;

        return playerList;
    }

    public static int SortByScores(Player a, Player b)
    {
        return b.GetScore().CompareTo(a.GetScore());
    }
}
