using UnityEngine;
using System.Collections;
using Photon.Pun;
public class PlayerAnimator : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private	GameObject	attackCollision;
	[SerializeField] Animator animator;
	public AudioSource WalkL;
	public AudioSource WalkR;
	public AudioSource Evade;
	public AudioSource kick;
	public AudioSource Attack1;
	public AudioSource Attack2;
	public AudioSource Attack2_1;
	public AudioSource Respawn;
	[SerializeField] float EvadeDistance = 17.5f;
	[SerializeField] PlayerScript playerscript;
	public PhotonView PV;
	AbilitiesCool coolObj;
	float tmp;
	private void Awake()
    {
		PV = GetComponent<PhotonView>();
		coolObj = GameObject.Find("SkillCool").GetComponent<AbilitiesCool>();
	}
    /*
		public void OnMovement(float horizontal, float vertical)
		{
			animator.SetFloat("horizontal", horizontal);
			animator.SetFloat("vertical", vertical);
		}*/
    [PunRPC]
    public void FrontWalk(bool _flag)
    {
		animator.SetBool("FrontWalk", _flag);
    }
	[PunRPC]
	public void BackWalk(bool _flag)
	{
		animator.SetBool("BackWalk", _flag);
	}
	[PunRPC]
	public void LeftWalk(bool _flag)
	{
		animator.SetBool("LeftWalk", _flag);
	}
	[PunRPC]
	public void RightWalk(bool _flag)
	{
		animator.SetBool("RightWalk", _flag);
	}
	public void OnJump()
	{
		animator.SetTrigger("onJump");
	}

	public void OnKickAttack()
	{
		animator.SetTrigger("onKickAttack");
	}

	public void OnWeaponAttack()
	{
		animator.SetTrigger("onWeaponAttack");
	}

	public void OnSkillAttack()
    {
		animator.SetTrigger("onSkillAttack");
		playerscript.SkillAttackTo();
	}
	public void IsGroundCheckSkillOn() 
    {
		animator.speed = 0.0f;
		StartCoroutine(IsGroundCheckSkillOff());
	}
	IEnumerator IsGroundCheckSkillOff()
	{
		yield return new WaitUntil(() => Physics.Raycast(transform.position, Vector3.down, playerscript.characterController.bounds.extents.y + 3.8f).Equals(true));
		animator.speed = 1.25f;
		playerscript.eSkillActiveEnd = false;
        if (PV.IsMine)
        {
			coolObj.skill2Active = true;
        }
		//coolObj.Ability2();
	}
	public void OnSkillEnd()
    {
		playerscript.PV.RPC("OnSkillEndRPC", RpcTarget.All);
	}
	public void OnAttackCollision()
	{
		playerscript.PV.RPC("OnAttackCollisionRPC", RpcTarget.All);
	}
	
	[PunRPC]
	public void OnEvadeFront()
    {
		animator.SetTrigger("onEvadeFront");
    }
	[PunRPC]
	public void OnEvadeBack()
    {
		animator.SetTrigger("onEvadeBack");
    }
	[PunRPC]
	public void OnEvadeLeft()
    {
		animator.SetTrigger("onEvadeLeft");
    }
	[PunRPC]
	public void OnEvadeRight()
    {
		animator.SetTrigger("onEvadeRight");
    }
	

	public void EvadeStart()
    {
        if (PV.IsMine)
        {
			coolObj.EvadeEnd = false;

        }
		tmp = playerscript.applySpeed;
		playerscript.applySpeed = EvadeDistance;
		Evade.Play();
    }

	public void EvadeEnd()
	{
		playerscript.applySpeed = tmp;
        if (PV.IsMine)
        {
			coolObj.EvadeEnd = true;

        }
	}
	
	public void StunStart()
    {
		playerscript.applySpeed = 0f;
    }

	public void StunEnd()
    {
		playerscript.applySpeed = 5f;
    }

	public void FootL()
    {
		WalkL.Play();
    }

	public void FootR()
    {
		WalkR.Play();
    }
	public void KickSound()
	{
		kick.Play();
	}
	public void Attacknum1()
    {
		Attack1.Play();
    }
	public void Attacknum2()
	{
		Attack2.Play();
	}
	public void Attacknum21()
	{
		Attack2_1.Play();
	}

	public void RespawnSound()
    {
		Respawn.Play();
    }
}

