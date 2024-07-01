/*
 * author       : jave.lin
 * datetime     : 2024/07/01 15:31
 * description  : 对 c# 脚本资源的后处理
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSharpMonoBehaviourScriptAssetsModProcessor : UnityEditor.AssetModificationProcessor
{
    private static HashSet<string> _s_pHashsetWillCreate = new HashSet<string>();

    public static bool InWillCreate(string t_strAssetName)
    {
        return _s_pHashsetWillCreate.Contains(t_strAssetName);
    }

    public static void RemoveFromWillCreate(string t_strAssetName)
    {
        _s_pHashsetWillCreate.Remove(t_strAssetName);
    }
    private static void OnWillCreateAsset(string assetName)
    {
        // 比如： 输出：AssetsModProcess OnWillCreateAsset : Assets/PG/Editor/test.cs
        // Debug.Log($"AssetsModProcess OnWillCreateAsset : {assetName}");
        var t_strExt = Path.GetExtension(assetName);
        if (t_strExt == ".cs")
        {
            _s_pHashsetWillCreate.Add(assetName);
            if (System.IO.File.Exists(assetName))
            {
                var t_strContent = System.IO.File.ReadAllText(assetName);
                t_strContent = t_strContent.Replace("jave.lin", "linjianfeng");
                t_strContent = t_strContent.Replace("#DATETIME#", System.DateTime.Now.ToString());

                System.IO.File.WriteAllText(assetName, t_strContent);
                AssetDatabase.Refresh();
            }
        }
    }
}

public class MonoBehaviourScriptAssetsPostProcessor : AssetPostprocessor
{
    private static Regex _s_pCSharpFileHeaderComment =
        new Regex(
            @"/\*\s*\* author\s*:\s*.*\s*\*\s*datetime\s*:\s*.*\s*\*\sdescription\s*:\s*\[description]\s*\s*\*\s*\*/",
            RegexOptions.Compiled);
    private void OnPreprocessAsset()
    {
        if (CSharpMonoBehaviourScriptAssetsModProcessor.InWillCreate(assetPath))
        {
            if (CSharpMonoBehaviourScriptMod(assetPath))
            {
                CSharpMonoBehaviourScriptAssetsModProcessor.RemoveFromWillCreate(assetPath);
            }
        }
    }

    private static bool CSharpMonoBehaviourScriptMod(string t_strAssetName)
    {
        try
        {
            var t_strContent = System.IO.File.ReadAllText(t_strAssetName);
            var match = _s_pCSharpFileHeaderComment.Match(t_strContent);
            if (!match.Success)
            {
                var t_strHeaderSB = new StringBuilder();
                t_strHeaderSB.AppendLine(@"/*
 * author       : #AUTHOR#
 * datetime     : #DATETIME#
 * description  : [description]
 * */");
                t_strContent = t_strHeaderSB.ToString() + t_strContent;
            }
            t_strContent = t_strContent.Replace("#AUTHOR#", Environment.MachineName);
            t_strContent = t_strContent.Replace("#DATETIME#", System.DateTime.Now.ToString());  

            System.IO.File.WriteAllText(t_strAssetName, t_strContent);
            return true;
        }
        catch (System.Exception t_pERR)
        {
            Debug.LogError(t_pERR);
            return false;
        }
    }
}
