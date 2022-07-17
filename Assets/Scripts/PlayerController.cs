using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Manger")]
    public GameManager gameManger;
    public SoundManager soundMagager;

    [Header("Camera")]
    public Camera Mycamera;

    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;
    public bool jumping;

    [Header("Camera Rotate")]
    public float lookSensitivity; //카메라 민감도 
    public float cameraRotationLimit; //각도 제한 

    [Header("Weapon Attack")]
    public float delay;
    public float AttackStr;
    public int Maxcount;
    public int nowcount;
    public int casingcount;
    public int bulletSpeed;
    public Transform bulletPos;
    public GameObject bulletObject;
    public Transform casingPos;
    public GameObject casingObject;

    public AudioSource audiosource;
    private Rigidbody rigid;
    private float currentCameraRotationX = 0f;

    private bool fire;
    private bool jump;
    private bool load;
    private bool reloading;

    private float time;
    private float moveDirX;
    private float moveDirZ;

    private float xRotation;
    private float yRotation;
    private float cameraRotationX;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); // 컴포넌트를 rigid 변수에 대입 
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Jump();
        input();
        MouseCurser();
        attack();
        StartCoroutine("Reload");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraController();
        CharacterRotation();
    }

    private void input()
    {
        moveDirX = Input.GetAxisRaw("Horizontal"); //좌우 방향키 값을 리턴 (1,0,-1)
        moveDirZ = Input.GetAxisRaw("Vertical"); //앞뒤 방향키 값을 리턴 
        xRotation = Input.GetAxisRaw("Mouse Y");
        yRotation = Input.GetAxisRaw("Mouse X");
        jump = Input.GetButtonDown("Jump");
        load = Input.GetButtonDown("Reload");
        fire = Input.GetMouseButton(0);
    }

    private void Move()
    {
        if (moveDirX!=0 || moveDirZ != 0)
        {
            Vector3 MoveHorizontal = transform.right * moveDirX; //transform 오브젝트의 위치,각도 정보 컴포넌트 
            Vector3 MoveVertical = transform.forward * moveDirZ;

            Vector3 velocity = (MoveHorizontal + MoveVertical).normalized * moveSpeed; //normalized 합의 값이 1로 고정 

            rigid.MovePosition(transform.position + velocity * Time.deltaTime); //1초 마다 움직임 
        }
    }

    private void Jump()
    {
        if (jump && !jumping)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumping = true;
        }
    }

    //마우스 커서 비활성화 
    private void MouseCurser()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //상하 카메라 회전
    private void CameraController()
    {
        cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //변수 값으로 최대값 고정 

        Mycamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(characterRotationY));
    }

    private void attack()
    {
        RandomDamage();
        time += Time.deltaTime;
        if (!reloading && nowcount > 0 && fire && time >= delay)
        {
            GameObject instantBullet = Instantiate(bulletObject, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * bulletSpeed;
            soundMagager.SoundSetup("shoot");

            GameObject instantCasing = Instantiate(casingObject, casingPos.position, casingPos.rotation);
            Rigidbody CasingRigid = instantCasing.GetComponent<Rigidbody>();
            Vector3 casingvec = casingPos.forward * Random.Range(-2, -4) + Vector3.right * Random.Range(0, 0);
            CasingRigid.AddForce(casingvec, ForceMode.Impulse);

            nowcount--;
            time = 0;
        }

    }

    IEnumerator Reload()
    {
        if (load)
        {
            reloading = true;
            soundMagager.SoundSetup("Reloadsound");
            yield return new WaitForSeconds(1.51f);
            nowcount = casingcount;
            reloading = false;
        }
    }

    private void RandomDamage()
    {
        AttackStr = Random.Range(5, 8);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumping = false;
        }
    }
}
