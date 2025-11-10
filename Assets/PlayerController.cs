using UnityEngine;
using Inputs;
using UnityEngine.InputSystem;
using Unity.Multiplayer.Center.Common.Analytics;
using System.Collections;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CharacterInput _input;
    private CharacterController _controller;
    private GameObject _mainCamera;

    private scan _scanner; 

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;


    [SerializeField] float MoveSpeed = 5.0f;
    [SerializeField] float WalkSpeed = 2.0f;

    public float SpeedChangeRate = 10.0f;
    private float _speed;

    private float _rotationVelocity;
    private float _targetRotation = 0.0f;
    public float RotationSmoothTime = 0.12f;

    public float CameraSensivity = 1.0f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    float stepCounter = 0;

    Player _player;

    [SerializeField] private Canvas _canvas;

    LayerMask layerMask;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<CharacterInput>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
        _player = GetComponent<Player>();

        _scanner = GetComponent<scan>();
        

        layerMask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();
        Interact();
        AttackAndOther();
    }

    void AttackAndOther()
    {
        if(_input.scan>0)
        {
            var a = _canvas.transform.Find("clap").gameObject;
            _scanner.StartWave();
            StartCoroutine(ShowImage(a));
            
        }
    }

    IEnumerator ShowImage(GameObject img)
    {
        img.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        img.SetActive(false);

        yield return null;
    }

    void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 5.0f, layerMask))
        {
            hit.transform.gameObject.GetComponent<IInteractable>().OnHover();
        }
        if(_input.interact>0)
        {
            if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 5.0f, layerMask))
            {
                hit.transform.gameObject.GetComponent<IInteractable>().OnInteract();   
            }
        }
    }

    void Look()
    {



        float deltaTimeMultiplier = 1.0f;

        _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * CameraSensivity * 0.1f;
        _cinemachineTargetPitch += -1 * _input.look.y * deltaTimeMultiplier * CameraSensivity * 0.1f;

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        transform.rotation = Quaternion.Euler(0, _cinemachineTargetYaw, 0);

        _mainCamera.transform.rotation = Quaternion.Euler(
            _cinemachineTargetPitch + 0.0f,
            _cinemachineTargetYaw,
            0.0f
        );

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void Move()
    {
        float targetSpeed = MoveSpeed;
        if (_input.walk > 0)
        {
            targetSpeed = WalkSpeed;
        }

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        //kontrolcü desteğini saldım
        float inputMagnitude = _input.move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;

        }
        else
        {
            _speed = targetSpeed;

        }


        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero && Time.timeScale > 0)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, 0, 0.0f) * Time.deltaTime);

        if (currentHorizontalSpeed > WalkSpeed + speedOffset)
        {
            stepCounter += Time.deltaTime;
            if (stepCounter >= 0.3f)
            {
                //GetComponent<scan>().StartWave(duration: GetComponent<scan>().duration / 3, size: GetComponent<scan>().size / 3);
                Vector3 wavePos = transform.position + transform.forward * 1.5f;
                _scanner.StartWave(duration: 3f, size: 5f, simSpeed: 4, position: wavePos);
                stepCounter = 0f;
            }
        }
        else
        {
            stepCounter = 0f;
        }
    }



}
