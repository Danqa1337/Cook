using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Database<Tself, Ttable, Tparam, Tdata> : SingletonScriptableObject<Tself>, IStartUp
    where Tself : Database<Tself, Ttable, Tparam, Tdata>
    where Ttable : ScriptableObject
    where Tparam : class
{
    protected virtual string enumName { get => ""; }

    protected abstract void ProcessParam(Tparam param);

    private static string _assetPath => Application.dataPath + "Databases/" + typeof(Tself);

    protected Tparam[] GetParams()
    {
        return TableImporter.GetParams<Ttable, Tparam>(TableImporter.FindTable<Ttable>());
    }

    [ContextMenu("Reimport")]
    public virtual void Reimport()
    {
        StartReimport();
        if (UpdateEnum()) return;
        var parameters = GetParams();

        foreach (var param in parameters)
        {
            ProcessParam(param);
        }

        EndReimport();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.ImportAsset(_assetPath);
        AssetDatabase.Refresh();
#endif
    }

    protected virtual void StartReimport()
    {
        Debug.Log("Reimporting " + GetType());
    }

    protected virtual void EndReimport()
    {
        Debug.Log("Reimporting " + GetType() + " complete!");
    }

    protected virtual bool UpdateEnum()
    {
        if (enumName == "") return false;
        var names = new List<string>();
        var enumNameField = typeof(Tparam).GetField("enumName");

        foreach (var param in GetParams())
        {
            var name = (string)enumNameField.GetValue(param);
            names.Add(name);
        }
        return EnumsUpdater.UpdateEnum(enumName, names.ToArray());
    }

    public abstract void StartUp();
}

public interface IStartUp
{
    public abstract void StartUp();
}