using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerAttackCollision : MonoBehaviour
{
	[SerializeField] PhotonView PV;
	[SerializeField] PlayerScript playerscript;
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		if (!PV.IsMine && other.GetComponent<PhotonView>().IsMine)
		{
			if (playerscript.kickSkillActive.Equals(true))
			{

				if (other.GetComponent<PlayerScript>().Hp > 0)
				{
					if((int)playerscript.Owner.CustomProperties["Ranking"] == 1)
                    {
						other.GetComponent<PlayerScript>().TakeDamage("kick", 1);
                    }
                    else
                    {
						other.GetComponent<PlayerScript>().TakeDamage("kick", 0);
					}
				}

			}else if (playerscript.swordSkillActive.Equals(true))
			{
				if (other.GetComponent<PlayerScript>().Hp > 0)
				{
					if ((int)playerscript.Owner.CustomProperties["Ranking"] == 1)
					{
						other.GetComponent<PlayerScript>().TakeDamage("weapon", 1);
					}
					else
					{
						other.GetComponent<PlayerScript>().TakeDamage("weapon", 0);
					}
				}

			}else if (playerscript.eSkillActive.Equals(true))
			{
				if (other.GetComponent<PlayerScript>().Hp > 0)
				{
					if ((int)playerscript.Owner.CustomProperties["Ranking"] == 1)
					{
						other.GetComponent<PlayerScript>().TakeDamage("Eskill", 1);
					}
					else
					{
						other.GetComponent<PlayerScript>().TakeDamage("Eskill", 0);
					}
				}

			}
		}

	}
	
}

