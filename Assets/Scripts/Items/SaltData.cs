using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable salt data", menuName = "Item/Salt Item Data")]
public class SaltData : ItemData
{
    [Header("Salt Weapon Data")]
    public GameObject throwablePrefab;
    public ItemData projectileItemData;

    public void Fire(Vector3 spawnPosition, Quaternion spawnRotation, Character.Team team)
    {
        GameObject proj = Instantiate(throwablePrefab, spawnPosition, spawnRotation);
        proj.GetComponent<Slide>();
    }
}
