using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutator : MonoBehaviour
{
    public bool onEnemy = false, onGround = false, onPlayer = false;

    public void ChangeState(bool onEnemy, bool onGround, bool onPlayer)
    {
        this.onEnemy = onEnemy;
        this.onGround = onGround;
        this.onPlayer = onPlayer;
    }

    protected bool AllFalse()
    {
        return !onEnemy && !onGround && !onPlayer;
    }

    /// <summary>
    /// activates the mutation when an ability is used.
    /// </summary>
    /// <param name="duration">the amount of time the ability is active. 0 if it is does not do something over time (like invisibility or turret)</param>
    /// <returns></returns>
    public virtual IEnumerator Activate(float duration)
    {
        yield return null;
    }
}
