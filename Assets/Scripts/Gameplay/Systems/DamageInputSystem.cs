using LeoEcsPhysics;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class DamageInputSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private GameObject _enemyCollisionIn;
    private GameObject _enemyCollisionOut;
    private GameObject _playerCollision;
    private GameObject _projectileCollision;
    private GameObject _experienceCollision;
    private GameObject _sender;
    private GameObject _collision;
    private float _enemyDamage;
    private float _playerDamage;
    private float _experience;
    private Slider _hpBar;
    private TextMeshProUGUI _textCountKills;
    private int _countKill;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var uiFilter = _world.Filter<MainUIComponent>().End();
        var uiPool = _world.GetPool<MainUIComponent>();

        foreach (var entity in uiFilter)
        {
            ref var uiComponent = ref uiPool.Get(entity);
            _hpBar = uiComponent.HPbar;
            _textCountKills = uiComponent.Text_countKill;
        }
    }

    public void Run(EcsSystems systems)
    {
        var filterEnter = _world.Filter<OnTriggerEnterEvent>().End();
        var poolEnter = _world.GetPool<OnTriggerEnterEvent>();
        
        var projectileFilter = _world.Filter<ProjectileTag>().Inc<ModelComponent>()
            .Inc<WeaponComponent>().End();
        var enemyFilter = _world.Filter<EnemyTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().End();
        var playerFilter = _world.Filter<PlayerTag>().Inc<ModelComponent>()
            .Inc<PlayerParameterComponent>().End();
        var experienceFilter = _world.Filter<ExperienceComponent>().Inc<ModelComponent>().End();
        
        var unitPool = _world.GetPool<ModelComponent>();
        var weaponPool = _world.GetPool<WeaponComponent>();
        var paramPool = _world.GetPool<ParameterComponent>();
        var playerParamPool = _world.GetPool<PlayerParameterComponent>();
        var experiencePool = _world.GetPool<ExperienceComponent>();
        
        var filterUI = _world.Filter<UIComponent>().End();
        var poolUI = _world.GetPool<UIComponent>();
        
        foreach (var entity in filterEnter)
        {
            ref var eventData = ref poolEnter.Get(entity);
            _sender = eventData.senderGameObject;
            foreach (var entityProj in projectileFilter)
            {
                ref var projectile = ref unitPool.Get(entityProj);
                ref var projectileConfig = ref weaponPool.Get(entityProj);
                _playerDamage = projectileConfig.Weapons[0].Damage * projectileConfig.Weapons[0].Level;
                _projectileCollision = projectile.modelTransform.gameObject;
                if (_projectileCollision == _sender)
                {
                    _enemyCollisionIn = eventData.collider.gameObject;
                    break;
                }
            }
            foreach (var entityEnemy in enemyFilter)
            {
                ref var enemy = ref unitPool.Get(entityEnemy);
                ref var enemyConfig = ref paramPool.Get(entityEnemy);
                _enemyDamage = enemyConfig.Config.Damage;
                if (enemy.modelTransform.gameObject == _sender)
                {
                    _enemyCollisionOut = enemy.modelTransform.gameObject;
                    _playerCollision = eventData.collider.gameObject;
                    break;
                }
            }
            foreach (var entityExperience in experienceFilter)
            {
                ref var experience = ref unitPool.Get(entityExperience);
                ref var experienceConfig = ref experiencePool.Get(entityExperience);
                _experience = experienceConfig.CristalConfig.Experience;
                if (experience.modelTransform.gameObject == _sender)
                {
                    _playerCollision = eventData.collider.gameObject;
                    _experienceCollision = _sender;
                    _enemyCollisionOut = null;
                    break;
                }
            }
            poolEnter.Del(entity);
        }

        foreach (var entity in enemyFilter)
        {
            ref var enemy = ref unitPool.Get(entity);
            ref var enemyParam = ref paramPool.Get(entity);
            if (enemy.modelTransform.gameObject == _enemyCollisionIn)
            {
                Debug.Log("damage to enemy");
                if (enemyParam.HP == 0)
                {
                    enemyParam.HP = enemyParam.Config.HP;
                }
                enemyParam.HP -= _playerDamage;
                if (enemyParam.HP <= 0)
                {
                    _countKill++;
                    _textCountKills.text = _countKill + "";
                    _enemyCollisionIn.SetActive(false);
                    Object.Instantiate(enemyParam.Config.ExperienceCrystal, _enemyCollisionIn.transform.position,
                        _enemyCollisionIn.transform.rotation);
                }
                _projectileCollision.SetActive(false);
                _enemyCollisionIn = null;
                _projectileCollision = null;
                break;
            }
        }
        
        foreach (var entity in playerFilter)
        {
            ref var player = ref unitPool.Get(entity);
            ref var playerConfig = ref playerParamPool.Get(entity);
            if (player.modelTransform.gameObject == _playerCollision && _enemyCollisionOut)
            {
                Debug.Log("damage to player");
                if (playerConfig.HP == 0)
                {
                    playerConfig.HP = playerConfig.Config.HP;
                }
                playerConfig.HP -= _enemyDamage;
                _hpBar.value = playerConfig.HP / playerConfig.Config.HP;
                if (playerConfig.HP <= 0)
                {
                    Time.timeScale = 0;
                    foreach (var entityUI in filterUI)
                    {
                        ref var uIpool = ref poolUI.Get(entityUI);
                        uIpool._view_Lose.Show();
                    }
                }
                _playerCollision = null;
                _enemyCollisionOut = null;
                break;
            }
            
            if(player.modelTransform.gameObject == _playerCollision && _experienceCollision)
            {
                Debug.Log("EXP to player");
                _experienceCollision.SetActive(false);
                playerConfig.Config.Experience += _experience;
                _playerCollision = null;
                _experienceCollision = null;
                break;
            }
        }
    }
}