using UnityEditor;
using UnityEngine;

public class RenameSelected : EditorWindow
{
    private string baseName = "Placehoulder";
    private int startNumber = 1;
    private int digits = 2;

    [MenuItem("Tools/Rename Selected")]
    static void Open()
    {
        GetWindow<RenameSelected>("Rename Selected");
    }

    void OnGUI()
    {
        GUILayout.Label("Rename Objects", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name", baseName);
        startNumber = EditorGUILayout.IntField("Start Number", startNumber);
        digits = EditorGUILayout.IntField("Digits", digits);

        GUILayout.Space(10);

        if (GUILayout.Button("Rename"))
        {
            Rename();
        }
    }

    void Rename()
    {
        GameObject[] objects = Selection.gameObjects;

        for (int i = 0; i < objects.Length; i++)
        {
            Undo.RecordObject(objects[i], "Rename Objects");

            string number = (startNumber + i).ToString("D" + digits);

            objects[i].name = $"{baseName}_{number}";
        }
    }
}