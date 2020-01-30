using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using System.Text;

namespace DoshikSDK
{
    [ScriptedImporter(1, "doshik")]
    public class DoshikScriptImporter : ScriptedImporter
    {
        [MenuItem("Assets/Create/Doshik Program", priority = 82)]
        public static void CreateNewProgramFile()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateNewDoshikProgramFileEndNameEditAction>(), "Program.doshik", null, "");
        }

        public static Object CreateAsset(string fileName)
        {
            var fileSource = "// Start is called before the first frame update\nevent void Start()\n{\n}\n\n// Update is called once per frame\nevent void Update()\n{\n}";

            string fullPath = Path.GetFullPath(fileName);
            File.WriteAllText(fullPath, fileSource, new UTF8Encoding(true));
            AssetDatabase.ImportAsset(fileName);
            return AssetDatabase.LoadAssetAtPath(fileName, typeof(Object));
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            string fullPath = Path.GetFullPath(ctx.assetPath);
            string sourceCode = File.ReadAllText(fullPath);

            DoshikProgramAsset programAsset = ScriptableObject.CreateInstance<DoshikProgramAsset>();
            programAsset.SetSourceCode(sourceCode);

            programAsset.RefreshProgram();

            ctx.AddObjectToAsset("Doshik Program", programAsset);
            ctx.SetMainObject(programAsset);
        }
    }
}