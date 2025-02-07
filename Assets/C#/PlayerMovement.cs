
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController Controller;//角色控制器
    public float speed = 10f;//速度
    public float walkSpeed = 10f;//行走速度
    public float runSpeed = 15f;//奔跑速度
    public float jumpPower = 3f;//跳跃力度
    public Vector3 Velocity;//设置y轴的充量变化\
    private Transform groundCheck;//地面检测
    public LayerMask groundMask;//与主角碰撞的面
    private bool isGround;//检测碰撞的变量
    public float gravity;//重力
    private float groundDistance=0.1f;//与地面的距离
    private bool isRun;//判断是否在奔跑
    private bool isJump;//判断是否在跳跃
    private Vector3 playerMovement;//角色位置
    private KeyCode LiftShift;//奔跑键位
    private KeyCode jumpKeyCode;//跳跃键位
    private float times;//跳跃时间计时
    public float slopForce = 600f;//走斜坡的力
    public float slopForceRayLength = 2f;//斜坡射线长度 
    private AudioSource audioSource;//声音控制器
    public AudioClip walkSound;//步行
    public AudioClip runSound;//跑步

    private Animator animator;

    private Vector3 RecordWhetherMove;
    // Start is called before the first frame update
    void Start()
    {
        Controller = this.GetComponent<CharacterController>();
        audioSource = this.GetComponent<AudioSource>();
        LiftShift = KeyCode.LeftShift;
        jumpKeyCode = KeyCode.Space;
        groundCheck = GameObject.Find("Player/groundCheck").GetComponent<Transform>();
        animator = GameObject.Find("Gam Camera/WeaponHolder/AK47/arms_assault_rifle_01").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        move();
    }

    private void move()
    {
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");
        //Debug.Log(h+"+"+v);
        isRun=Input.GetKey(LiftShift);
        if (isRun)
        {
            speed = runSpeed;
            animator.SetBool("Run",true);
            //animator.Play("Run");
        }
        else
        {
            animator.SetBool("Run",false);
            speed = walkSpeed;
        }
        //playerMovement = new Vector3(h*speed*Time.deltaTime, 0, v*speed*Time.deltaTime);
        playerMovement = (transform.right * h + transform.forward * v).normalized;
        Debug.Log(playerMovement);
        if (RecordWhetherMove == playerMovement)
        {
            animator.SetBool("Walk",false);
            animator.SetBool("Fire",false);
            RecordWhetherMove = playerMovement;
        }
        else
        {
            animator.SetBool("Walk",true);
            animator.SetBool("Fire",true);
        }
        Controller.Move(playerMovement*speed*Time.deltaTime);
         times += Time.deltaTime;
                // Debug.Log(times);
                // if (times>3f && times<6f)
                // {
                //     Debug.Log(times);
                //     
                //     times = 0f;
                // }
        if (isGround == false)
        {
            if (times>0.2f && times<0.4f)
            {
                Velocity.y += gravity + Time.deltaTime;
            }
            if (times>0.4f)
            {
                times = 0f;
            }
        }
        //Velocity.y += gravity + Time.deltaTime;
        Controller.Move(Velocity * Time.deltaTime);
        jump();
        //Debug.Log(Vector3.down +"+"+ Controller.height);
        Onslpe();
        if (true)
        {
            Controller.Move(Vector3.down * Controller.height / 2 * slopForce * Time.deltaTime);
        }

        PlayFootStepSound();
    } 
    //播放声音
    public void PlayFootStepSound()
    {
        if (isGround && playerMovement.sqrMagnitude>0.9f)
        {
            audioSource.clip = isRun ? runSound : walkSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
    //跳跃
    private void jump()
    {
        isJump=Input.GetKey(jumpKeyCode);
        if (isJump && isGround)
        {
            Velocity.y =  MathF.Sqrt(jumpPower * -2f * gravity);
        }
    }
    //地面检测
    public void CheckGround()
    {
        //检测位置是否在地面上
       isGround=Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
       if (isGround &&  Velocity.y <= 0f)
       {
           Velocity.y = -2f;
       }
       // if (isGround && Velocity.y <= 0)
       // {
       //     Velocity.y = (-2f);
       // } 
    }
    //控制斜坡平滑下坡
    public bool Onslpe()
    {
        
        if (isJump)
        {
            return false;
        }
            
        RaycastHit hit;
        //Debug.Log(Physics.Raycast(transform.position, Vector3.down, out hit, 1.8f));
         //Debug.Log(hit.normal != Vector3.up);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.8f))
        {
             if (hit.normal != Vector3.up) {
                     return true; 
             }
        }
        return false;
    }
}
