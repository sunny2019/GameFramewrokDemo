using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class EditorCodeGen
    {
        private static string templatePathFormat = "Assets/GameMain/Common/Editor/CodeGen/Template/{0}.txt";
        
        [MenuItem("Assets/Create/C# Script Template/UIForm")]
        static void CodeGenUIForm()
        {
            CodeGenerate("DefaultUIForm");
        }

        [MenuItem("Assets/Create/C# Script Template/Entity")]
        static void CodeGenEntity()
        {
            CodeGenerate("DefaultEntity");
        }

        [MenuItem("Assets/Create/C# Script Template/Procedure")]
        static void CodeGenProcedure()
        {
            CodeGenerate("Procedure_Default");
        }

        static void CodeGenerate(string temlateName)
        {
            string fileName = temlateName;
            string filePath = EditorBaseTool.EditorSelectPath("脚本生成", fileName, "cs");
            if (File.Exists(filePath))
            {
                Log.Debug("生成文件已存在：" + filePath);
            }
            else
            {
                string sourcePath = string.Format(Path.Combine(Directory.GetParent(Application.dataPath).FullName, templatePathFormat), fileName);
                string allText = File.ReadAllText(sourcePath);
                allText = allText.Replace(fileName, Path.GetFileNameWithoutExtension(filePath));
                File.WriteAllText(filePath, allText);
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
            }
        }
    }
}