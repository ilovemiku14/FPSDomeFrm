using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public WeaponController weaponController;

    private TextMeshProUGUI sun;
    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Fire").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        sun.text = weaponController.currentBullects.ToString()+"/"+weaponController.bulletleft.ToString();
    }
}
