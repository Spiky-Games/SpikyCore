using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericLevelDefinitionScriptable<T> : ScriptableObject where T : GenericDefinition
{
    //Goals list, for now it will be iterated in order. 
    [SerializeField]
    private List<T> definitions;

    //This will be used so if the goals are removed the Scriptable will be preserved.
    private GenericLevelDefinitionScriptable<T> clone;

    //Returns the goal based on the LevelManager.Instance.Current Level.
    public T GetDefinition(int definitionIndex)
    {
        if (this.clone == null)
        {
            this.clone = Instantiate(this);
        }

        return this.clone.definitions[definitionIndex];
    }

    public int DefinitionCount
    {
        get { return this.definitions.Count; }
    }
}

//Generic Goal
[Serializable]
public abstract class GenericDefinition
{
}
