using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class sm_10 
{
    static readonly string outputPath = "Assets/RaymatchingTutorial/Resources/RaymarchingQuad.mesh";
    const int expandBounds = 10000;//拡張するサイズ

    [MenuItem("Tools/CreateRaymarchingQuadMesh")]
    static void CreateRaymarchingQuadMesh()
    {
        //MeshのAssetを作成する
        var mesh = new Mesh
        {
            vertices = new[]
            {
                new Vector3(1f,1f,0f),
                new Vector3(-1f,1f,0f),
                new Vector3(-1f,-1f,0f),
                new Vector3(1f,-1f,0f)
            },
            uv = new[]
            {
                new Vector2(1f,1f),
                new Vector2(0f,1f),
                new Vector2(0f,0f),
                new Vector2(1f,0f)
            },
            triangles = new[] { 0, 1, 2, 2, 3, 0 }
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //バウンディングボックスを拡張する
        var bounds = mesh.bounds;
        bounds.Expand(expandBounds);
        mesh.bounds = bounds;

        //後述の関数
        SafeCreateDirectory(Path.GetDirectoryName(outputPath));

        var oldAsset = AssetDatabase.LoadAssetAtPath<Mesh>(outputPath);
        if (oldAsset)
        {
            //既にアセットがあるなら更新する
            oldAsset.Clear();//Meshアセット更新の前に消す必要あり
            EditorUtility.CopySerialized(mesh, oldAsset);
            AssetDatabase.SaveAssets();
        }
        else
        {
            //まだアセットがない場合は新規作成する
            AssetDatabase.CreateAsset(mesh, outputPath);
            AssetDatabase.Refresh();
        }

    }

    static DirectoryInfo SafeCreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return null;
        }
        return Directory.CreateDirectory(path);
    }
}
