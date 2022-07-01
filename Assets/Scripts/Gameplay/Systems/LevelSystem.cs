using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class LevelSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private EcsWorld _world = null;
    private ExperienceConfig _experienceConfig;
    private int _lvlNumber;
    private string _firstWeapon;
    private string _secondWeapon;
    private string _thirdWeapon;
    private WeaponComponent _playerWeapons;
    
    public void Init(EcsSystems systems)
    {
        LevelUISystem.OnLevelUp += LevelUp;
        _world = systems.GetWorld ();
        
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<PlayerExperienceComponent>()
            .Inc<WeaponComponent>()
            .Inc<ExperienceCounterComponent>().End();
        var weaponPool = _world.GetPool<WeaponComponent>();
        
        var UIFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var LevelButton = _world.GetPool<LevelButtonComponent>();

        foreach (var entity in playerFilter)
        {
            _playerWeapons = weaponPool.Get(entity);
            _playerWeapons.LevelSettings[0].Level = 1;
            for (var i = 1; i < _playerWeapons.LevelSettings.Length; i++)
            {
                _playerWeapons.LevelSettings[i].Level = 0;
            }
        }

        foreach (var entity in UIFilter)
        {
            ref var levelButtons = ref LevelButton.Get(entity);
            levelButtons.Button_weapon_1.onClick.AddListener(ClickOne);
            levelButtons.Button_weapon_2.onClick.AddListener(ClickTwo);
            levelButtons.Button_weapon_3.onClick.AddListener(ClickThree);
        }

        UpdateIcons();
    }

    public void Destroy(EcsSystems systems)
    {
        LevelUISystem.OnLevelUp -= LevelUp;
    }

    private void ClickOne()
    {
        LevelUpWeapon(_firstWeapon);
        UpdateIcons();
    }
    private void ClickTwo()
    {
        LevelUpWeapon(_secondWeapon);
        UpdateIcons();
    }
    private void ClickThree()
    {
        LevelUpWeapon(_thirdWeapon);
        UpdateIcons();
    }

    private void LevelUpWeapon(string weaponName)
    {
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<WeaponComponent>().End();
        var weaponPool = _world.GetPool<WeaponComponent>();
        foreach (var i in playerFilter)
        {
            ref var weaponLevels = ref weaponPool.Get(i);
            for (int j = 0; j < weaponLevels.LevelSettings.Length; j++)
            {
                if (weaponLevels.LevelSettings[j].Name == weaponName)
                {
                    weaponLevels.LevelSettings[j].Level++;
                }
            }
        }
    }

    private void UpdateIcons()
    {
        var uiFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var uiPool = _world.GetPool<MainUIComponent>();
        foreach (var entity in uiFilter)
        {
            ref var ui = ref uiPool.Get(entity);
            Image[] icons = new Image[4] 
            {ui.IconCube, ui.IconBall, 
                ui.IconThree, ui.IconT};
            for (var i = 0; i < _playerWeapons.LevelSettings.Length; i++)
            {
                Debug.Log(_playerWeapons.LevelSettings[i].Level);
                if (_playerWeapons.LevelSettings[i].Level > 0)
                {
                    icons[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void LevelUp()
    {
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<WeaponComponent>().End();
        var weaponPool = _world.GetPool<WeaponComponent>();

        var UIFilter = _world.Filter<UIComponent>().End();
        var UIviews = _world.GetPool<UIComponent>();
        
        foreach (var i in playerFilter)
        {
            ref var weaponLevels = ref  weaponPool.Get(i);
            foreach (var entity in UIFilter)
            {
                ref var uiViewsComponent = ref  UIviews.Get(entity);
                uiViewsComponent._view_LevelUp.Show();
            }
            Time.timeScale = 0;
                
            foreach (var weapon in weaponLevels.LevelSettings)
            {
                if (weapon.Level > 0)
                {
                    _firstWeapon = weapon.Name;
                    break;
                }
            }
            while (_secondWeapon == null)
            {
                var weapon = weaponLevels.LevelSettings[Random.Range(0, weaponLevels.LevelSettings.Length)];
                if (weapon.Name != _firstWeapon)
                    _secondWeapon = weapon.Name;
            }
            while (_thirdWeapon == null)
            {
                var weapon = weaponLevels.LevelSettings[Random.Range(0, weaponLevels.LevelSettings.Length)];
                if (weapon.Name != _firstWeapon && weapon.Name != _secondWeapon)
                    _thirdWeapon = weapon.Name;
            }

            var uiFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
                .Inc<LevelButtonComponent>().End();
            var levelButton = _world.GetPool<LevelButtonComponent>();

            foreach (var entity in uiFilter)
            {
                ref var levelButtons = ref levelButton.Get(entity);
                levelButtons.Text_weapon_1.text = _firstWeapon;
                levelButtons.Text_weapon_2.text = _secondWeapon;
                levelButtons.Text_weapon_3.text = _thirdWeapon;
            }
        }
    }

    public void Run(EcsSystems systems)
    {
        var timeFilter = _world.Filter<GlobalTimeComponent>().End();
        var timePool = _world.GetPool<GlobalTimeComponent>();

        var uiFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var uiPool = _world.GetPool<UIComponent>();
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            if (globalTime.GlobalTime >= globalTime.TimeLimit)
            {
                foreach (var entity in uiFilter)
                {
                    ref var uiPoolComponent = ref uiPool.Get(entity);
                    uiPoolComponent._view_Win.Show();
                    Time.timeScale = 0;
                }
            }
        }
    }
}