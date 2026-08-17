using Code.Player;
using System;
using UnityEngine;

public class Paper : Item
{
    [SerializeField] Transform hand;
    [SerializeField] GameObject paperGO;
    [SerializeField] float coverRange;
    [SerializeField] Transform playerViewOrigin;
    [SerializeField] bool drawRangeGizmos;

    public override ItemType ItemType => ItemType.Paper;

    public override void Activate()
    {
        paperGO.SetActive(true);
        paperGO.transform.SetParent(hand);
        paperGO.transform.localPosition = Vector3.zero;
        paperGO.transform.localRotation = Quaternion.identity;
    }

    public override void Deactivate()
    {
        paperGO.SetActive(false);
    }

    public override void PrimaryAction()
    {
        if (CoverObject())
            Release();
    }

    private bool CoverObject()
    {
        Vector3 origin = playerViewOrigin.position;
        Vector3 target = origin + playerViewOrigin.forward * coverRange;

        if (Physics.Raycast(origin, playerViewOrigin.forward, out RaycastHit hit, coverRange))
        {
            Debug.Log($"Trying to cover object: {hit.transform.name}");

            if (hit.collider.TryGetComponent<ICoverable>(out var coverable) ||
                hit.transform.parent.parent.TryGetComponent<ICoverable>(out coverable))
            {
                coverable.Cover();
                Debug.Log($"Covering object {coverable}");
                return true;
            }

        }

        return false;
    }

    public override void PrimaryActionReleased()
    {

    }

    public override void Release()
    {
        OnItemReleased?.Invoke(this);
        Debug.Log("Paper released");
        Destroy(gameObject);
    }

    public override void SecondaryAction()
    {

    }

    public override void SecondaryActionReleased()
    {

    }

    private void OnDrawGizmos()
    {
        if (!drawRangeGizmos)
            return;

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(playerViewOrigin.position, playerViewOrigin.position + playerViewOrigin.forward * coverRange);
    }
}