using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform WeaponHold;
    Gun EquippedGun;
    public Gun StartingGun;

    // Start is called before the first frame update
    void Start()
    {
        if(StartingGun != null)
        {
            EquipGun(StartingGun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipGun(Gun gunToEquip)
    {
        if(EquippedGun != null)
        {
            Destroy(EquippedGun.gameObject);
        }

        EquippedGun = Instantiate(gunToEquip, WeaponHold.position, WeaponHold.rotation);
        // gun should move with player
        EquippedGun.transform.parent = WeaponHold;
    }

    public void shoot()
    {
        if(EquippedGun != null)
        {
            EquippedGun.Shoot();
        }
    }
}
