// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using Doozy.Engine.Nody.Attributes;
using Doozy.Engine.Nody.Connections;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.Utils;
using Gamebase;
using UnityEngine;

namespace Doozy.Engine.UI.Nodes
{
    /// <summary>
    ///    Global node that listens for a set game event (string value). When triggered it jumps instantly to the next node in the Graph.
    ///    A global node is active as long as its parent Graph is active.
    ///    This particular node allows for jumping from one part of the UI flow to another, without the need of a direct connection.
    ///    Due to the way it works, this node can also be considered as a 'virtual connection' between multiple active Graphs.
    /// </summary>
    [NodeMenu(GamebaseDoozyExtensionsMenuUtils.GlobalEventPortalNode_CreateNodeMenu_Name, GamebaseDoozyExtensionsMenuUtils.GlobalEventPortalNode_CreateNodeMenu_Order)]
    public class GlobalEventPortalNode : Node
    {
        private static GlobalEventsSystem GlobalEventsSystem => GamebaseSystems.Instance.GlobalEventsSystem;

#if UNITY_EDITOR
        public override bool HasErrors => base.HasErrors;
        public bool ErrorNotListeningForAnyGameEvent;
#endif

        private const bool DEFAULT_ANY_VALUE = false;
        
        [SerializeField] private GlobalEventType m_globalEventType = GlobalEventType.None;
        public GlobalEventType GlobalEventTypeToListenFor { get { return m_globalEventType; } }

        [NonSerialized] private Graph m_portalGraph;
        public Graph PortalGraph { get { return m_portalGraph; } set { m_portalGraph = value; } }

        public bool AnyValue = DEFAULT_ANY_VALUE;
        public bool SwitchBackMode = false;

        private Node m_sourceNode;
        private bool m_activatedByEvent;
        public bool HasSource { get { return m_sourceNode != null; } }
        public Node Source { get { return m_sourceNode; } }

        public string WaitForInfoTitle => GamebaseDoozyExtensionsMenuUtils.GlobalEventLabelName;

        public string WaitForInfoDescription
        {
            get
            {
                return AnyValue
                    ? UILabels.AnyGameEvent
                    : GlobalEventTypeToListenFor == GlobalEventType.None
                        ? "---"
                        : GlobalEventTypeToListenFor.ToString();
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CanBeDeleted = true;
            SetNodeType(NodeType.Global);
            SetName(UILabels.PortalNodeName);
        }

        public override void AddDefaultSockets()
        {
            base.AddDefaultSockets();
//            AddInputSocket(ConnectionMode.Multiple, typeof(PassthroughConnection), false, false);
            AddOutputSocket(ConnectionMode.Override, typeof(PassthroughConnection), false, false);
        }

        private void AddListeners()
        {
            GlobalEventsSystem.Subscribe(GlobalEventTypeToListenFor,OnGlobalEventMessage);
        }

        private void RemoveListeners()
        {
            GlobalEventsSystem.Unsubscribe(GlobalEventTypeToListenFor,OnGlobalEventMessage);
        }

        public override void Activate(Graph portalGraph)
        {
            if (m_activated) return;
            base.Activate(portalGraph);
            PortalGraph = portalGraph;
            AddListeners();
        }

        public override void Deactivate()
        {
            if (!m_activated) return;
            base.Deactivate();
            RemoveListeners();
        }

        private void UpdateSourceNode(Node node)
        {
            if (!SwitchBackMode) return;
            m_sourceNode = node;
        }


        private void OnGlobalEventMessage()
        {
            if (PortalGraph != null && !PortalGraph.Enabled) return;
            m_activatedByEvent = true;
            PortalGraph.SetActiveNodeById(Id);
        }

        public override void CopyNode(Node original)
        {
            base.CopyNode(original);
            var node = (GlobalEventPortalNode) original;
            m_globalEventType = node.m_globalEventType;
            AnyValue = node.AnyValue;
            SwitchBackMode = node.SwitchBackMode;
        }

        public override void OnEnter(Node previousActiveNode, Connection connection)
        {
            base.OnEnter(previousActiveNode, connection);
            if (ActiveGraph == null) return;
            if (!FirstOutputSocket.IsConnected) return;

            if (!SwitchBackMode) //Switch Back Node disabled -> Activate the first connected node
            {
                PortalGraph.SetActiveNodeByConnection(FirstOutputSocket.FirstConnection);
                return;
            }

            if (!m_activatedByEvent && HasSource) //node activated by a direct connection and has a source -> go back
            {
                PortalGraph.SetActiveNodeById(m_sourceNode.Id);
                m_sourceNode = null; //reset source after going back
                return;
            }

            UpdateSourceNode(m_activatedByEvent ? previousActiveNode : null); //update the source to the previously active node if activated by an event
            PortalGraph.SetActiveNodeByConnection(FirstOutputSocket.FirstConnection);
        }

        public override void OnExit(Node nextActiveNode, Connection connection)
        {
            base.OnExit(nextActiveNode, connection);
            m_activatedByEvent = false;
        }

        public override void CheckForErrors()
        {
            base.CheckForErrors();
#if UNITY_EDITOR
            ErrorNotListeningForAnyGameEvent = GlobalEventTypeToListenFor == GlobalEventType.None;
#endif
        }
    }
}