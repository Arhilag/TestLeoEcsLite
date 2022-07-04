using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

sealed class LevelSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    readonly EcsFilterInject<Inc<PlayerTag,
        WeaponComponent>> _playerFilter = default;
    readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
    
    readonly EcsFilterInject<Inc<ButtonComponent, UIWeapon1Component>> _buttonWeapon1Filter = default;
    readonly EcsFilterInject<Inc<ButtonComponent, UIWeapon2Component>> _buttonWeapon2Filter = default;
    readonly EcsFilterInject<Inc<ButtonComponent, UIWeapon3Component>> _buttonWeapon3Filter = default;
    readonly EcsPoolInject<ButtonComponent> _buttonWeaponPool = default;
    
    readonly EcsFilterInject<Inc<TextComponent, UIWeapon1Component>> _textWeapon1Filter = default;
    readonly EcsFilterInject<Inc<TextComponent, UIWeapon2Component>> _textWeapon2Filter = default;
    readonly EcsFilterInject<Inc<TextComponent, UIWeapon3Component>> _textWeapon3Filter = default;
    readonly EcsPoolInject<TextComponent> _textWeaponPool = default;
    
    readonly EcsFilterInject<Inc<ViewComponent,
    UILevelUpComponent>> _uiLevelUpFilter = default;
    readonly EcsPoolInject<ViewComponent> _uiLevelUpPool = default;
    
    readonly EcsFilterInject<Inc<ViewComponent,
        UIWinComponent>> _uiWinFilter = default;
    readonly EcsPoolInject<ViewComponent> _uiWinPool = default;
    
    readonly EcsFilterInject<Inc<GlobalTimeComponent>> _timeFilter = default;
    readonly EcsPoolInject<GlobalTimeComponent> _timePool = default;
    
    readonly EcsFilterInject<Inc<IconComponent,
    CubeComponent>> _cubeIconFilter = default;
    readonly EcsFilterInject<Inc<IconComponent,
        BallComponent>> _ballIconFilter = default;
    readonly EcsFilterInject<Inc<IconComponent,
        ThreeComponent>> _threeIconFilter = default;
    readonly EcsFilterInject<Inc<IconComponent,
        TComponent>> _tIconFilter = default;
    readonly EcsPoolInject<IconComponent> _iconPool = default;

    private string _firstWeapon;
    private string _secondWeapon;
    private string _thirdWeapon;
    private WeaponComponent _playerWeapons;
    
    public void Init(EcsSystems systems)
    {
        LevelUISystem.OnLevelUp += LevelUp;

        var playerFilter = _playerFilter.Value;
        var weaponPool = _weaponPool.Value;

        var buttonWeapon1Filter = _buttonWeapon1Filter.Value;
        var buttonWeapon2Filter = _buttonWeapon2Filter.Value;
        var buttonWeapon3Filter = _buttonWeapon3Filter.Value;
        var buttonWeaponPool = _buttonWeaponPool.Value;

        foreach (var entity in playerFilter)
        {
            _playerWeapons = weaponPool.Get(entity);
            _playerWeapons.LevelSettings[0].Level = 1;
            for (var i = 1; i < _playerWeapons.LevelSettings.Length; i++)
            {
                _playerWeapons.LevelSettings[i].Level = 0;
            }
        }

        foreach (var entity in buttonWeapon1Filter)
        {
            ref var levelButtons = ref buttonWeaponPool.Get(entity);
            levelButtons.Button.onClick.AddListener(ClickOne);
        }
        foreach (var entity in buttonWeapon2Filter)
        {
            ref var levelButtons = ref buttonWeaponPool.Get(entity);
            levelButtons.Button.onClick.AddListener(ClickTwo);
        }
        foreach (var entity in buttonWeapon3Filter)
        {
            ref var levelButtons = ref buttonWeaponPool.Get(entity);
            levelButtons.Button.onClick.AddListener(ClickThree);
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
        var playerFilter = _playerFilter.Value;
        var weaponPool = _weaponPool.Value;
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
        Image[] icons = new Image[4];
        var cubeIconFilter = _cubeIconFilter.Value;
        var ballIconFilter = _ballIconFilter.Value;
        var threeIconFilter = _threeIconFilter.Value;
        var tIconFilter = _tIconFilter.Value;
        var iconPool = _iconPool.Value;
        foreach (var n in cubeIconFilter)
        {
            ref var icon = ref iconPool.Get(n);
            icons[0] = icon.Icon;
        }
        foreach (var j in ballIconFilter)
        {
            ref var icon = ref iconPool.Get(j);
            icons[1] = icon.Icon;
        }
        foreach (var k in threeIconFilter)
        {
            ref var icon = ref iconPool.Get(k);
            icons[2] = icon.Icon;
        }
        foreach (var m in tIconFilter)
        {
            ref var icon = ref iconPool.Get(m);
            icons[3] = icon.Icon;
        }
        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(false);
        }
        for (var i = 0; i < _playerWeapons.LevelSettings.Length; i++)
        {
            Debug.Log(_playerWeapons.LevelSettings[i].Level);
            if (_playerWeapons.LevelSettings[i].Level > 0)
            {
                icons[i].gameObject.SetActive(true);
            }
        }
    }

    public void LevelUp()
    {
        var playerFilter = _playerFilter.Value;
        var weaponPool = _weaponPool.Value;

        var uiLevelUpFilter = _uiLevelUpFilter.Value;
        var uiLevelUpPool = _uiLevelUpPool.Value;
        
        var textWeapon1Filter = _textWeapon1Filter.Value;
        var textWeapon2Filter = _textWeapon2Filter.Value;
        var textWeapon3Filter = _textWeapon3Filter.Value;
        var levelButtonPool = _textWeaponPool.Value;
        
        foreach (var i in playerFilter)
        {
            ref var weaponLevels = ref  weaponPool.Get(i);
            foreach (var entity in uiLevelUpFilter)
            {
                ref var uiViewsComponent = ref  uiLevelUpPool.Get(entity);
                uiViewsComponent.View.Show();
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

            foreach (var entity in textWeapon1Filter)
            {
                ref var levelButtons = ref levelButtonPool.Get(entity);
                levelButtons.Text.text = _firstWeapon;
            }
            foreach (var entity in textWeapon2Filter)
            {
                ref var levelButtons = ref levelButtonPool.Get(entity);
                levelButtons.Text.text = _secondWeapon;
            }
            foreach (var entity in textWeapon3Filter)
            {
                ref var levelButtons = ref levelButtonPool.Get(entity);
                levelButtons.Text.text = _thirdWeapon;
            }
        }
    }

    public void Run(EcsSystems systems)
    {
        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;

        var uiWinFilter = _uiWinFilter.Value;
        var uiWinPool = _uiWinPool.Value;
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            if (globalTime.GlobalTime >= globalTime.TimeLimit)
            {
                foreach (var entity in uiWinFilter)
                {
                    ref var uiPoolComponent = ref uiWinPool.Get(entity);
                    uiPoolComponent.View.Show();
                    Time.timeScale = 0;
                }
            }
        }
    }
}