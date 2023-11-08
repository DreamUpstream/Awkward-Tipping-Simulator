using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : EquipItem
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private AudioClip shootSFX;
private float _lastAttackTime;

    public override void OnUse()
    {
        SaltData sd = item as SaltData;
        sd.Fire(muzzle.position, muzzle.rotation, Character.Team.Player);

        Inventory.Instance.RemoveItem(sd);
        Player.Instance.equipCtrl.UnEquip();
        AudioManager.Instance.PlayPlayerSound(shootSFX);

    }
}