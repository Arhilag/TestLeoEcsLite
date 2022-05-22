using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public static class GamebaseBuildTargetGroups
    {
        public static IEnumerable<BuildTargetGroup> GetAllBuildTargetGroups()
        {
            var enumType = typeof(BuildTargetGroup);
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var value = (BuildTargetGroup)values.GetValue(i);

                if (value == BuildTargetGroup.Unknown) continue;

                var member = enumType.GetMember(name);
                var entry = member.FirstOrDefault(p => p.DeclaringType == enumType);

                if (entry == null) continue;

                if (entry.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length != 0)
                {
                    // obsolete, ignore.
                    continue;
                }

                yield return value;
            }
        }
    }
}