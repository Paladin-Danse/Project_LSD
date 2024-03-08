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
    public partial class ProjectSetupAssistant
    {
        /// <summary>
        /// Project Setup Assistant reflection helper.
        /// </summary>
        internal static class ReflectionHelper
        {
            public readonly static Type attributeType = typeof(PSAItemAttribute);
            public readonly static Assembly assembly = Assembly.GetExecutingAssembly();
            public readonly static Type[] types = assembly.GetTypes();

            public static IEnumerable<Type> GetAttributes()
            {
                IEnumerable<Type> attributes = types.Where(t => t.IsDefined(attributeType));
                return attributes;
            }

            /// <summary>
            /// Sorted item properties array.
            /// </summary>
            /// <returns>ItemProperties[] array.</returns>
            public static ItemProperty[] GetItemProperties()
            {
                IEnumerable<Type> attributes = ReflectionHelper.GetAttributes();
                List<ItemProperty> itemProperties = new List<ItemProperty>();
                foreach (Type item in attributes)
                {
                    PSAItemAttribute _PSAAttribute = (PSAItemAttribute) Attribute.GetCustomAttribute(item, typeof(PSAItemAttribute));

                    if (_PSAAttribute == null)
                        continue;

                    MethodInfo onEnable = item.GetMethod("OnEnable");
                    MethodInfo onGUI = item.GetMethod("OnGUI");
                    MethodInfo isReady = item.GetMethod("IsReady");
                    Action onEnableDelegate = null;
                    Action onGUIDelegate = null;
                    bool isReadySetup = true;
                    string prefix = " ( ? )";

                    if (onEnable != null && onEnable.IsStatic)
                    {
                        onEnableDelegate = (Action) Delegate.CreateDelegate(typeof(Action), null, onEnable);
                    }

                    if (onGUI != null && onGUI.IsStatic)
                    {
                        onGUIDelegate = (Action) Delegate.CreateDelegate(typeof(Action), null, onGUI);
                    }

                    if (isReady != null && isReady.IsStatic)
                    {
                        isReadySetup = (bool) isReady.Invoke(item, null);
                        prefix = isReadySetup ? " ( ✓ )" : " ( ! )";
                    }

                    int id = UnityEngine.Random.Range(1, 9999);
                    while (itemProperties.Any(t => id == t.GetID()))
                    {
                        id = UnityEngine.Random.Range(1, 9999);
                    }

                    itemProperties.Add(new ItemProperty(id, _PSAAttribute.GetIndex(), _PSAAttribute.GetName() + prefix, _PSAAttribute.GetItemType(), onEnableDelegate, onGUIDelegate, isReadySetup));
                }
                return itemProperties.ToArray();
            }

            /// <summary>
            /// Return true if all Project Setup Assistant is complete ready,
            /// return false if at least one is false.
            /// </summary>
            /// <returns></returns>
            public static bool ItemsIsReady()
            {
                ItemProperty[] itemProperties = GetItemProperties();
                if (itemProperties == null || itemProperties.Length == 0)
                {
                    return true;
                }

                bool[] readiness = new bool[itemProperties.Length];
                for (int i = 0; i < itemProperties.Length; i++)
                {
                    if (!itemProperties[i].IsReady())
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}