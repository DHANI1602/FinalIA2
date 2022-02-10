using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PlayerModel _playerModel;
    public PlayerController _playerController;
    public AudioClip[] shootSounds;
    public AudioSource Audio;

    void Update()
    {
        if (MainMenu.GameIsPaused == false)
        {

            if (Input.GetMouseButtonDown(0) & _playerController.GetTimer() <= 0 && _playerController.GetisFiring() == false)
            {
                Audio.clip = shootSounds[0];
                Audio.Play();
            }


            if (Input.GetMouseButton(1) && _playerController.GetTimer1() <= 0)
            {

                if (_playerModel.WH._currentWeaponIndex == 0)
                {

                    Audio.clip = shootSounds[2];
                    Audio.Play();
                }
                else
                {
                    Audio.clip = shootSounds[1];
                    Audio.Play();
                }

            }

            else if (Input.GetMouseButtonUp(1))
            {
                if (_playerModel.WH._currentWeaponIndex == 0)
                {

                    Audio.Pause();
                }


            }

        }


    }
}
