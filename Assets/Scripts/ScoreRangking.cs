using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreRangking : MonoBehaviourPunCallbacks
{
    public GameObject ScoreRankingEntryPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY
    public void Start()
    {
        playerListEntries = new Dictionary<int, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(ScoreRankingEntryPrefab);

            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().text = string.Format("{0} : {1}", p.NickName, p.GetScore());

            playerListEntries.Add(p.ActorNumber, entry);
        }
    }
    #endregion

    #region PUN CALLBACKS

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            entry.GetComponent<Text>().text = string.Format("{0} : {1}", targetPlayer.NickName, targetPlayer.GetScore());
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
            GameObject entry = Instantiate(ScoreRankingEntryPrefab);

            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().text = string.Format("{0} : {1}", newPlayer.NickName, newPlayer.GetScore());

            playerListEntries.Add(newPlayer.ActorNumber, entry);
        
    }

    #endregion
}
