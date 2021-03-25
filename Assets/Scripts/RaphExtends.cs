using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public static class RaphExtends
{
    public static Vector3 MaxPosByVector(Vector3 distant, Vector3 source, float maxDistance, Vector3 axis, bool verbose = false)
    {
        Vector3 path = source - distant;


        Vector3 result;

        if (path.magnitude < maxDistance)
        {
            if (verbose)
                Debug.Log("Inférieure au max: " + maxDistance.ToString());
            //return distant;
            result = distant;
        }
        else
        {
            float RelativeDist = maxDistance / path.magnitude;

            float x = source.x + (RelativeDist * path.x) * -axis.x;
            float y = source.y + (RelativeDist * path.y) * -axis.y;
            float z = source.z + (RelativeDist * path.z) * -axis.z;

            result = new Vector3(x, y, z);

        }




        if (verbose)
        {
            Debug.Log("Distance: " + path.magnitude);
            Debug.DrawLine(source, distant, Color.red);
            Debug.DrawLine(source, result, Color.green);
        }

        return result;
    }

    public static Vector3 PosByVector(Vector3 distant, Vector3 source, float Distance, Vector3 axis, bool verbose = false)
    {
        Vector3 path = source - distant;


        Vector3 result;


        float RelativeDist = Distance / path.magnitude;

        float x = source.x + (RelativeDist * path.x) * -axis.x;
        float y = source.y + (RelativeDist * path.y) * -axis.y;
        float z = source.z + (RelativeDist * path.z) * -axis.z;

        result = new Vector3(x, y, z);






        if (verbose)
        {
            Debug.Log("Distance: " + path.magnitude);
            Debug.DrawLine(source, distant, Color.red);
            Debug.DrawLine(source, result, Color.green);
        }

        return result;
    }

    public static void setActiveDelayed(this GameObject gameObject, bool activeness, float delay)
    {
        if (gameObject.GetComponent<MonoBehaviour>())
        {
            gameObject.GetComponent<MonoBehaviour>().StartCoroutine(ActiveDelay(gameObject, activeness, delay));
        }
        else if (Camera.main)
        {
            Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(ActiveDelay(gameObject, activeness, delay));
        }
        else
        {
            GameObject obj = GameObject.Instantiate(new GameObject("$DELAY%"));
            obj.GetComponent<MonoBehaviour>().StartCoroutine(ActiveDelay(gameObject, activeness, delay));
            GameObject.Destroy(obj, delay + 0.1f);
        }
    }

    public static IEnumerator ActiveDelay(GameObject obj, bool activeness, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(activeness);
    }


    //VECTORS
    public static Vector2 ToVector2XY(this Vector3 v)
    {

        ///<summary>
        ///<para>return new Vector2(v.x, v.y);</para>
        ///</summary>
        return new Vector2(v.x, v.y);

    }

    public static Vector2 ToVector2YZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector2 ToVector2XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    ///<summary>
    ///return new Vector3(v.x, v.y, 0)
    ///</summary>
    public static Vector3 ToVector3Z(this Vector2 v)
    {


        ///</summary>
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 ToVector3Y(this Vector2 v)
    {
        ///<summary>
        ///<para> return new Vector3(v.x, 0, v.y);</para>
        ///</summary>
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector3 ToVector3X(this Vector2 v)
    {
        ///<summary>
        ///<para>return new Vector3(0, v.x, v.y);</para>
        ///</summary>
        return new Vector3(0, v.x, v.y);
    }

    public static Vector3 Round(this Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static Vector3 Random(this Vector3 v)
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
    }

    public static Vector3 Min(this Vector3 v, float min)
    {
        return new Vector3(Mathf.Min(v.x, min), Mathf.Min(v.y, min), Mathf.Min(v.z, min));
    }

    public static Vector3 Min(this Vector3 v, Vector3 min)
    {
        return new Vector3(Mathf.Min(v.x, min.x), Mathf.Min(v.y, min.y), Mathf.Min(v.z, min.z));
    }

    public static Vector3 Max(this Vector3 v, float max)
    {
        return new Vector3(Mathf.Max(v.x, max), Mathf.Max(v.y, max), Mathf.Max(v.z, max));
    }

    public static Vector3 Max(this Vector3 v, Vector3 max)
    {
        return new Vector3(Mathf.Max(v.x, max.x), Mathf.Max(v.y, max.y), Mathf.Max(v.z, max.z));
    }

    public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
    }

    public static Vector3 Clamp(this Vector3 v, float min, float max)
    {
        return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
    }

    public static Vector3 Lerp(this Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
    }

    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static void BlendVolume(this AudioSource a, float Volume, float delay)
    {
        a.GetComponentInParent<MonoBehaviour>().StartCoroutine(_BlendVolume(a, Volume, delay));
    }


    static IEnumerator _BlendVolume(AudioSource source, float volume, float t)
    {
        float initVolume = source.volume;
        float baseT = t;

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            source.volume = Mathf.Lerp(initVolume, volume, (baseT - t) / baseT);
        }
    }

    public static Vector2Int oneDtoTwoD(int index, int width)
    {
        int x = index % width;
        int y = index / width;
        return new Vector2Int(x, y);
    }

    public static int twoDtoOneD(Vector2Int pos, int width)
    {
        return pos.y * width + pos.x;
    }


    //COLORS

    public static ColorHSL toHSL(this Color c)
    {

        float h, s, l;
        Color.RGBToHSV(c, out h, out s, out l);
        return new ColorHSL(h * 360, s, l);
    }

    public static Vector2 Abs(this Vector2 v)
    {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
    }

    public static Vector2 Clamp(this Vector2 v, float min, float max)
    {
        return new Vector2(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
    }


    public static Transform Find(this Transform t, string name)
    {
        return GameObject.Find(name).transform;
    }

    public static Bounds Mul(this Bounds b, float val)
    {
        return new Bounds(b.center, b.size * val);
    }

    public static Vector3 Mul(this Vector3 A, Vector3 B)
    {
        return new Vector3(A.x * B.x, A.y * B.y, A.z * B.z);
    }


    public static void startDelayed(this GameObject gameObject, Action action, float delay, params string[] args)
    {

        gameObject.GetComponent<MonoBehaviour>().StartCoroutine(_startDelayed(action, delay, args));

    }

    static IEnumerator _startDelayed(Action action, float delay, params string[] args)
    {
        yield return new WaitForSeconds(delay);
        action.DynamicInvoke(args);
    }


    public static T RandomOne<T>(this T[] array)
    {
        if (array.Length > 1)
            return array[UnityEngine.Random.Range(0, array.Length)];
        else if (array.Length > 0)
            return array[0];
        else
            return default(T);
    }

    public static T RandomOne<T>(this List<T> array)
    {
        if (array.Count > 1)
            return array[UnityEngine.Random.Range(0, array.Count)];
        else if (array.Count > 0)
            return array[0];
        else
            return default(T);
    }

    public static int RandomSign()
    {
        float v = UnityEngine.Random.Range(0, 100);
        if (v > 50)
            return 1;
        else
            return -1;
    }


    public static void StartRandom(this AudioSource source)
    {
        source.Play();
        source.time = UnityEngine.Random.Range(0f, source.clip.length);
    }


    public static int toInt(this bool b)
    {
        if (b == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static bool toBool(this int i)
    {
        if (i >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //GAMEOBJECT
    public static T GameobjectFindComponent<T>(this GameObject obj, string name)
    {
        GameObject o = GameObject.Find(name);
        if (o != null)
            return o.GetComponent<T>();
        else
        {
            Debug.LogError("Enable to find " + name, obj);
            return default;
        }


    }

    public static Vector3 toV3(this float f)
    {
        return new Vector3(f, f, f);
    }

}



public class InputExtend
{
    public static Vector3 WorldMousePos { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    public static Vector2 Direction2D
    {
        get
        {
            Vector2 res = Vector2.zero;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.I))
                res.y = 1;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K))
                res.y = -1;

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.J))
                res.x = -1;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.L))
                res.x = 1;

            res = res.normalized;

            return res;
        }
    }


    public static Vector3 Direction3D(Transform transform)
    {

        Vector3 res = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.I))
            res += transform.forward;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.K))
            res -= transform.forward;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.J))
            res -= transform.right;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.L))
            res += transform.right;

        if (Input.GetKeyDown(KeyCode.Space))
            res += transform.up;

        res = res.normalized;

        return res;

    }
}

public class ColorHSL
{
    public ColorHSL(float h, float s, float l)
    {
        hue = h;
        saturation = s;
        light = l;
    }

    public float hue;
    public float saturation;
    public float light;

    public Color toRGB()
    {
        return Color.HSVToRGB(hue / 360, saturation, light);


    }

}