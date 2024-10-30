using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Player Owner { get; private set; }

    // GameObject[] PlayerListTxt;
    public Rigidbody RB;
    public MeshRenderer SR;
    public PhotonView PV;
    public int getStarCount = 0;

    Vector3 curPos;
    Quaternion curRot;
    private Rigidbody myRigid;
    [SerializeField] public float walkSpeed; // 기본값 : 6.7 테스트할때는 속도를 바꿀 수 있음
    public float applySpeed;

    [SerializeField] float lookSensitivity;
    [SerializeField] Camera theCamera;

    [SerializeField] GameObject vision;
    [SerializeField] GameObject playerCam;

    [SerializeField] GameObject enemyMinimapIcon;

    Vector3 _velocity;

    GameObject[] Players;
    Dictionary<string, int> listDic = new Dictionary<string, int>();

    public CharacterController characterController;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 5.2f;
    private PlayerAnimator playerAnimator;
    bool frontWalk;
    bool backWalk;
    bool leftWalk;
    bool rightWalk;
    public bool isGround = true;
    public bool isSkill; //스킬 실행중 체크
    public bool isEvade;
    public bool eSkillActive;
    public bool eSkillActiveEnd;
    public bool kickSkillActive;
    public bool swordSkillActive;
    private Vector3 moveDirection;
    private Vector3 rotateCharactor;
    [SerializeField] float eSkillSpeed = 2f;
    [SerializeField] float rotateSpeed;  // 캐릭터가 가는방향으로 회전하는 속도
    Vector3 _moveVertical;
    Vector3 _moveHorizontal;
    //히트컨트롤러 변수
    public int Hp = 10;
    private int FullHp;
    public float delaytime = 1.2f;
    //[SerializeField] Text hpText;
    public GameObject attackCol;
    [SerializeField] GameObject enemyHpBar;
    [SerializeField] Image enemyHpFill;
    int myStars = 0;
    AbilitiesCool coolObj;
    void Awake()
    {/*
        Cursor.visible = false;                 // 마우스 커서를 보이지 않게
        Cursor.lockState = CursorLockMode.Locked;	// 마우스 커서 위치 고정*/
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        PV = GetComponent<PhotonView>();
        Owner = PV.Owner;
        applySpeed = walkSpeed;
        myRigid = GetComponent<Rigidbody>();

    }
    private void Start()
    {
        FullHp = Hp;
        if (PV.IsMine)
        {//자신일 경우
            coolObj = GameObject.Find("SkillCool").GetComponent<AbilitiesCool>();
            theCamera.enabled = true;
            Minimap miniMap = FindObjectOfType<Minimap>();
            this.gameObject.layer = 12;
            vision.gameObject.layer = 8;
            playerCam.gameObject.layer = 12;
            enemyMinimapIcon.gameObject.layer = 11;
           // gameObject.tag = "Player";

            gameObject.GetComponent<FogCoverable>().enabled = false;
            enemyMinimapIcon.GetComponent<FogCoverable>().enabled = false;

        }
        else
        {//다른플레이어일 경우
            //enemyHpBar.SetActive(true);
            enemyMinimapIcon.GetComponent<MeshRenderer>().material.color = Color.red;
            vision.gameObject.SetActive(false);
            transform.Find("PlayerCam").gameObject.GetComponent<AudioListener>().enabled = false;
        }

    }
    void Update()
    {
        IsGround();
        // 중력 설정. 플레이어가 땅을 밟고 있지 않다면
        if (!isGround)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * applySpeed * Time.deltaTime);
        if (PV.IsMine)
        {
            Move();
            if (Input.GetMouseButtonDown(0))
            {
                if (!isSkill && isGround && !isEvade)
                {
                    PV.RPC("KickAttack", RpcTarget.All);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!isSkill && isGround && !isEvade && !coolObj.isCooldown1)
                {
                    if (PV.IsMine)
                    {
                        coolObj.skill1Active = true;
                        coolObj.Ability1();

                    }
                    PV.RPC("OnWeaponAttack", RpcTarget.All);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isSkill && isGround && !isEvade && !coolObj.isCooldown2)
                {
                    PV.RPC("OnSkillAttack", RpcTarget.All);
                }
            }
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100)
        {
            transform.position = curPos;
            transform.rotation = curRot;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.smoothDeltaTime * 30);
            transform.rotation = Quaternion.Slerp(transform.rotation, curRot, Time.smoothDeltaTime * 30);
        }
        // CharacterRotation();
    }
    [PunRPC]
    void OnSkillEndRPC()
    {
        isSkill = false;
        isEvade = false;
        kickSkillActive = false;
        swordSkillActive = false;
        eSkillActive = false;
        attackCol.SetActive(false);
    }

    [PunRPC]
    void OnAttackCollisionRPC()
    {
        attackCol.SetActive(true);
    }
    public void TakeDamage(string skill, int EnemyRank)
    {
        if (Hp > 0)
        {
            if (skill.Equals("kick"))
            {   
                if(EnemyRank == 1)
                {//1등은 강력한 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.2f;
                    Hp -= 2;
                }
                else
                {//2 3 4 5등은 일반 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.1f;
                    Hp -= 1;
                }
            }else if (skill.Equals("weapon"))
            {
                if (EnemyRank == 1)
                {//1등은 강력한 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.3f;
                    Hp -= 3;
                }
                else
                {//2 3 4 5등은 일반 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.2f;
                    Hp -= 2;
                }
            }else if (skill.Equals("Eskill"))
            {
                if (EnemyRank == 1)
                {//1등은 강력한 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.4f;
                    Hp -= 4;
                }
                else
                {//2 3 4 5등은 일반 데미지
                    NetworkManager.NM.HealthImage.fillAmount -= 0.3f;
                    Hp -= 3;
                }
            }
            //hpText.text = "내 체력 : " + Hp.ToString();
            if (Hp <= 0)
            {
                Vector3 pos = transform.position;
                PV.RPC("DropStar", RpcTarget.All, pos);
                myStars = 0;
                Owner.SetScore(0);

                PV.RPC("RendereOff", RpcTarget.All);
                var ranX = Random.Range(0f, 67f);
                if (Random.Range(0f, 100f) <= 50f)
                {
                    ranX = ranX * -1f;
                }

                var ranZ = Random.Range(0f, 67f);
                if (Random.Range(0f, 100f) <= 50f)
                {
                    ranZ = ranZ * -1f;
                }
                characterController.enabled = false;
                characterController.transform.position = new Vector3(ranX, 5, ranZ);
                characterController.enabled = true;

                NetworkManager.NM.HealthImage.fillAmount = 1;
                Hp = FullHp;
                //animator.SetTrigger("onDead");
                //Destroy(this.gameObject, delaytime);
            }
            NetworkManager.NM.hpText.text = Hp + " / 10";
        }
        // 피격 애니메이션 재생
        //animator.SetTrigger("onHit");
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
    private IEnumerator RendereOffCor(MeshRenderer item)
    {
        if (!PV.IsMine) { 
            item.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
    #region PUNRPC
    [PunRPC]
    void DropStar(Vector3 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < myStars; i++)
            {
                var ranX = Random.Range(0, 2f);
                if (Random.Range(0, 100) <= 50)
                {
                    ranX = ranX * -1f;
                }

                var ranZ = Random.Range(0, 2f);
                if (Random.Range(0, 100) <= 50)
                {
                    ranZ = ranZ * -1f;
                }
                PhotonNetwork.Instantiate("Star", new Vector3(pos.x + ranX, 2f, pos.y + ranZ), Quaternion.identity);
            }
        }
    }
    [PunRPC]
    void RendereOff()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var item in meshes)
        {
            StartCoroutine(RendereOffCor(item));
        }
    }
    [PunRPC]
    void KickAttack()
    {
        isSkill = true;
        kickSkillActive = true;
        playerAnimator.OnKickAttack();
    }
    [PunRPC]
    void OnWeaponAttack()
    {
        isSkill = true;
        swordSkillActive = true;
        playerAnimator.OnWeaponAttack();
    }
    [PunRPC]
    void OnSkillAttack()
    {
        isSkill = true;
        eSkillActive = true;
        eSkillActiveEnd = true;
        playerAnimator.OnSkillAttack();
    }
    #endregion
    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, characterController.bounds.extents.y + 0.8f);
    }
    public void SkillAttackTo()
    {
        if (isGround)
        {
            moveDirection = transform.up * jumpForce;
            //characterController.Move(moveDirection * applySpeed * Time.deltaTime);
        }
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
    }

    void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (isSkill && !isEvade && !eSkillActiveEnd)
        {
            _moveDirX = 0;
            _moveDirZ = 0;
        }
        _moveHorizontal = transform.right * _moveDirX;
        _moveVertical = transform.forward * _moveDirZ;

        _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //캐릭터 움직임 애니메이션
        if (_moveDirZ > 0)
        {
            frontWalk = true;
        }
        else
        {
            frontWalk = false;
        }

        if (_moveDirZ < 0)
        {
            backWalk = true;
        }
        else
        {
            backWalk = false;
        }

        if (_moveDirX < 0 && _moveDirZ == 0)
        {
            leftWalk = true;
        }
        else
        {
            leftWalk = false;
        }

        if (_moveDirX > 0 && _moveDirZ == 0)
        {
            rightWalk = true;
        }
        else
        {
            rightWalk = false;
        }
        playerAnimator.PV.RPC("FrontWalk", RpcTarget.All, frontWalk);
        playerAnimator.PV.RPC("BackWalk", RpcTarget.All, backWalk);
        playerAnimator.PV.RPC("LeftWalk", RpcTarget.All, leftWalk);
        playerAnimator.PV.RPC("RightWalk", RpcTarget.All, rightWalk);
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isSkill && !isEvade && isGround && !coolObj.skill3Active)
            {
                if (PV.IsMine)
                {
                    coolObj.abilityImage3.fillAmount -= (float)1 / 3;
                    coolObj.isCooldown3 = true;
                }
                isEvade = true;
                if (_moveDirZ < 0)
                {
                    playerAnimator.PV.RPC("OnEvadeBack", RpcTarget.All);
                }
                else if (_moveDirX < 0 && _moveDirZ == 0)
                {
                    playerAnimator.PV.RPC("OnEvadeLeft", RpcTarget.All);                    
                }
                else if (_moveDirX > 0 && _moveDirZ == 0)
                {
                    playerAnimator.PV.RPC("OnEvadeRight", RpcTarget.All);                    
                }
                else
                {
                    playerAnimator.PV.RPC("OnEvadeFront", RpcTarget.All);                    
                }
                
            }
        }
        MoveTo(playerCam.transform.rotation * new Vector3(_moveDirX, 0, _moveDirZ));
        // 회전 설정 (항상 앞만 보도록 캐릭터의 회전은 카메라와 같은 회전 값으로 설정)
        rotateCharactor = moveDirection;
        if (((_moveDirZ > 0 && _moveDirX > 0) || (_moveDirZ > 0 && _moveDirX < 0)) && isGround == true)
        {
            rotateCharactor.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateCharactor.normalized), rotateSpeed * Time.deltaTime);
        }else if (((_moveDirZ < 0 && _moveDirX > 0) || (_moveDirZ < 0 && _moveDirX < 0)) && isGround == true)
        {
            rotateCharactor.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-rotateCharactor.normalized), rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, playerCam.transform.eulerAngles.y, 0), rotateSpeed * Time.deltaTime); ;
        }
    }

/*    void FixedUpdate()
    {
        //characterController.Move(moveDirection * applySpeed * Time.fixedDeltaTime);//용재씨 구현
        //myRigid.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);//설빈 프로토타입용
    }*/
    void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {//위치, 체력 변수 동기화 진행
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Hp);
            stream.SendNext(NetworkManager.NM.HealthImage.fillAmount);
            stream.SendNext(myStars);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            Hp = (int)stream.ReceiveNext();
            enemyHpFill.fillAmount = (float)stream.ReceiveNext();
            myStars = (int)stream.ReceiveNext();
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.transform.CompareTag("Star"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            if (PV.IsMine)
            {
                myStars += 1;
                Owner.SetScore(myStars);
                other.gameObject.GetComponent<StarCheck>().DestroyObj();
            }
            
        }
    }

  
}
