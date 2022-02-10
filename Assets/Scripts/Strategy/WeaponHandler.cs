using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    private IThrow[] weaponss;
    public IThrow _currentWeapon;
    public int _currentWeaponIndex;

    private void Start()
    {
        weaponss = GetComponentsInChildren<IThrow>();
        _currentWeapon = weaponss[0];
    }
       
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_currentWeaponIndex > 0)
            {

                _currentWeaponIndex--;
                _currentWeapon = ChangeWeapon(_currentWeaponIndex);
            }
            else
            {
                _currentWeaponIndex++;
                _currentWeapon = ChangeWeapon(_currentWeaponIndex);
            }
        }

    }
    private IThrow ChangeWeapon(int index)
    {
      return weaponss[index];
    }
}
