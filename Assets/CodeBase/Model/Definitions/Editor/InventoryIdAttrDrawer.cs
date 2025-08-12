using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttr))]
    public class InventoryIdAttrDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.I.Items.ItemsForEditor;
            var ids = defs.Select(i => i.Id).ToList();

            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);
            EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}
