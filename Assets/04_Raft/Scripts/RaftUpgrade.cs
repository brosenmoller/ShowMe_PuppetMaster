using UnityEngine;

[CreateAssetMenu(menuName = "RaftUpgrade", fileName = "New RaftUpgrade")]
public class RaftUpgrade : ScriptableObject
{
    public string upgradeName;
    public GameObject prefab;
    public GameObject pickUpPrefab;
    public Vector3 spawnOffset;
    public int healthWorth;
}
