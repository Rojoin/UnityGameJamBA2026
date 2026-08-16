
using System.Collections;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private ProjectileDetector _projectileDetector;

    [Header("Rotation")]
    [SerializeField] private GameObject _cameraRotator;

    [Header("Scan Rotation")]
    [SerializeField] private float _scanMaxAngle;
    [SerializeField] private float _scanPauseTime;
    [SerializeField] private float _scanDuration;

    [Header("Dodge rotation")]
    [SerializeField] private float _dodgeMaxAngle;
    [SerializeField] private float _dodgePauseTime;
    [SerializeField] private float _dodgeDuration;

    private bool _isDodging = false;
    private Quaternion _initialRot;
    private Coroutine _cameraRoutine;

    private void Awake()
    {
        if (_cameraRotator == null)
        {
            Debug.LogError("Camera rotator not provided");
            return;
        }    

        if (_projectileDetector == null)
        {
            Debug.LogError("No projectile detector provided");
            return;
        }

        _initialRot = _cameraRotator.transform.localRotation;

        _projectileDetector.OnProjectileDetected += DodgeProjectile;

        _cameraRoutine = StartCoroutine(Scan());
    }

    private void DodgeProjectile()
    {
        if (_isDodging)
            return;

        if (_cameraRoutine != null)
            StopCoroutine(_cameraRoutine);

        _cameraRoutine = StartCoroutine(Dodge());
    }

    private IEnumerator Scan()
    {
        while (true)
        {
            Quaternion targetRot = _initialRot * Quaternion.AngleAxis(_scanMaxAngle, Vector3.up);

            yield return RotateTo(targetRot, _scanDuration);

            yield return new WaitForSeconds(_scanPauseTime);

            targetRot = _initialRot * Quaternion.AngleAxis(-_scanMaxAngle, Vector3.up);

            yield return RotateTo(targetRot, _scanDuration);
            yield return new WaitForSeconds(_scanPauseTime);
        }
    }

    private IEnumerator Dodge()
    {
        _isDodging = true;
        _cameraRotator.transform.localRotation = _initialRot;

        int direction = Random.Range(0, 2) == 0 ? -1 : 1;

        Quaternion dodgeRot = _initialRot * Quaternion.AngleAxis(direction * _dodgeMaxAngle, Vector3.right);

        yield return RotateTo(dodgeRot, _dodgeDuration);

        yield return new WaitForSeconds(_dodgePauseTime);

        yield return RotateTo(_initialRot, _dodgeDuration);

        _isDodging = false;

        _cameraRoutine = StartCoroutine(Scan());
    }

    private IEnumerator RotateTo(Quaternion targetRot, float duration)
    {
        Quaternion startRot = _cameraRotator.transform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float interval = Mathf.Clamp01(elapsedTime / duration);

            _cameraRotator.transform.localRotation = Quaternion.Slerp(startRot, targetRot, interval);

            yield return null;
        }

        _cameraRotator.transform.localRotation = targetRot;
    }
}
