using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    void Interact();
}

public interface IDamageable {
    void TakeDamage(float damage);
}

public interface IDamageableFriendly {
    void TakeDamage(float damage);
}