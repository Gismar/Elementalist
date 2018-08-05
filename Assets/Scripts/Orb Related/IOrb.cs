using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrb {
    void MainAttack();
    void SecondaryAttack();
    void SetIdle();
    void ActivateAimLine();
    void UpdateAimLine();
    void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType);
    void Swap();

    float Damage { get; }
    float MainAttackDelay { get; }
    float SecondaryAttackDelay { get; }
}
