
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace KLRB.Utility.Editor
{
    
    public static class AssetDataBaseUtility 
    {
    public static List<Object> GetNestedAssetsSorted(Object baseObject, string prefix)
    {
        List<Object> assetsList = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(baseObject)).ToList();
        assetsList = assetsList.Where(asset => asset.name.Contains(prefix)).ToList();
        assetsList = assetsList.OrderBy(asset => asset.name).ToList();
        return assetsList;
    }
    
    
    /*public static GameObject CreateNestedAsset(ScriptableObject baseAsset,GameObject toPrefab, string prefix, string suffix)
    {
        string path = AssetDatabase.GetAssetPath(baseAsset);
        var asset = PrefabUtility.SaveAsPrefabAsset()
        return AddNestedAsset<T>(asset, prefix, suffix);
    }*/
    
    
    public static T CreateNestedAsset<T>(ScriptableObject baseAsset, string prefix, string suffix) where T :ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        return AddNestedAsset<T>(baseAsset,asset, prefix, suffix);
    }
    
    public static T CreateNestedAsset<T>(ScriptableObject baseAsset, string prefix, int index, Func<int,string> suffixFunc, List<T> listRef) where T :ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        return AddNestedAsset(baseAsset,asset, prefix, index, suffixFunc, listRef);
    }

    
        
    public static T AddNestedAsset<T>(ScriptableObject baseAsset,Object assetToAdd, string prefix,string suffix) where T : Object
    {
        string path = AssetDatabase.GetAssetPath(baseAsset);
        var assets = GetNestedAssets<T>(baseAsset);
        assetToAdd.name = prefix + "_" + (assets.Count)  +  suffix;
        AssetDatabase.AddObjectToAsset(assetToAdd, path);
        AssetDatabase.SaveAssetIfDirty(baseAsset);
        return (T)assetToAdd;
    }
    
    public static T AddNestedAsset<T>(ScriptableObject baseAsset,Object assetToAdd, string prefix, int index, Func<int,string> suffixFunc, List<T> listRef ) where T : Object
    {
        AddNestedAsset<T>(baseAsset,assetToAdd, prefix, "");
        var assets = GetNestedAssetsSorted<T>(baseAsset);
        listRef.Clear();
        foreach(var sortedAsset in assets) listRef.Add(sortedAsset as T);

        for (int i = 0; i < listRef.Count; i++)
        {
            listRef[i].name = prefix + "_" + i.ToString() +  suffixFunc(i);
        }
        AssetDatabase.SaveAssetIfDirty(baseAsset);
        return (T)assetToAdd;
    }
    
    
    public static void RemoveNestedAsset<T>(ScriptableObject baseAsset, T assetToRemove, bool displayConfirmation = false ) where T : Object
    {

        if (displayConfirmation)
        {
            bool confirm = EditorUtility.DisplayDialog("Delete Nested Asset",
                $" Are you sure you want to delete {assetToRemove.name}?", "Yes", "No");
            if (!confirm) return;
        }
        
        Object.DestroyImmediate((T)assetToRemove, true);
        AssetDatabase.SaveAssetIfDirty(baseAsset);
     
        //AssetDatabase.Refresh();
    }
    
    public static void RemoveNestedAsset<T>(ScriptableObject baseAsset, int index) where T :Object
    {
        var sortedAssets = AssetDataBaseUtility.GetNestedAssetsSorted<T>(baseAsset);
        Object.DestroyImmediate((T)sortedAssets[index], true);
        AssetDatabase.SaveAssetIfDirty(baseAsset);
       // AssetDatabase.Refresh();
    }
    
    
    public static void RemoveNestedAsset<T>(ScriptableObject baseAsset, string prefix, int index, Func<int,string> suffixFunc, List<T> listRef) where T :Object
   {
       RemoveNestedAsset<T>(baseAsset, index);
       listRef.RemoveAt(index);
       for (int i = 0; i < listRef.Count; i++)
       {
           listRef[i].name = prefix + "_" + i.ToString() +  suffixFunc(i);
       }
       AssetDatabase.SaveAssetIfDirty(baseAsset);
   }

    
    public static T TryGetNestedAsset<T>(ScriptableObject baseAsset) where T : Object
    {
        var assets = GetNestedAssets<T>(baseAsset);
        if (assets.Count > 0)
        {
            return assets[0];
        }
        return null;
    }
    
    public static List<T> GetNestedAssets<T>(Object baseObject) where T : Object
    {
        List<T> assetsList = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(baseObject)).OfType<T>().ToList();
        return assetsList;
    }

    public static List<T> GetNestedAssetsSorted<T>(Object baseObject) where T : Object
    {
        List<T> assetsList = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(baseObject)).OfType<T>().ToList();
        assetsList = assetsList.OrderBy(asset => asset.name).ToList();
        return assetsList;
    }
    
    
    public static bool IsAssetOfType<T>(string path)
    {
        if (!path.EndsWith(".asset", StringComparison.OrdinalIgnoreCase)) return false;
        var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
        return typeof(T).IsAssignableFrom(assetType);
    }
    
}


}

#endif
