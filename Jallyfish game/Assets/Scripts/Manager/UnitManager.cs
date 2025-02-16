﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //будет отвечать на связь твоего перса со всеми внутренними системами 

    public event Action<EWeapon, float> WeaponColldownChanged;

    public event Action<float> ChangeHealth; 
    public event Action<float> ChangeArmor;
    public event Action<float> ChangeFast;
    public event Action<float> ChangeLevel;
    public event Action PlayerDead;

    private PlayerHealthScript PlayerHealthScript; //скрипт со здоровьем
    private PlayerMovement PlayerMovementScript; //скрипт со скоростью 
    private GroundChecker GroundChecker;//скрипт с лучём
    private WeaponManager WeaponManager; //скрипит с видами оружия
    private PlayerLevelSystem PlayerLevelSystem;

    private void Awake()
    {
        PlayerMovementScript = GetComponent<PlayerMovement>();
        GroundChecker = GetComponent<GroundChecker>();
        WeaponManager = GetComponent<WeaponManager>();
        PlayerHealthScript = GetComponent<PlayerHealthScript>();
        PlayerLevelSystem = GetComponent<PlayerLevelSystem>();

        WeaponManager.WeaponColldownChanged += WeaponManager_WeaponColldownChanged;
        PlayerHealthScript.ChangeHealth += PlayerHealthScript_ChangeHealth;
        PlayerHealthScript.ChangeArmor += PlayerHealthScript_ChangeArmor;
        PlayerMovementScript.ChangeFast += PlayerMovementScript_ChangeFast;
        PlayerLevelSystem.ChangeLevel += PlayerLevelSystem_ChangeLevel;

        PlayerHealthScript.PlayerDead += PlayerHealthScript_PlayerDead;
    }

    private void Update()
    {
        float difference = GroundChecker.CheckGround();
        PlayerMovementScript.SetVerticalPosition(difference);
    }

    public void FillSaverData(SaverData saverData)
    {
        saverData.PlayerLevel = PlayerLevelSystem.CurrentLevel;
        saverData.PlayerXp = PlayerLevelSystem.currentXp;
        saverData.PlayerRequiredXp = PlayerLevelSystem.requiredXp;

        saverData.PlayerHP = PlayerHealthScript.CurrentHP;
        saverData.PlayerArmor = PlayerHealthScript.curArmor;
        saverData.PlayerPosition = new float[] { transform.position.x, transform.position.y, transform.position.z };
    }

    public void ApplySaverData(SaverData saverData)
    {
        PlayerLevelSystem.SetNewLevel(saverData.PlayerLevel);
        PlayerLevelSystem.SetNewXp(saverData.PlayerXp);
        PlayerLevelSystem.SetNewRequiredXp(saverData.PlayerRequiredXp);

        PlayerHealthScript.SetNewHealth(saverData.PlayerHP);
        PlayerHealthScript.SetNewArmor(saverData.PlayerArmor);
        transform.position = new Vector3(saverData.PlayerPosition[0], saverData.PlayerPosition[1], saverData.PlayerPosition[2]);
    }

    //игрок умер
    private void PlayerHealthScript_PlayerDead()
    {
        PlayerDead?.Invoke();
    }


    private void PlayerLevelSystem_ChangeLevel(float level)
    {
        ChangeLevel?.Invoke(level);
    }

    private void PlayerMovementScript_ChangeFast(float curStam)
    {
        ChangeFast?.Invoke(curStam);
    }

    private void PlayerHealthScript_ChangeArmor(float curArm)
    {
        ChangeArmor?.Invoke(curArm);
    }

    private void PlayerHealthScript_ChangeHealth(float curHP)
    {
        ChangeHealth?.Invoke(curHP);
    }


    private void WeaponManager_WeaponColldownChanged(EWeapon weapon, float cooldownPercent)
    {
        WeaponColldownChanged?.Invoke(weapon, cooldownPercent);
    }

    
    public void Init(ResourceManager resourceManager, AudioManager audioManager)
    {
        WeaponManager.Init(resourceManager, PlayerLevelSystem, audioManager);
    }


    //методы для выбора оружия
    public void ChoosWeaponOne()
    {
        WeaponManager.ChoosWeaponOne();
    }

    public void ChoosWeaponTwo()
    {
        WeaponManager.ChoosWeaponTwo();
    }

    public void ChoosWeaponThree()
    {
        WeaponManager.ChoosWeaponThree();
    }

    public void ChoosWeaponFour()
    {
        WeaponManager.ChoosWeaponFour();
    }

    public void Shoot()
    {
        WeaponManager.Shoot();
    }


    //метод принимает направление и передаёт в скрипт 
    public void ChangeMovementDirection(Vector3 direction) 
    {
        PlayerMovementScript.ChangeMovementDirection(direction);
    }

    //метод принимает точку и передаёт в скрипт
    public void ChangeLookingPoint(Vector3 point) 
    {
        WeaponManager.SetShootingPoint(point);

        PlayerMovementScript.ChangeLookingPoint(point - transform.position);
    }


    //метод ускорение
    public void fastSpeedStart() //вызываем метод
    {
        PlayerMovementScript.fastSpeedStart();
    }

    public void fastSpeesEnd()
    {
        PlayerMovementScript.fastSpeesEnd();
    }
}
