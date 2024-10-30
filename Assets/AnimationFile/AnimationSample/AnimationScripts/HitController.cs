using System.Collections;
using UnityEngine;

public class HitController : MonoBehaviour
{
	private	Animator			animator;
	private	SkinnedMeshRenderer	meshRenderer;
	private	Color				originColor;
	public float EnemyHp = 10f;
	public float delaytime = 1.2f;

	private void Awake()
	{
		animator	 = GetComponent<Animator>();
		meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		originColor	 = meshRenderer.material.color;
	}

	public void TakeDamage(int damage)
	{
		Debug.Log(damage + "의 체력이 감소합니다.");
		if (EnemyHp > 0)
        {
			Debug.Log("EnemyHp > 0 실행");
			EnemyHp -= 2;
			if (EnemyHp == 0)
            {
				animator.SetTrigger("onDead");
				Destroy(this.gameObject, delaytime);
				Debug.Log("사망.");
			}
		}


		// 피격 애니메이션 재생
		animator.SetTrigger("onHit");
		// 색상 변경
		//StartCoroutine("OnHitColor");
	}

	private IEnumerator OnHitColor()
	{
		// 색을 빨간색으로 변경한 후 0.1초 후에 원래 색상으로 변경
		//meshRenderer.material.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		//meshRenderer.material.color = originColor;
	}
}

