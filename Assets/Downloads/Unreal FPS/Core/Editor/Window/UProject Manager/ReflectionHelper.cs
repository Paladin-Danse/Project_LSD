/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnrealFPS.Editor
{
    public partial class UProjectManager
    {
        /// <summary>
        /// UProject Manager items reflection helper.
        /// </summary>
        internal static class ReflectionHelper
        {
            public readonly static Type attributeType = typeof(UPMItemAttribute);
            public readonly static Assembly assembly = Assembly.GetExecutingAssembly();
            public readonly static Type[] types = assembly.GetTypes();

            public static IEnumerable<Type> GetAttributes()
            {
                IEnumerable<Type> attributes = types.Where(t => t.IsDefined(attributeType));
                return attributes;
            }

            public static ItemProperty[] GetItemProperties()
            {
                IEnumerable<Type> attributes = GetAttributes();
                List<ItemProperty> itemProperties = new List<ItemProperty>();
                foreach (Type item in attributes)
                {
                    UPMItemAttribute _UPMAttribute = (UPMItemAttribute) Attribute.GetCustomAttribute(item, typeof(UPMItemAttribute));

                    if (_UPMAttribute == null)
                    {
                        continue;
                    }

                    MethodInfo onEnable = item.GetMethod("OnEnable");
                    MethodInfo onGUI = item.GetMethod("OnGUI");
                    Action onEnableDelegate = null;
                    Action onGUIDelegate = null;

                    if (onEnable != null && onEnable.IsStatic)
                    {
                        onEnableDelegate = (Action) Delegate.CreateDelegate(typeof(Action), null, onEnable);
                    }

                    if (onGUI != null && onGUI.IsStatic)
                    {
                        onGUIDelegate = (Action) Delegate.CreateDelegate(typeof(Action), null, onGUI);
                    }

                    int id = UnityEngine.Random.Range(1, 9999);
                    while (itemProperties.Any(t => id == t.GetID()))
                    {
                        id = UnityEngine.Random.Range(1, 9999);
                    }

                    itemProperties.Add(new ItemProperty(id, _UPMAttribute.GetIndex(), _UPMAttribute.GetName(), _UPMAttribute.GetItemType(), onEnableDelegate, onGUIDelegate));
                }
                return itemProperties.ToArray();
            }
        }
    }
}