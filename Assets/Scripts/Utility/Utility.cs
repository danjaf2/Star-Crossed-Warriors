using System.Collections.Generic;
using UnityEngine;

static class Utility {
    #region CONSTANTS

    public const float TWO_PI = 2 * Mathf.PI;
    public static readonly int ANIM_STATE_KEY = Animator.StringToHash("State");

    #endregion

    #region BOOL

    public static byte ToByte(this bool value) { return value ? (byte)1 : (byte)0; }
    public static float ToFloat(this bool value) { return value ? 1f : 0f; }

    #endregion


    #region ANGLES
    public static Vector2 PolarCoordinates(float angle) {
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }
    public static Vector3 PolarCoordinatesXY(float angle) {
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
    }
    public static Vector3 PolarCoordinatesXZ(float angle) {
        return new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
    }

    public static float PolarAngle(float x, float y) {
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    public static Vector3 RandomDirection() {
        float angle = Random.Range(0f, TWO_PI);
        return new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
    }
    #endregion



    #region VECTORS

    public static Vector3 TopDown3D(this Vector2 vector) {
        return new Vector3(vector.x, 0f, vector.y);
    }

    public static Vector3 Towards(this Transform transform, Transform other) {
        return other.position - transform.position;
    }

    public static Vector3 RandomPointInArea(this Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    #endregion


    #region GAMEOBJECT
    public static void CreateGameObjectAtLocation(string name, Vector3 worldPosition) {
        GameObject spawned = new GameObject(name);
        spawned.transform.position = worldPosition;
    }
    
    public static void ToggleActive(this GameObject gameObj) {
        gameObj.SetActive(!gameObj.activeSelf);
    }

    public static void Translate(this Rigidbody rbody, Vector3 movement) {
        rbody.MovePosition(rbody.position + movement);
    }
    public static void Teleport(this CharacterController controller, Vector3 newPosition) {
        controller.enabled = false;
        controller.transform.position = newPosition;
        controller.enabled = true;
    }

    #endregion


    #region LISTS

    /// <summary>
    /// Adds the element to the list if it's not already present.
    /// </summary>
    public static void AddUnique<T>(this List<T> list, T element) {
        if (list.Contains(element)) return;
        list.Add(element);
    }

    public static T GetRandom<T>(this IList<T> list) {
        int maxIndex = list.Count;

        if (maxIndex < 0) return default(T);
        return list[Random.Range(0, maxIndex)];
    }

    public static void Shuffle<T>(this IList<T> list) {

        for (int i = list.Count - 1; i > 0; i--) {
            int swapIndex = Random.Range(0, i + 1);

            T temp = list[swapIndex];
            list[swapIndex] = list[i];
            list[i] = temp;
        }
    }

    #endregion
}