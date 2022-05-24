using Doozy.Engine.UI;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class LevelSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private PlayerParameterComponent _playerConfig;
    private float _globalTime;
    private float _timeLimit;
    private TextMeshProUGUI _text_time;
    private TextMeshProUGUI _text_lvl;
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
        EcsWorld _world = systems.GetWorld ();
        
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
            _experienceConfig = experience.ExperienceSetting;
        }

        _lvlNumber = 0;
        _globalTime = 0;
        foreach (var entity in UIFilter)
        {
            ref var UI = ref UIpool.Get(entity);
            _views = UIviews.Get(entity);
            _levelButtons = LevelButton.Get(entity);
            _text_time = UI.Text_time;
            _text_lvl = UI.Text_level;
            _slider = UI.Levelbar;
        }
        
        foreach (var entity in LevelFilter)
        {
            ref var settingConfig = ref Levelpool.Get(entity);
            _timeLimit = settingConfig.Setting.TimeLimit;
            _weapons = settingConfig.Weapons;
        }

        _playerConfig.Config.Experience = 0;
        
        _levelButtons.Button_weapon_1.onClick.AddListener(ClickOne);
        _levelButtons.Button_weapon_2.onClick.AddListener(ClickTwo);
        _levelButtons.Button_weapon_3.onClick.AddListener(ClickThree);
    }
    
    public void Run(EcsSystems systems)
    {
        _globalTime += Time.deltaTime;
        var minute = (int) _globalTime / 60;
        _text_time.text = minute + ":" + ((int)_globalTime-minute*60);
        _text_lvl.text = _lvlNumber + "";
        if (_lvlNumber == 0)
        {
            _slider.value = _playerConfig.Config.Experience / _experienceConfig.ExperienceToUp[_lvlNumber];
        }
        else
        {
            _slider.value = (_playerConfig.Config.Experience - _experienceConfig.ExperienceToUp[_lvlNumber-1]) 
                / _experienceConfig.ExperienceToUp[_lvlNumber];
        }
        if (minute >= _timeLimit)
        {
            _views._view_Win.Show();
            Time.timeScale = 0;
        }

        if (_lvlNumber < _experienceConfig.ExperienceToUp.Length)
        {
            if (_playerConfig.Config.Experience > _experienceConfig.ExperienceToUp[_lvlNumber])
            {
                _lvlNumber++;
                _views._view_LevelUp.Show();
                Time.timeScale = 0;
                
                Debug.Log(_playerWeapons.Weapons.Length);
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