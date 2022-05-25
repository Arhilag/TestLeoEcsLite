using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class LevelSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private PlayerParameterComponent _playerConfig;
    private LevelConfig _levelConfig;
    private TextMeshProUGUI _textTime;
    private TextMeshProUGUI _textLvl;
    private Slider _slider;
    private UIComponent _views;
    private LevelButtonComponent _levelButtons;
    private ExperienceConfig _experienceConfig;
    private int _lvlNumber;
    private WeaponConfig[] _weapons;
    private WeaponConfig _firstWeapon;
    private WeaponConfig _secondWeapon;
    private WeaponConfig _thirdWeapon;
    private WeaponComponent _playerWeapons;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        
        var playerFilter = _world.Filter<PlayerTag>().Inc<PlayerParameterComponent>()
            .Inc<PlayerExperienceComponent>().Inc<WeaponComponent>().End();
        var parameterpool = _world.GetPool<PlayerParameterComponent>();
        var experiencepool = _world.GetPool<PlayerExperienceComponent>();
        var eaponPool = _world.GetPool<WeaponComponent>();
        
        var UIFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var UIpool = _world.GetPool<MainUIComponent>();
        var UIviews = _world.GetPool<UIComponent>();
        var LevelButton = _world.GetPool<LevelButtonComponent>();
        
        var LevelFilter = _world.Filter<LevelSettingComponent>().End();
        var Levelpool = _world.GetPool<LevelSettingComponent>();
        
        foreach (var entity in playerFilter)
        {
            _playerConfig = parameterpool.Get(entity);
            ref var experience = ref experiencepool.Get(entity);
            _playerWeapons = eaponPool.Get(entity);
            _playerWeapons.Weapons[0].Level = 1;
            for (var i = 1; i < _playerWeapons.Weapons.Length; i++)
            {
                _playerWeapons.Weapons[i].Level = 0;
            }
            _experienceConfig = experience.ExperienceSetting;
        }

        _lvlNumber = 0;
        foreach (var entity in UIFilter)
        {
            ref var ui = ref UIpool.Get(entity);
            _views = UIviews.Get(entity);
            _levelButtons = LevelButton.Get(entity);
            _textTime = ui.Text_time;
            _textLvl = ui.Text_level;
            _slider = ui.Levelbar;
        }
        
        foreach (var entity in LevelFilter)
        {
            ref var settingConfig = ref Levelpool.Get(entity);
            _levelConfig = settingConfig.Setting;
            _weapons = settingConfig.Weapons;
        }

        _levelConfig.GlobalTime = 0;
        _playerConfig.Config.Experience = 0;
        
        _levelButtons.Button_weapon_1.onClick.AddListener(ClickOne);
        _levelButtons.Button_weapon_2.onClick.AddListener(ClickTwo);
        _levelButtons.Button_weapon_3.onClick.AddListener(ClickThree);
    }
    
    public void Run(EcsSystems systems)
    {
        _levelConfig.GlobalTime += Time.deltaTime;
        var minute = (int) _levelConfig.GlobalTime / 60;
        _textTime.text = minute + ":" + ((int)_levelConfig.GlobalTime-minute*60);
        _textLvl.text = _lvlNumber + "";
        _slider.value = _playerConfig.Config.Experience / _experienceConfig.ExperienceToUp[_lvlNumber];
        if (_levelConfig.GlobalTime >= _levelConfig.TimeLimit)
        {
            _views._view_Win.Show();
            Time.timeScale = 0;
        }

        if (_lvlNumber < _experienceConfig.ExperienceToUp.Length-1)
        {
            if (_playerConfig.Config.Experience > _experienceConfig.ExperienceToUp[_lvlNumber])
            {
                _lvlNumber++;
                _playerConfig.Config.Experience = 0;
                _views._view_LevelUp.Show();
                Time.timeScale = 0;
                
                foreach (var weapon in _weapons)
                {
                    if (weapon.Level > 0)
                    {
                        _firstWeapon = weapon;
                        break;
                    }
                }
                while (!_secondWeapon)
                {
                    var weapon = _weapons[Random.Range(0, _weapons.Length)];
                    if (weapon != _firstWeapon)
                        _secondWeapon = weapon;
                }
                while (!_thirdWeapon)
                {
                    var weapon = _weapons[Random.Range(0, _weapons.Length)];
                    if (weapon != _firstWeapon && weapon != _secondWeapon)
                        _thirdWeapon = weapon;
                }

                _levelButtons.Text_weapon_1.text = _firstWeapon.name;
                _levelButtons.Text_weapon_2.text = _secondWeapon.name;
                _levelButtons.Text_weapon_3.text = _thirdWeapon.name;
            }
        }
    }

    private void ClickOne()
    {
        _firstWeapon.Level++;
    }
    private void ClickTwo()
    {
        _secondWeapon.Level++;
    }
    private void ClickThree()
    {
        _thirdWeapon.Level++;
    }
}