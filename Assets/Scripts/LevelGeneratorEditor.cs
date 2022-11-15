using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;

        if(GUILayout.Button("Reset Level"))
        {
            levelGenerator.ResetLevel();
        }

        base.OnInspectorGUI();
    }
}
