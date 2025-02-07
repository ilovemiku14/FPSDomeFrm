using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform shootPoint;//发射的位置

    public int bulletsMag = 30;//弹夹容量

    public int range = 300;//射击距离

    public int bulletleft = 300; //备弹

    public int currentBullects; //当前弹夹剩余子弹

    private bool GunShootInput;//检测是否按下鼠标左键

    public float fireRate = 0.1f;//射击射速

    private float fireTimer;//射击计时器

    private KeyCode relodinputName;//换单检测
    
    public ParticleSystem muzzleFlash;//枪口火焰特效
    
    public Light muzzleLight;//枪口火焰灯光
    
    private AudioSource audioSource;
    
    public AudioClip Ak47SoundClip;//ak47设计音效片段 
    
    public GameObject hitparticle;//子弹击中粒子特效

    public GameObject bullectHole;//弹孔

    private Animator anim;//动画控制器

    public float BombChangingTime=2f;//换弹时间

    private bool isBombChanging;//检测动画是否播放完成

    private KeyCode viewAnimation;//检视动画

    private KeyCode throwGrenades;//投掷手雷Aim In

    private KeyCode ExecutionsOrDaggerAttack;//处决动画或者匕首攻击

    private KeyCode aimAndShoot;//瞄准射击

    private Camera camera;//摄像机

    public float view = 60f;//正常视野
    public float minView=40f;//视野

    private float temporaryVision;//临时视野值
    //private KeyCode daggerAttack;//
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        currentBullects = bulletsMag;
        relodinputName = KeyCode.R;
        anim = GetComponent<Animator>();
        viewAnimation = KeyCode.I;
        throwGrenades = KeyCode.G;
        ExecutionsOrDaggerAttack = KeyCode.E;
        aimAndShoot = KeyCode.Mouse1;
        camera = GetComponent<Camera>();
        //Camera.main.fieldOfView = view;
    }

    // Update is called once per frame
    void Update()
    {  
        // temporaryVision = view - Time.deltaTime * 100;
        //          Camera.main.fieldOfView = temporaryVision;
        // anim.SetBool("Run",true);
        // anim.SetBool("Walk",false);
        GunShootInput=Input.GetKey(KeyCode.Mouse0);
        if (Input.GetKey(aimAndShoot))
        {
            anim.SetBool("Aim",true);
         
            if (GunShootInput && currentBullects>0)
            {
                if (view>minView)
                {
                    anim.Play("Aim Fire");
                    GumFire();
                }
                else
                {
                    anim.Play("Aim Fire");
                    GumFire();
                }
            }
        }
        else
        {
            // temporaryVision = view + Time.deltaTime * 10;
            // Camera.main.fieldOfView = temporaryVision;
            anim.SetBool("Aim",false);
        }
        temporaryVision=view;
        if (Input.GetKey(viewAnimation))
        {
            anim.Play("Inspect");
        }
        if (Input.GetKey(throwGrenades))
        {
            anim.Play("GrenadeThrow");
        }
        if (Input.GetKey(ExecutionsOrDaggerAttack))
        {
            anim.Play("Knife Attack 2");
        }
        if (GunShootInput)
        {
            if (!anim.GetBool("Run"))
            {
                if (!anim.GetBool("Aim"))
                {
                    GumFire();
                                     anim.Play("Fire",0,fireTimer);
                }
            }
        }
        else
        {
            muzzleLight.enabled = false; 
        }

        if (Input.GetKeyDown(relodinputName) && currentBullects<bulletsMag && bulletleft>0)
        {
            if (currentBullects>0)
            {
                anim.Play("Reload Ammo Left",0,0);
            }
            else
            {
                anim.Play("Reload Out Of Ammo",0,0);
            }
        } 
        if (isBombChanging) 
        {
                        Relod();
                        isBombChanging = false;
        } 
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
                      if (stateinfo.IsName("Reload Ammo Left") && stateinfo.normalizedTime >= 1.0f || stateinfo.IsName("Reload Out Of Ammo") && stateinfo.normalizedTime >= 1.0f)
                      {
                          isBombChanging = true;
                          //Debug.Log(0);
                         
                          //方法
                      }
                      else if (stateinfo.IsName("Reload Ammo Left") && stateinfo.normalizedTime <= 0.0f || stateinfo.IsName("Reload Out Of Ammo") && stateinfo.normalizedTime <= 0.0f)
                      {
                          //Debug.Log(1);
                          //方法
                      }
        if (fireTimer <fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    //射线模拟子弹
    public void GumFire()
    {
        if (fireTimer <fireRate || currentBullects<=0) return;
        RaycastHit hit;
        Vector3 shootDirection = shootPoint.forward;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit,range))
        {
            Debug.Log(hit.transform.name);
            GameObject hitparticles= Instantiate(hitparticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));//实例化击中特效
            GameObject bullectHoles= Instantiate(bullectHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));//实例化弹孔特效
            Destroy(hitparticles,1f);
            Destroy(bullectHoles,3f);
            // Debug.Log(currentBullects);
        }
        muzzleFlash.Play();
        PlayerShootSould();
        muzzleLight.enabled = true;
        currentBullects--;
        fireTimer = 0f;//重置
    }
    //换单方法
    // public int bulletsMag = 30;//弹夹容量
    //
    // public int range = 300;//射击距离
    //
    // public int bulletleft = 300; //备弹
    //
    // public int currentBullects; //当前弹夹剩余子弹
    public void Relod()
    {
        if (bulletleft<=0) return;
        int bulletToLoad = bulletsMag - currentBullects;
        int bulletToReduce=bulletleft >= bulletToLoad ? bulletToLoad : bulletleft;
        bulletleft -= bulletToReduce;
        currentBullects += bulletToReduce;
    }
    //播放射击声音
    public void PlayerShootSould()
    {
        audioSource.clip = Ak47SoundClip;
        audioSource.Play();
    }
}
