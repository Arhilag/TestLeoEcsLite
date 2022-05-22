// Copyright (c) 2015 - 2020 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Engine.Nody.Attributes;
using Doozy.Engine.Nody.Connections;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.Utils;
using Gamebase;

namespace Doozy.Engine.UI.Nodes.GamebaseExtensions
{
    /// <summary>
    ///     Sends a Game Event (string value) and jumps instantly to the next node in the Graph.
    ///     <para />
    ///     The next node in the Graph is the one connected to this node’s output socket.
    /// </summary>
    [NodeMenu(GamebaseDoozyExtensionsMenuUtils.GlobalEventNode_CreateNodeMenu_Name, GamebaseDoozyExtensionsMenuUtils.GlobalEventNode_CreateNodeMenu_Order)]
    public class GlobalEventNode : Node
    {
        private static GlobalEventsSystem GlobalEventsSystem => GamebaseSystems.Instance.GlobalEventsSystem;
        
#if UNITY_EDITOR
        public override bool HasErrors { get { return base.HasErrors || ErrorNotSendingAnyGameEvent; } }
        public bool ErrorNotSendingAnyGameEvent;
#endif
        
        public GlobalEventType GlobalEventType;

        public override void OnCreate()
        {
            base.OnCreate();
            CanBeDeleted = true;
            SetNodeType(NodeType.General);
            SetName(GamebaseDoozyExtensionsMenuUtils.SendGlobalEvents);
        }

        public override void AddDefaultSockets()
        {
            base.AddDefaultSockets();
            AddInputSocket(ConnectionMode.Multiple, typeof(PassthroughConnection), false, false);
            AddOutputSocket(ConnectionMode.Override, typeof(PassthroughConnection), false, false);
        }

        public override void CopyNode(Node original)
        {
            base.CopyNode(original);
            var node = (GlobalEventNode) original;
            GlobalEventType = node.GlobalEventType;
        }

        public override void OnEnter(Node previousActiveNode, Connection connection)
        {
            base.OnEnter(previousActiveNode, connection);
            if (ActiveGraph == null) return;
            SendGlobalEvent();
            if (!FirstOutputSocket.IsConnected) return;
            ActiveGraph.SetActiveNodeByConnection(FirstOutputSocket.FirstConnection);
        }

        public override void CheckForErrors()
        {
            base.CheckForErrors();
#if UNITY_EDITOR
            ErrorNotSendingAnyGameEvent = GlobalEventType == GlobalEventType.None;
#endif
        }

        private void SendGlobalEvent()
        {
            if (GlobalEventType == GlobalEventType.None) return;
            GlobalEventsSystem.Invoke(GlobalEventType);
        }
    }
}