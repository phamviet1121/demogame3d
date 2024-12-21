using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSwitcher : MonoBehaviour
{
    public Animator anim;
    public Rigidbody[] rigids;
    [ContextMenu("RetriverigidBodies")]
    private void RetriverigidBodies()
    {
        rigids = GetComponentsInChildren<Rigidbody>();
    }
    [ContextMenu("ClearRagdoll")]
    private void ClearRagdoll()
    {
        CharacterJoint[] joint = GetComponentsInChildren<CharacterJoint>();
        for (int i = 0; i < joint.Length; i++)
        {
            DestroyImmediate(joint[i]);

        }
        Rigidbody[] riggidlist = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < riggidlist.Length; i++)
        {
            DestroyImmediate(riggidlist[i]);

        }
        Collider[] colls = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colls.Length; i++)
        {
            DestroyImmediate(colls[i]);

        }
    }
    [ContextMenu("EnableRagdoll")]
    public void EnableRagdoll()
    {
        anim.SetTrigger("die");
        SetRagdoll(true);
    }
    [ContextMenu("DisableRagdoll")]
    public void DisableRagdoll()
    {
        SetRagdoll(false);
    }

    private void SetRagdoll(bool ragdollEnable)
    {
        anim.enabled = !ragdollEnable;

        for (int i = 0; i < rigids.Length; i++)
        {

            rigids[i].isKinematic = !ragdollEnable;

        }
    }
    [ContextMenu("AddHitSurface")]
    private void AddHitSurface()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (gameObject.GetComponent<HitSurface>() == null)
            {
                var hitSurface = collider.gameObject.AddComponent<HitSurface>();
                hitSurface.surfaceType = HitSurfaceType.Blood;
            }
        }
    }
}
