using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SystemCreatorTool : EditorWindow
{
    private const string SystemNameField = "SYSTEM_NAME";
    private string _typeName;

    [MenuItem("SavvaHelper/SystemCreator")]
    private static void Init()
    {
        SystemCreatorTool window = (SystemCreatorTool)EditorWindow.GetWindow(typeof(SystemCreatorTool));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("System Creator for Lazy Ass", EditorStyles.boldLabel);
        _typeName = EditorGUILayout.TextField("System Name:", _typeName);
        if(string.IsNullOrEmpty(_typeName))
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Create"))
            {
            }
            EditorGUILayout.HelpBox("Where is Type Name??", MessageType.Warning);
            GUI.color = Color.white;
        }
        else
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Create"))
            {
                CreateSystem();
            }
            GUI.color = Color.white;
        }
    }

    private void CreateSystem()
    {
        var systemName = $"{_typeName}System";

        var systemPath = Path.Combine(Application.dataPath, "Scripts", "Core", "ECS", "Systems", $"{systemName}.cs");
        var sourcePath = Path.Combine(Application.dataPath, "Scripts", "Editor", "CodeGenSource", "SystemSource");

        var systemBody = File.ReadAllText(sourcePath).
            Replace(SystemNameField, systemName);

        File.WriteAllText(systemPath, systemBody);

        _typeName = string.Empty;
        Repaint();
        AssetDatabase.Refresh();
    }
}
