﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

public partial class WeaponManager
{
    [CustomEditor(typeof(WeaponManager))]
    private class WeaponMangerInspector : Editor
    {
        private WeaponManager weaponManager;

        private void OnEnable()
        {
            weaponManager = (WeaponManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);

            foreach (EWeapon eWeapon in (EWeapon[])Enum.GetValues(typeof(EWeapon)))
            {
                if (weaponManager.WeaponsCurrentCooldown.ContainsKey(eWeapon))
                {
                    GUILayout.Label(eWeapon + "   :   " + weaponManager.WeaponsCurrentCooldown[eWeapon]);
                }
            }
        }
    }
}

#endif

public partial class WeaponManager : MonoBehaviour
{
    public event Action<EWeapon, float> WeaponColldownChanged;

    //шарит за оружие
    public Transform firePoint;

    public EWeapon currentWeapon; //текущее оружие

    private ResourceManager ResourceManager;
    private PlayerLevelSystem LevelsSystem;
    private AudioManager AudioManager;

    private Dictionary<EWeapon, float> WeaponsCurrentCooldown = new Dictionary<EWeapon, float>();

    private Dictionary<EWeapon, float> WeaponsDefaultCooldown = new Dictionary<EWeapon, float>()
    {
        //{ EWeapon.BulletLight, 0.5f },
        //{ EWeapon.BulletJelly, 1f },
        //{ EWeapon.JellyHoming, 1.5f },
        //{ EWeapon.Fart, 2f },

        { EWeapon.BulletLight, 0.01f },
        { EWeapon.BulletJelly, 0.01f },
        { EWeapon.JellyHoming, 0.01f },
        { EWeapon.Fart, 0.01f }
    };

    private void Awake()
    {
        currentWeapon = EWeapon.BulletLight;
    }

    private void Update()
    {
        foreach (EWeapon eWeapon in (EWeapon[])Enum.GetValues(typeof(EWeapon)))
        {
            if (WeaponsCurrentCooldown.ContainsKey(eWeapon))
            {
                if (WeaponsCurrentCooldown[eWeapon] < 0)
                {
                    WeaponsCurrentCooldown[eWeapon] = 0;
                }
                else if (WeaponsCurrentCooldown[eWeapon] > 0)
                {
                    WeaponsCurrentCooldown[eWeapon] -= Time.deltaTime;
                }

                WeaponColldownChanged?.Invoke(eWeapon, WeaponsCurrentCooldown[eWeapon] / WeaponsDefaultCooldown[eWeapon]); //получаем проценты
            }
        }
    }

    public void Init(ResourceManager resourceManager, PlayerLevelSystem levelsSystem, AudioManager audioManager)
    {
        ResourceManager = resourceManager;
        LevelsSystem = levelsSystem;
        AudioManager = audioManager;
    }

    public void ChoosWeaponOne()
    {
        currentWeapon = EWeapon.BulletLight;
    }

    public void ChoosWeaponTwo()
    {
        currentWeapon = EWeapon.BulletJelly;
    }

    public void ChoosWeaponThree()
    {
        currentWeapon = EWeapon.JellyHoming;
    }

    public void ChoosWeaponFour()
    {
        currentWeapon = EWeapon.Fart;
    }

    public void SetShootingPoint(Vector3 newPoint)
    {
        Vector3 targetDirection = newPoint - transform.position;
        targetDirection = targetDirection.normalized;
        firePoint.position = transform.position + targetDirection;

        firePoint.LookAt(newPoint);
    }

    public void Shoot()
    {
        if (WeaponsCurrentCooldown.ContainsKey(currentWeapon) == false)
        {
            WeaponsCurrentCooldown.Add(currentWeapon, 0);
        }

        if (WeaponsCurrentCooldown[currentWeapon] <= 0)
        {
            switch (currentWeapon)
            {
                case EWeapon.BulletLight:
                    ShootBulletLight();
                    break;
                case EWeapon.BulletJelly:
                    ShootBulletJelly();
                    break;
                case EWeapon.JellyHoming:
                    ShootBulletHoming();
                    break;
                case EWeapon.Fart:
                    ShootFart();
                    break;
            }

            WeaponsCurrentCooldown[currentWeapon] = WeaponsDefaultCooldown[currentWeapon];
        }
    }

    private void ShootBulletLight() //удар молнии
    {
        GameObject bullet = ResourceManager.GetObjectInstance(EObjectType.BulletLight); //берём пулю из пула

        if (bullet != null) //если пуля не пуста
        {
            if (LevelsSystem != null) //и система лэвлов не пуста
            {
                AudioManager.Play("SoundWeaponLight");

                WeaponBulletLight bulletLight = bullet.GetComponent<WeaponBulletLight>(); //то берём скрипт пули
                bulletLight.SetShooterLevelsSystem(LevelsSystem);  //и метод из лэвлов
            }

            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
        }
    }

    private void ShootBulletJelly() //метод пули медуза
    {
        GameObject bullet = ResourceManager.GetObjectInstance(EObjectType.BulletJelly); //берём пулю из пула

        if (bullet != null) //если пуля не пуста
        {
            if (LevelsSystem != null)//и система лэвлов не пуста
            {
                AudioManager.Play("SoundWeaponJelly");

                WeaponBulletJelly bulletJelly = bullet.GetComponent<WeaponBulletJelly>(); //то берём скрипт пули
                bulletJelly.SetShooterLevelsSystem(LevelsSystem);  //и метод из лэвлов
            }
        }

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);
    }

    private void ShootBulletHoming() //Самонаводящееся пуля
    {
        GameObject bullet = ResourceManager.GetObjectInstance(EObjectType.JellyHoming); //берём пулю из пула

        if (bullet != null) // если пуля не пуста
        {
            if (LevelsSystem != null) //и система лэвлов не пуста
            {
                AudioManager.Play("SoundWeaponHoming");

                WeaponJellyHoming bulletHoming = bullet.GetComponent<WeaponJellyHoming>(); //то берём скрипт пули
                bulletHoming.Initialize(transform, LevelsSystem);  //и метод из лэвлов
            }

            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
        }
    }

    private void ShootFart() //Большой пук
    {
        GameObject bullet = ResourceManager.GetObjectInstance(EObjectType.BulletFart); //берём пулю из пула

        if (bullet != null) // если пуля не пуста
        {
            if (LevelsSystem != null) //и система лэвлов не пуста
            {
                AudioManager.Play("SoundWeaponFart2");

                WeaponFart bulletHoming = bullet.GetComponent<WeaponFart>(); //то берём скрипт пули
                bulletHoming.SetShooterLevelsSystem(LevelsSystem);  //и метод из лэвлов
            }

            bullet.transform.position = transform.position;
            bullet.SetActive(true);
        }
    }
}
