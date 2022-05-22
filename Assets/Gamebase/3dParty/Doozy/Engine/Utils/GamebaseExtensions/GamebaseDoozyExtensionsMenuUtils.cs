using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doozy.Engine.Utils
{
    public static class GamebaseDoozyExtensionsMenuUtils
    {
        //GlobalEvent Node
        public const string GlobalEventNode_CreateNodeMenu_Name = "Global Event";
        public const int GlobalEventNode_CreateNodeMenu_Order = 1;
        public const string GlobalEventNode_Manual = "";
        public const string GlobalEventNode_YouTube = "";

        public const string SendGlobalEvents = "Send Global Events";
        public const string SendGlobalEvent = "Send Global Event";

        public const string GlobalEventPropertyName = "GlobalEventType";
        public const string GlobalEventLabelName = "Global Event";

        //Global Event Portal Node
        public const string GlobalEventPortalNode_CreateNodeMenu_Name = "Navigation/Global Event Portal";
        public const int GlobalEventPortalNode_CreateNodeMenu_Order = MenuUtils.DefaultNodeOrder;
        public const string GlobalEventPortalNode_Manual = "";
        public const string GlobalEventPortalNode_YouTube = "";

        public const string GlobalEventPortalPropertyName = "m_globalEventType";
    }
}
