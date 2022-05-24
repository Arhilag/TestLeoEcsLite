using LeoEcsPhysics;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class DamageInputSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
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
        world = systems.GetWorld ();
        var uifilter = world.Filter<MainUIComponent>().End();
        var uipool = world.GetPool<MainUIComponent>();

        foreach (var entity in uifilter)
        {
            ref var uiComponent = ref uipool.Get(entity);
            _hpBar = uiComponent.HPbar;
            _textCountKills = uiComponent.Text_countKill;
        }

    }

    public void Run(EcsSystems systems)
    {
        var filterEnter = world.Filter<OnTriggerEnterEvent>().End();
        var poolEnter = world.GetPool<OnTriggerEnterEvent>();
        
        var projectileFilter = world.Filter<ProjectileTag>().Inc<ModelComponent>()
            .Inc<WeaponComponent>().End();
        var enemyFilter = world.Filter<EnemyTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<ModelComponent>()
            .Inc<PlayerParameterComponent>().End();
        var experienceFilter = world.Filter<ExperienceComponent>().Inc<ModelComponent>().End();
        
        var unitPool = world.GetPool<ModelComponent>();
        var weaponPool = world.GetPool<WeaponComponent>();
        var paramPool = world.GetPool<ParameterComponent>();
        var playerparamPool = world.GetPool<PlayerParameterComponent>();
        var experiencePool = world.GetPool<ExperienceComponent>();
        
        var filterUI = world.Filter<UIComponent>().End();
        var poolUI = world.GetPool<UIComponent>();
        
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
            ref var playerConfig = ref playerparamPool.Get(entity);
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
                        ref var UIpool = ref poolUI.Get(entityUI);
                        UIpool._view_Lose.Show();
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