using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false, true, false, "Prefab brush")]
public class PrefabBrush : GameObjectBrush
{

}
