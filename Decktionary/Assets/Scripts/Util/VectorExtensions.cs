using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2Int ToVecInt(this Vector2 vec2)
    {
	   return new Vector2Int((int)vec2.x, (int)vec2.y);
    }

    public static Vector2 ToVec(this Vector2Int vec2)
    {
	   return new Vector2(vec2.x, vec2.y);
    }

    public static Vector2 DirVecTo(this Vector2 start, Vector2 target)
    {
	   return (target - start).normalized;
    }

    public static Vector3Int ToVecInt(this Vector3 vec3)
    {
	   return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
    }

    public static Vector3 ToVec(this Vector3Int vec3)
    {
	   return new Vector3(vec3.x, vec3.y, vec3.z);
    }

    public static Vector3 DirVecTo(this Vector3 start, Vector3 target)
    {
	   return (target - start).normalized;
    }

    public static Quaternion ToRotation(this Vector2 direction)
    {
	   float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	   return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    /// Converts <paramref name="angle"></paramref> (in rads) to a Vector2 direction.
    /// </summary>
    /// <param name="angle">Angle in radians.</param>
    /// <returns></returns>
    public static Vector2 DirectionFromAngle(this float angle)
    {
	   return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    /// <summary>
    /// Converts <paramref name="dir"></paramref> (a vector) to a float in degrees.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static float ToAngle(this Vector2 dir)
    {
	   if (dir.x < 0)
	   {
		  return 360 - (Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg * -1);
	   }
	   else
	   {
		  return Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
	   }
    }

    public static float ToAngle(this Quaternion quat)
    {
	   return quat.eulerAngles.z;
    }
}
