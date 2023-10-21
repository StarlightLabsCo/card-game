using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starlight.UI
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprRend;
        [SerializeField] LineRenderer line;

        Vector3[] points;

        /// <summary>
        /// Sets the number of points of the line.
        /// </summary>
        /// <param name="count">Amount of points.</param>
        public void SetPointCount(int count)
        {
            points = new Vector3[count];
        }

	   /// <summary>
	   /// Sets an individual position on the arrow.
	   /// </summary>
	   /// <param name="index">Index of the point to set.</param>
	   /// <param name="pos">Position of the point to set</param>
	   public void SetPoint(int index, Vector3 pos)
        {
            points[index] = pos;
            UpdateArrow();
	   }

        [Button]
        /// <summary>
        /// Sets the points of the arrow (requires at least 2).
        /// </summary>
        /// <param name="points">The points of the arrow.</param>
        public void SetPoints(Vector3[] points)
        {
            this.points = new Vector3[points.Length];
            points.CopyTo(this.points, 0);
            UpdateArrow();
        }

        private void UpdateArrow()
        {
		  sprRend.transform.SetPositionAndRotation(points[0], ((Vector2)points[1].DirVecTo(points[0])).ToRotation());
		  line.SetPositions(points);
	   }
    }
}