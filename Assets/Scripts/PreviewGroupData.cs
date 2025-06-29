using System.Collections.Generic;
using UnityEngine;

public class PreviewGroupData
{
    public List<Vector2Int> poses = new List<Vector2Int>();

    public PreviewGroupData Clone()
    {
        var clone = new PreviewGroupData();
        clone.poses = new List<Vector2Int>(this.poses);
        return clone;
    }
}