// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor.Internal;
using Doozy.Editor.Nody;
using Doozy.Editor.Nody.Editors;
using Doozy.Engine.Nody.Connections;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.UI.Nodes;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.UI.Nodes
{
    [CustomEditor(typeof(GlobalEventPortalNode))]
    public class GlobalEventPortalNodeEditor : BaseNodeEditor
    {
        private GlobalEventPortalNode TargetNode { get { return (GlobalEventPortalNode)target; } }

        private InfoMessage m_infoMessageUnnamedNodeName,
                            m_infoMessageDuplicateNodeName,
                            m_infoMessageNotListeningForAnyGameEvent;

        private SerializedProperty
            m_anyValue,
            m_globalEventType,
            m_switchBackMode;

        protected override void LoadSerializedProperty()
        {
            base.LoadSerializedProperty();

            m_anyValue = GetProperty(PropertyName.AnyValue);
            m_globalEventType = GetProperty(GamebaseDoozyExtensionsMenuUtils.GlobalEventPortalPropertyName);
            m_switchBackMode = GetProperty(PropertyName.SwitchBackMode);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateNodeName(GetNodeName());
            m_infoMessageUnnamedNodeName = new InfoMessage(InfoMessage.MessageType.Error, UILabels.UnnamedNodeTitle, UILabels.UnnamedNodeMessage);
            m_infoMessageDuplicateNodeName = new InfoMessage(InfoMessage.MessageType.Error, UILabels.DuplicateNodeTitle, UILabels.DuplicateNodeMessage);
            m_infoMessageNotListeningForAnyGameEvent = new InfoMessage(InfoMessage.MessageType.Error, UILabels.NotListeningForAnyGameEventTitle, UILabels.NotListeningForAnyGameEventMessage);

            UpdateSwitchBackModeState(TargetNode.SwitchBackMode);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawHeader(Styles.GetStyle(Styles.StyleName.ComponentHeaderPortalNode), MenuUtils.PortalNode_Manual, MenuUtils.PortalNode_YouTube);
            DrawDebugMode(true);
            GUILayout.Space(DGUI.Properties.Space(2));
            DrawNodeName(false);
            m_infoMessageUnnamedNodeName.Draw(TargetNode.ErrorNodeNameIsEmpty, InspectorWidth);
            m_infoMessageDuplicateNodeName.Draw(TargetNode.ErrorDuplicateNameFoundInGraph, InspectorWidth);
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawSwitchBackMode();
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawOutputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space(16));
            EditorGUI.BeginChangeCheck();
            DrawOptions();
            if (EditorGUI.EndChangeCheck())
                NodeUpdated = true;
            GUILayout.Space(DGUI.Properties.Space(2));
            serializedObject.ApplyModifiedProperties();
            if (NodeUpdated) UpdateNodeName(GetNodeName());
            SendGraphEventNodeUpdated();
        }

        private void DrawSwitchBackMode()
        {
            EditorGUI.BeginChangeCheck();
            DGUI.Toggle.Switch.Draw(m_switchBackMode, UILabels.SwitchBackMode, ComponentColorName, true, false);
            if (!EditorGUI.EndChangeCheck()) return;
            UpdateSwitchBackModeState(m_switchBackMode.boolValue);
        }

        private void UpdateSwitchBackModeState(bool value)
        {
            if (value)
            {
                if (TargetNode.InputSockets == null ||
                    TargetNode.InputSockets.Count == 0)
                    TargetNode.AddInputSocket(ConnectionMode.Multiple, typeof(PassthroughConnection), false, false);
            }
            else
            {
                if (TargetNode.InputSockets != null &&
                    TargetNode.InputSockets.Count > 0)
                {
                    if (TargetNode.FirstInputSocket != null &&
                        TargetNode.FirstInputSocket.IsConnected)
                        GraphEvent.DisconnectSocket(TargetNode.FirstInputSocket);

                    TargetNode.InputSockets.Clear();
                }

                NodeUpdated = true;
            }
        }

        private string GetNodeName()
        {
            return TargetNode.GlobalEventTypeToListenFor + " Global Event";
        }

        private void DrawGameEvent(SerializedProperty property, GUIStyle iconStyle, string label)
        {
            bool hasErrors = TargetNode.ErrorNotListeningForAnyGameEvent;
            ColorName colorName = !hasErrors ? DGUI.Colors.ActionColorName : ColorName.Red;
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    DGUI.Property.Draw(property, "", DGUI.Colors.ActionColorName, hasErrors);
                    if (EditorGUI.EndChangeCheck()) NodeUpdated = true;
                    GUILayout.Space(DGUI.Properties.Space());
                    if (DGUI.Button.Dynamic.DrawIconButton(Styles.GetStyle(Styles.StyleName.IconFaPaste),
                                                           UILabels.Paste,
                                                           Size.S, TextAlign.Left,
                                                           colorName, colorName,
                                                           DGUI.Properties.SingleLineHeight + DGUI.Properties.Space(2), false))
                        property.stringValue = EditorGUIUtility.systemCopyBuffer;
                    GUILayout.Space(DGUI.Properties.Space());
                    bool enabledState = GUI.enabled;
                    GUI.enabled = !hasErrors;
                    if (DGUI.Button.Dynamic.DrawIconButton(Styles.GetStyle(Styles.StyleName.IconFaCopy),
                                                           UILabels.Copy,
                                                           Size.S, TextAlign.Left,
                                                           colorName, colorName,
                                                           DGUI.Properties.SingleLineHeight + DGUI.Properties.Space(2), false))
                    {
                        EditorGUIUtility.systemCopyBuffer = property.stringValue;
                        Debug.Log(GamebaseDoozyExtensionsMenuUtils.GlobalEventLabelName + " '" + property.stringValue + "' " + UILabels.HasBeenAddedToClipboard);
                    }

                    GUI.enabled = enabledState;
                }
                GUILayout.EndHorizontal();

                m_infoMessageNotListeningForAnyGameEvent.Draw(TargetNode.ErrorNotListeningForAnyGameEvent, InspectorWidth);
            }
            GUILayout.EndVertical();
        }

        private void DrawOptions()
        {
            ColorName backgroundColorName = DGUI.Colors.ActionColorName;
            ColorName textColorName = DGUI.Colors.ActionColorName;
            DrawBigTitleWithBackground(Styles.GetStyle(Styles.StyleName.IconFaEar), UILabels.GlobalListener, backgroundColorName, textColorName);
            GUILayout.Space(DGUI.Properties.Space(2));
            GUILayout.Space(DGUI.Properties.Space());
            EditorGUILayout.BeginHorizontal();
            DrawGameEvent(m_globalEventType, Styles.GetStyle(Styles.StyleName.IconGameEvent), UILabels.GameEvent);
            EditorGUILayout.EndHorizontal();
        }
    }
}