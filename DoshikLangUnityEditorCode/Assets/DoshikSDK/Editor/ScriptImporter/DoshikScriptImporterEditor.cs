using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace DoshikSDK
{
    [CustomEditor(typeof(DoshikProgramAsset))]
    public class DoshikScriptImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            bool enabled = GUI.enabled;
            GUI.enabled = true;

            GUILayout.Label("Source code file for Doshik language (Udon)", EditorStyles.boldLabel, new GUILayoutOption[0]);

            GUI.enabled = enabled;
        }
    }
}