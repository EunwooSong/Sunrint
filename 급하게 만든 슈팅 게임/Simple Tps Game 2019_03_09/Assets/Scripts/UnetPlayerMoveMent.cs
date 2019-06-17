using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Animator, CharacterrController Componnet 적용
[RequireComponent(typeof(Animator))]

public class UnetPlayerMoveMent : MonoBehaviour {

    Animator anim;

    //Animation작동을 위한 설정
    [System.Serializable]
    public class AnimationSettings
    {
        public string verticalVelocity = "Forward";
        public string horizontalVelocity = "Strafe";
        public string jumpBool = "isJumping";
        public string groundedBool = "isGrounded";
        public float jumpTime = 1f;
    }
    [SerializeField]
    public AnimationSettings animations;

    //중력이 잘 작동하지 않아 여러 사이트를 참조하여 제작.....ㅠㅠ
    //Gravity--------------------------------------------------------------
    [System.Serializable]
    public class PhysicsSettings
    {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50.0f;
        public float resetGravityValue = 1.2f;
    }
    [SerializeField]
    public PhysicsSettings physics;

    bool resetGravity;
    public float gravity = 20.0f;

    //Anim-----------------------------------------------------------------

    public float moveSpeed = 6.0f;
    public float jumpSpeed = 6.0f;
    public bool isJump = false;
    
    Vector3 MoveDir = Vector3.zero;

    CharacterController ch;
    //Move-----------------------------------------------------------------

    private void Start()
    {
        anim = GetComponent<Animator>();
        ch = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (ch.isGrounded)
        {
            if(Input.GetButton("Jump")) Jump();
        }
        Animate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }

    void Animate(float forward, float strafe)
    {
        anim.SetFloat(animations.verticalVelocity, forward);
        anim.SetFloat(animations.horizontalVelocity, strafe);
        anim.SetBool(animations.jumpBool, isJump);
        anim.SetBool(animations.groundedBool, ch.isGrounded);
    }

    void ApplyGravity() {
        if (!ch.isGrounded)
        {
            if (!resetGravity)
            {
                gravity = physics.resetGravityValue;
                resetGravity = true;
            }
            gravity += Time.deltaTime * physics.gravityModifier;
        }

        else {
            gravity = physics.baseGravity;
            resetGravity = false;
        }

        Vector3 gravityVector = new Vector3();

        if (!isJump) {

        }
    }

    
    void Jump() {
        isJump = true;
        //MoveDir.y = jumpSpeed;
        StartCoroutine("StopJump");
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(animations.jumpTime);
        isJump = false;
    }
}



































































/*
private Transform tr;
private CharacterController ch;

public float moveSpeed = 5.0f;
public float aimSpeed = 2.0f;
public float runSpeed = 10.0f;
public float jumpSpeed = 3.0f;
public float rotSpeed = 150.0f;
public float gravity = 20.0f;


private Vector3 move = Vector3.zero;
// Use this for initialization
void Start()
{
    tr = GetComponent<Transform>();
    ch = GetComponent<CharacterController>();
}

void LateUpdate()
{

    float r = Input.GetAxis("Mouse X"); //rotate
    float v = Input.GetAxis("Vertical"); //forward, back
    float h = Input.GetAxis("Horizontal"); //left, right
    bool s = Input.GetKey(KeyCode.Space);
    bool isAime = Input.GetMouseButton(1); // aim
    bool isRun = Input.GetKey(KeyCode.LeftShift); //run

    PlayerMove(r, v, h, s, isAime, isRun);
}

void PlayerMove(float r, float v, float h, bool s, bool isAim, bool isRun)
{


    if (ch.isGrounded)
    {
        move = (tr.forward * v) + (tr.right * h);
        if (s)
            move.y += jumpSpeed;
    }
    move.y -= gravity * Time.deltaTime;

    if (isAim)
    {
        ch.Move(move * aimSpeed * Time.deltaTime);
    }
    else if (isRun)
    {
        ch.Move(move * runSpeed * Time.deltaTime);
    }
    else
    {
        ch.Move(move * moveSpeed * Time.deltaTime);
    }
}
*/
