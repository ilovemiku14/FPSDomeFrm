using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;//视野灵敏度

    public Transform playBody;//玩家位置

    private float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        //隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float X = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        float Y = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;
        xRotation -= Y;
        //Debug.Log(X+"+"+Y);
        //限制最大最小值
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //摄像机的y移动
        transform.localRotation=Quaternion.Euler(xRotation,0f,0f);
        //player的x移动
        playBody.Rotate(Vector3.up*X );
    }
}
