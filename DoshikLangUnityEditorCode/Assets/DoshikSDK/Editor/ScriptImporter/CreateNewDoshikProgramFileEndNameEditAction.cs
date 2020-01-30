using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace DoshikSDK
{
    public class CreateNewDoshikProgramFileEndNameEditAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            Object o = DoshikScriptImporter.CreateAsset(pathName);
            ProjectWindowUtil.ShowCreatedAsset(o);
        }
    }
}