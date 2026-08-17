
using Code.Player;
using System;
using System.Collections;
using UnityEngine;

public class SecurityCamera : PlayerRespawnable, ICoverable
{
    [Header("Player Detection")]
    [SerializeField] private Player player;
    [SerializeField] private Transform cameraOrigin;
    [SerializeField] private Light detectionLight;
    [SerializeField] private float detectionRange;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float detectionDelay = 2f;
    [SerializeField] private float followDelay;
    [SerializeField] private bool drawConeGizmos;

    [Header("Projectile Detection")]
    [SerializeField] private ProjectileDetector projectileDetector;

    [Header("Rotation")]
    [SerializeField] private GameObject cameraRotator;

    [Header("Scan Rotation")]
    [SerializeField] private float scanMaxAngle;
    [SerializeField] private float scanPauseTime;
    [SerializeField] private float scanDuration;

    [Header("Dodge rotation")]
    [SerializeField] private float dodgeMaxAngle;
    [SerializeField] private float dodgePauseTime;
    [SerializeField] private float dodgeDuration;

    private bool isDodging = false;
    private Quaternion initialRot;
    private Coroutine cameraRoutine;

    private bool isCovered = false;
    private bool wasSeeingPlayer = false;

    private float detectionTimer;
    private bool isTrackingPlayer;

    private bool isPlayerDisguised => player.GetSelectedItem() is Box && (player.GetSelectedItem() as Box).isDisguised;

    public Action OnPlayerRespawnCondition { get; set; }

    private void Awake()
    {
        if (cameraRotator == null)
        {
            Debug.LogError("Camera rotator not provided");
            return;
        }

        if (projectileDetector == null)
        {
            Debug.LogError("No projectile detector provided");
            return;
        }

        initialRot = cameraRotator.transform.localRotation;

        projectileDetector.OnProjectileDetected += DodgeProjectile;

        cameraRoutine = StartCoroutine(Scan());

        detectionLight.type = LightType.Spot;
        detectionLight.range = detectionRange;
        detectionLight.spotAngle = detectionAngle;
    }

    private void OnDrawGizmos()
    {
        if (cameraOrigin == null || !drawConeGizmos)
            return;

        Gizmos.color = Color.red;

        Vector3 origin = cameraOrigin.position;

        int rays = 20;
        int rings = 4;

        for (int ring = 1; ring <= rings; ring++)
        {
            float distance = detectionRange * ring / rings;

            float radius = Mathf.Tan(detectionAngle * 0.5f * Mathf.Deg2Rad) * distance;

            for (int i = 0; i < rays; i++)
            {
                float angle = i * 360f / rays;

                Vector3 offset =  cameraOrigin.right * Mathf.Cos(angle * Mathf.Deg2Rad) * radius + cameraOrigin.up * Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

                Gizmos.DrawLine(origin, origin + cameraOrigin.forward * distance + offset);
            }
        }

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(cameraOrigin.position, cameraOrigin.position + cameraOrigin.forward * detectionRange);
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            detectionTimer += Time.deltaTime;

            if (!isTrackingPlayer)
            {
                isTrackingPlayer = true;
                if (cameraRoutine != null)
                    StopCoroutine(cameraRoutine);

                cameraRoutine = StartCoroutine(FollowPlayer());
                Debug.Log("Camera sees player");
            }

            if (detectionTimer >= detectionDelay && !wasSeeingPlayer)
            {
                wasSeeingPlayer = true;
                OnPlayerRespawnCondition?.Invoke();
            }
        }
        else
        {
            detectionTimer = 0;
            wasSeeingPlayer = false;

            if (isTrackingPlayer)
            {
                isTrackingPlayer = false;
                if (cameraRoutine != null)
                    StopCoroutine(cameraRoutine);

                cameraRotator.transform.localRotation = initialRot;
                cameraRoutine = StartCoroutine(Scan());
            }
        }
    }

    private IEnumerator FollowPlayer()
    {
        while (isTrackingPlayer)
        {
            Vector3 direction = player.transform.position - cameraOrigin.transform.position;
            Quaternion targetRot = Quaternion.LookRotation(direction);

            cameraRotator.transform.rotation = Quaternion.Slerp(cameraRotator.transform.rotation, targetRot, followDelay * Time.deltaTime);

            yield return null;
        }
    }

    private void DodgeProjectile()
    {
        if (isDodging)
            return;

        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(Dodge());
    }

    private IEnumerator Scan()
    {
        while (true)
        {
            Quaternion targetRot = initialRot * Quaternion.AngleAxis(scanMaxAngle, Vector3.up);

            yield return RotateTo(targetRot, scanDuration);

            yield return new WaitForSeconds(scanPauseTime);

            targetRot = initialRot * Quaternion.AngleAxis(-scanMaxAngle, Vector3.up);

            yield return RotateTo(targetRot, scanDuration);
            yield return new WaitForSeconds(scanPauseTime);
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        cameraRotator.transform.localRotation = initialRot;

        int direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

        Quaternion dodgeRot = initialRot * Quaternion.AngleAxis(direction * dodgeMaxAngle, Vector3.right);

        yield return RotateTo(dodgeRot, dodgeDuration);

        yield return new WaitForSeconds(dodgePauseTime);

        yield return RotateTo(initialRot, dodgeDuration);

        isDodging = false;

        cameraRoutine = StartCoroutine(Scan());
    }

    private IEnumerator RotateTo(Quaternion targetRot, float duration)
    {
        Quaternion startRot = cameraRotator.transform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float interval = Mathf.Clamp01(elapsedTime / duration);

            cameraRotator.transform.localRotation = Quaternion.Slerp(startRot, targetRot, interval);

            yield return null;
        }

        cameraRotator.transform.localRotation = targetRot;
    }

    public void Cover()
    {
        isCovered = true;
        detectionLight.gameObject.SetActive(false);
    }

    private bool CanSeePlayer()
    {
        if (isCovered || isPlayerDisguised || !IsPlayerInView())
            return false;

        Vector3 origin = cameraOrigin.position;
        Vector3 target = player.transform.position;

        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
        {
            Debug.Log($"Camera sees: {hit.transform.name}");

            if (hit.transform.GetComponentInParent<Player>() == player ||
                hit.transform.GetComponentInChildren<Player>() == player ||
                hit.transform.GetComponent<Player>() == player)
                return true;
        }

        return false;
    }

    private bool IsPlayerInView()
    {
        Vector3 directionToPlayer = player.transform.position - cameraOrigin.transform.position;

        if (directionToPlayer.sqrMagnitude > detectionRange * detectionRange)
            return false;

        float angle = Vector3.Angle(cameraOrigin.forward, directionToPlayer);

        return angle <= detectionAngle * 0.5f;
    }
}


public interface ICoverable
{
    void Cover();
}