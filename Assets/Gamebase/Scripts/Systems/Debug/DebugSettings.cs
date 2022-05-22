using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Gamebase;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable RedundantDefaultMemberInitializer

/// <summary>
/// Содержит настройки отладки
/// </summary>
public class DebugSettings : StaticScriptableObject<DebugSettings>
{
    [Title("Настройки системы логирования")]
    
    [InfoBox("Система для централизованного управления отладкой. В настройках системы можно установить фильтры для сообщений которые можно пропускать, или которые нужно игнорировать.", InfoMessageType.None)]
    [Tooltip("Включить логгирование")]
    public bool enableConsole = true;
    
    [InfoBox("В логи будут попадать сообщения систем геймбейза, если включить данную галочку (это не относится к варнингам и ошибкам, они будут выводиться в любом случае)")]
    public bool enableGamebaseMessages = false;
    
    [InfoBox("В логи будут попадать только те сообщения, которые содержат что-то из списка ключевых слов, убедитесь, что нужные ключевые слова отмечены как активные.", visibleIfMemberName: "enableFilter", infoMessageType: InfoMessageType.Warning)]
    [Tooltip("Фильтровать ли сообщения (пропускать только те, что содержат что-то из списка ключевых слов)")]
    public bool enableFilter;
    
    [ShowIf("enableFilter"), Tooltip("Ключевые слова, сообщения которые их содержат будут пропущены в логи")]
    public List<ConsoleModeElement> keywords;
    
    [InfoBox("В логи НЕ будут попадать те сообщения, которые содержат что-то из списка слов-исключений", visibleIfMemberName: "enableExceptFilter", infoMessageType: InfoMessageType.Warning)]
    [Tooltip("Фильтровать ли сообщения (не пропускать те, что содержат что-то из списка слов-исключений)")]
    public bool enableExceptFilter;
    
    [ShowIf("enableExceptFilter"), Tooltip("Сообщения с какими словами необходимо пропускать и не записывать в логи")]
    public List<string> consoleExceptWords;
}

[Serializable, InlineProperty]
public class ConsoleModeElement
{
    [Tooltip("Использовать ли данное ключевое слово в текущем фильтре")]
    [HorizontalGroup(width: 50f), HideLabel]
    public bool enabled;
    
    [Tooltip("Ключевое слово (с учётом регистра)")]
    [HorizontalGroup, HideLabel]
    public string key;
}