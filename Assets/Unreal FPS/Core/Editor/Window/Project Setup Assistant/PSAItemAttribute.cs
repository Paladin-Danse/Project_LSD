using System;
using UnityEngine;
using UnrealFPS.Editor;
using static UnrealFPS.Editor.ProjectSetupAssistant;


namespace UnrealFPS.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PSAItemAttribute : Attribute
    {
        private string name;
        private int index;
        private ItemType type;

        /// <summary>
        /// UProject Manager item attribute.
        /// </summary>
        /// <param name="index">Position in item list.</param>
        /// <param name="name">Item name.</param>
        /// <param name="type">Item type.</param>
        public PSAItemAttribute(string name, int index, ItemType type)
        {
            this.name = name;
            this.index = index;
            this.type = type;
        }

        public string GetName()
        {
            return name;
        }

        public int GetIndex()
        {
            return index;
        }

        public ItemType GetItemType()
        {
            return type;
        }
    }
}