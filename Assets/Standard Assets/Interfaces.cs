using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    void Interact();
}

public interface IDamageable {
    void TakeDamage();
}