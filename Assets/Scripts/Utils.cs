using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public static class Utils
{
    public static IEnumerator WaitForComponent<T>(GameObject unityObject)
    {
        yield return new WaitUntil(() => unityObject.GetComponent<T>() != null);

    }
    public static float Remap(float x, float min1, float max1, float min2, float max2)
    {
        float t = (x - min1) / (max1 - min1);
        return min2 + t * (max2 - min2);
    }

    public static Vector2 TilingAndOffset(Vector2 uv, Vector2 tiling, Vector2 offset)
    {
        return uv * tiling + offset;
    }

    public static float Frac(float number) => number - Mathf.Floor(number);

    public static void DebugCube(Vector3 position)
    {
        GameObject debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        debugCube.transform.position = position;

        debugCube.transform.localScale = new Vector3(1, 1, 1);

        debugCube.GetComponent<Renderer>().material.color = Color.red;
    }

    public static void DestroyChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildrenImmediate(Transform transform)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            GameObject.DestroyImmediate(child);
        }
    }
    public static float SmoothStep(float edge0, float edge1, float x)
    {
        // Clamp x between 0 and 1 after normalizing it between edge0 and edge1
        float t = Math.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        // Smoothstep polynomial
        return t * t * (3.0f - 2.0f * t);
    }

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time, float deltaTime)
    {
        if (deltaTime < Mathf.Epsilon) return rot;
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time, float.PositiveInfinity, deltaTime),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time, float.PositiveInfinity, deltaTime),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time, float.PositiveInfinity, deltaTime),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time, float.PositiveInfinity, deltaTime)
        ).normalized;

        // ensure deriv is tangent
        var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
        deriv.x -= derivError.x;
        deriv.y -= derivError.y;
        deriv.z -= derivError.z;
        deriv.w -= derivError.w;

        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }

    public static Color FromHex(string hex)
    {
        Color col;
        if (!ColorUtility.TryParseHtmlString(hex, out col))
        {
            throw new System.Exception("Hex invalid");
        }
        return col;
    }

    public static Bounds GetColliderBounds(Collider colliderPrefab)
    {
        var coll = GameObject.Instantiate(colliderPrefab);
        var bounds = coll.bounds;
        GameObject.Destroy(coll.gameObject);
        return bounds;
    }

    public static void DisableParticleSystemEmission(GameObject effect)
    {
        foreach (var ef in effect.GetComponents<ParticleSystem>().Union(effect.GetComponentsInChildren<ParticleSystem>()))
        {
            var em = ef.emission;
            em.rateOverDistanceMultiplier = 0;
            em.rateOverTimeMultiplier = 0;
        }
    }

    public static void ResetEffect(GameObject effect)
    {
        foreach (var ef in effect.GetComponents<ParticleSystem>().Union(effect.GetComponentsInChildren<ParticleSystem>()))
        {
            ef.Play();
        }
    }

    public static void DestroyParticleSystemOnComplete(GameObject effect)
    {
        float maxDuration = 0;
        foreach (var ef in effect.GetComponents<ParticleSystem>().Union(effect.GetComponentsInChildren<ParticleSystem>()))
        {
            maxDuration = Mathf.Max(ef.main.duration, maxDuration);
        }
        GameObject.Destroy(effect, maxDuration);
    }

    public static string GetPrefabNameFromInstance(string instanceName)
    {
        return instanceName.Replace("(Clone)", "");
    }

    public static Color HexToColor(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}
