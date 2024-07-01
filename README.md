1. Download `CSharpMonoBehaviourScriptAssetsModProcessor.cs`.
2. Putting `CSharpMonoBehaviourScriptAssetsModProcessor.cs` to your unity project `Editor` folder.
3. Modify `CSharpMonoBehaviourScriptMod` functions to you want.

```csharp
    private static bool CSharpMonoBehaviourScriptMod(string t_strAssetName)
    {
        try
        {
            var t_strContent = System.IO.File.ReadAllText(t_strAssetName);
            if (!t_strContent.StartsWith("/*"))
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
```

U can custom place-holder to you want:
Now i what to add a `"PROJECTNAME"` place-hodler like this:
```csharp
            if (!match.Success)
            {
                var t_strHeaderSB = new StringBuilder();
                t_strHeaderSB.AppendLine(@"/*
 * author       : #AUTHOR#
 * datetime     : #DATETIME#
 * projectname  : #PROJECTNAME#
 * description  : [description]
 * */");
                t_strContent = t_strHeaderSB.ToString() + t_strContent;
            }
            t_strContent = t_strContent.Replace("#AUTHOR#", Environment.MachineName);
            t_strContent = t_strContent.Replace("#DATETIME#", System.DateTime.Now.ToString());
            t_strContent = t_strContent.Replace("#PROJECTNAME#", "Super Star XIII");  // this is what i want to custom place-holder
```
