// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor;
using Doozy.Editor.Internal;
using Doozy.Editor.Nody.Editors;
using Doozy.Engine.Extensions;
using Doozy.Engine.UI.Nodes.GamebaseExtensions;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.UI.Nodes.GamebaseExtensions
{
    [CustomEditor(typeof(GlobalEventNode))]
    public class GlobalEventNodeEditor : BaseNodeEditor
    {
        private GlobalEventNode TargetNode { get { return (GlobalEventNode) target; } }

        private InfoMessage m_infoMessageUnnamedNodeName,
                            m_infoMessageDuplicateNodeName,
                            m_infoMessageNotSendingAnyGameEvent;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_infoMessageUnnamedNodeName = new InfoMessage(InfoMessage.MessageType.Error, UILabels.UnnamedNodeTitle, UILabels.UnnamedNodeMessage);
            m_infoMessageDuplicateNodeName = new InfoMessage(InfoMessage.MessageType.Error, UILabels.DuplicateNodeTitle, UILabels.DuplicateNodeMessage);
            m_infoMessageNotSendingAnyGameEvent = new InfoMessage(InfoMessage.MessageType.Error, UILabels.NotSendingAnyGameEventTitle, UILabels.NotSendingAnyGameEventMessage);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            DrawHeader(Styles.GetStyle(Styles.StyleName.ComponentHeaderGameEventNode), GamebaseDoozyExtensionsMenuUtils.GlobalEventNode_Manual, GamebaseDoozyExtensionsMenuUtils.GlobalEventNode_YouTube);
            DrawDebugMode(true);
            GUILayout.Space(DGUI.Properties.Space(2));
            DrawNodeName();
            GUI.enabled = !TargetNode.ErrorNotSendingAnyGameEvent;
            GUILayout.Space(DGUI.Properties.Space());
            DrawRenameButton(TargetNode.GlobalEventType.ToString());
            GUI.enabled = true;
            m_infoMessageUnnamedNodeName.Draw(TargetNode.ErrorNodeNameIsEmpty, InspectorWidth);
            m_infoMessageDuplicateNodeName.Draw(TargetNode.ErrorDuplicateNameFoundInGraph, InspectorWidth);
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawInputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawOutputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space(16));
            DrawGlobalEvent(GetProperty(GamebaseDoozyExtensionsMenuUtils.GlobalEventPropertyName), Styles.GetStyle(Styles.StyleName.IconGameEvent), GamebaseDoozyExtensionsMenuUtils.SendGlobalEvents);
            GUILayout.Space(DGUI.Properties.Space(2));
            serializedObject.ApplyModifiedProperties();
            SendGraphEventNodeUpdated();
        }

        private void DrawGlobalEvent(SerializedProperty property, GUIStyle iconStyle, string label)
        {
            bool hasErrors = TargetNode.ErrorNotSendingAnyGameEvent;
            ColorName colorName = !hasErrors ? DGUI.Colors.ActionColorName : ColorName.Red;
            GUILayout.BeginVertical();
            {
                GUILayout.Space(DGUI.Properties.Space(2));
                float alpha = GUI.color.a;
                GUI.color = GUI.color.WithAlpha(DGUI.Properties.TextIconAlphaValue(hasErrors));
                DrawSmallTitle(iconStyle, label, colorName);
                GUI.color = GUI.color.WithAlpha(alpha);

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
                    {
                        property.stringValue = EditorGUIUtility.systemCopyBuffer;
                        NodeUpdated = true;
                    }

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
                        Debug.Log(UILabels.GameEvent + " '" + property.stringValue + "' " + UILabels.HasBeenAddedToClipboard);
                    }

                    GUI.enabled = enabledState;
                }
                GUILayout.EndHorizontal();

                m_infoMessageNotSendingAnyGameEvent.Draw(TargetNode.ErrorNotSendingAnyGameEvent, InspectorWidth);
            }
            GUILayout.EndVertical();
        }
    }
}