using System;
using UnityEditor;
using UnityEngine;

//The concrete goal scriptable is needed to create a instance of the Scriptable as there's no support for generic Scriptables yet.
[CreateAssetMenu(fileName = "LevelDefinitionScriptable", menuName = "CreateUtils/Level Definition Scriptable")]
public class LevelDefinitionScriptable : GenericLevelDefinitionScriptable<LevelDefinition>
{
}

//This Concrete Goal will be used to determine the set of values for each goal.
[Serializable]
public class LevelDefinition : GenericDefinition
{
    //Ex: If each level has a set of Materials to use for the skybox or an object it can be set here.
    //public Material skyboxMaterial;
    //public Material levelMaterial;

    //Another Ex: If each level has to loop through a set of objects you can set the objects in each level here.
    //public List<GameObject> goalObjects;
    public SceneAsset sceneToLoad;
}
