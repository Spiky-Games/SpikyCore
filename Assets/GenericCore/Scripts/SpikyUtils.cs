using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class SpikyUtils
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromAbs = from - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }

    public static Vector3 Parabola(this Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> bhaskara = x => -4 * height * x * x + 4 * height * x;

        Vector3 mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, bhaskara(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    /// <summary>
    ///     Pick a random item from array.
    /// </summary>
    public static T GetRandomItem<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    /// <summary>
    ///     Delete all childrens of transform: <paramref name="transformToClean" />.
    /// </summary>
    public static void ClearTransform(this Transform transformToClean)
    {
        for (int i = 0; i < transformToClean.childCount; i++)
        {
            Object.Destroy(transformToClean.GetChild(i).gameObject);
        }
    }

    /// <summary>
    ///     <para>Add component to a collection of GameObjects. </para>
    ///     <para>If <paramref name="allowMoreThanOnce" /> is <see langword="true" /> then it won't check it the gameobjects components to avoid doubles. </para>
    /// </summary>
    /// <typeparam name="T">Type of component to add.</typeparam>
    /// <param name="objects">Collection of gameObjects</param>
    /// <param name="allowMoreThanOnce">Allow more than once component of the same type in the same GameObject. Deactivated By default</param>
    public static void AddComponentToCollection<T>(this GameObject[] objects, bool allowMoreThanOnce = false) where T : Component
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (allowMoreThanOnce)
            {
                objects[i].AddComponent<T>();
            }
            else
            {
                //"_" to use memory discard feature C# 7.0.
                if (!objects[i].TryGetComponent(out T _))
                {
                    objects[i].AddComponent<T>();
                }
            }
        }
    }

    /// <summary>
    ///     Remove component of a collection of gameObject
    /// </summary>
    /// <param name="objects"></param>
    public static void RemoveComponentToCollection<T>(this GameObject[] objects) where T : Component
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].TryGetComponent(out T component))
            {
                Object.Destroy(component);
            }
        }
    }
}
