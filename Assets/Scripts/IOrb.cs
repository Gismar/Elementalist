using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrb {
    void MainAttack();
    void SecondaryAttack();
    void SetIdle();
    void ActivateAimLine();
    void UpdateAimLine();

    int Damage { get; }
    float IdleDelay { get; }
    float SecondaryAttackDelay { get; }
}
