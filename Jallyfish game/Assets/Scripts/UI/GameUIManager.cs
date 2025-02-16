﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    //выбор оружия
    public event Action choosWeaponOne;
    public event Action choosWeaponTwo;
    public event Action choosWeaponThree;
    public event Action choosWeaponFour;

    //в главное меню из паузы
    public event Action OnSaveGame;
    public event Action OnBackMainMenu;
    public event Action OnContinue;

    //кнопки оруджия на экране
    public Button WeaponOne;
    public Button WeaponTwo;
    public Button WeaponThree;
    public Button WeaponFour;
    //кнопки оружия картинки
    [Space(10)]
    public Image FillerOne;
    public Image FillerTwo;
    public Image FillerThree;
    public Image FillerFour;
    //Здоровье,щит,ускорение
    [Space(10)]
    public PlayerHealthBar PlayerHealthBar;
    public PlayerArmorBar PlayerArmorBar;
    public PlayerStaminaBar PlayerStaminaBar;
    public PlayerLevelBar levelBar;
    //public Text levelText; //показывает лэвл

    public EnamyHealthBar enamyHealthBar;

    [Space(10)] //меню паузы
    public Button SaveGame;
    public Button Continue;
    public Button BackMainMenu;

    //пауза
    public GameObject PauseMenuUI;
    //private bool IsPaused = false;

    private void Awake()
    {
        WeaponOne.onClick.AddListener(ChooseOne);
        WeaponTwo.onClick.AddListener(ChooseTwo);
        WeaponThree.onClick.AddListener(ChooseThree);
        WeaponFour.onClick.AddListener(ChooseFour);

        SaveGame.onClick.AddListener(SaveGameButton);
        Continue.onClick.AddListener(BackInGame);
        BackMainMenu.onClick.AddListener(BackMainMenuButton);
    }

    //сохранение
    private void SaveGameButton() //если нажата кнопка то сохраняем игру
    {
        if (OnSaveGame != null)
        {
            OnSaveGame();
        }
    }

    //пауза
    //private void Unpause()// метод чтобы отключить паузу
    //{
    //    Time.timeScale = 1f;
    //    IsPaused = false;
    //}

    //private void Pause() //вклюает паузу и время останавливается
    //{
    //    Time.timeScale = 0f;
    //    IsPaused = true;
    //}

    public void BackInGame()
    {
        if (OnContinue != null)
        {
            OnContinue();
        }
    }

    private void BackMainMenuButton() //если нажата кнопка назад в меню то пауза выключается  и идём в меню 
    {
        if (OnBackMainMenu != null)
        {
            //Unpause();
            OnBackMainMenu();
        }
    }

    public void SetPauseMenuActive(bool state)
    {
        PauseMenuUI.SetActive(state);
    }

    //public void PauseCheck() //включает паузу
    //{
    //    if (IsPaused) //если выключает паузу то IsPaused = false
    //    {
    //        Unpause(); //и пауза выклюяается
    //    }
    //    else
    //    {
    //        Pause(); //включчается пауза
    //    }

    //    PauseMenuUI.SetActive(IsPaused); //или включена или нет
    //}

    //кнопки для оружия
    private void ChooseOne()
    {
        choosWeaponOne?.Invoke();
    }

    private void ChooseTwo()
    {
        choosWeaponTwo?.Invoke();
    }

    private void ChooseThree()
    {
        choosWeaponThree?.Invoke();
    }

    private void ChooseFour()
    {
        choosWeaponFour?.Invoke();
    }

    public void SetWeaponFiller(EWeapon weapon, float percentage)
    {
        switch (weapon)
        {
            case EWeapon.BulletLight:
                FillerOne.fillAmount = percentage;
                break;
            case EWeapon.BulletJelly:
                FillerTwo.fillAmount = percentage;
                break;
            case EWeapon.JellyHoming:
                FillerThree.fillAmount = percentage;
                break;
            case EWeapon.Fart:
                FillerFour.fillAmount = percentage;
                break;
        }
    }

    public void ChangeHealthe(float curHp) //отображает здоровье
    {
        PlayerHealthBar.SetValue(curHp);
    }

    public void ChangeArmor(float curArmor) //отображает щит
    {
        PlayerArmorBar.SetValue(curArmor);
    }

    public void ChangeStamina(float curStam) //отображает ускорение
    {
        PlayerStaminaBar.SetValue(curStam);
    }

    public void ChangeLevel(float level) //отображает лэвл
    {
        levelBar.SetValue(level);
    }


    public void EnamyHealth(float hp)
    {
        enamyHealthBar.SetValue(hp);
    }

}
