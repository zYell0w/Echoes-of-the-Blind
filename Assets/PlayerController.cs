using UnityEngine;
using Inputs;
using UnityEngine.InputSystem;
using Unity.Multiplayer.Center.Common.Analytics;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System;
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

    [SerializeField] Sprite[] clapImageSprite;
    [SerializeField] GameObject clap;
    private Image clapImage;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private float frameDelay = 0.1f;




    public float SpeedChangeRate = 10.0f;
    private float _speed;

    private float _rotationVelocity;
    private float _targetRotation = 0.0f;
    public float RotationSmoothTime = 0.12f;

    public float CameraSensivity = 1.0f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private float Gravity = -15f;

    [SerializeField] private GameObject _menu;

    float stepCounter = 0;

    Player _player;

    [SerializeField] private float InteractRange = 2.0f; 

    [SerializeField] private Canvas _canvas;

    public bool Grounded = true;

    public float GroundedOffset = 0.7f;

    public float GroundedRadius = 0.5f;

    public LayerMask GroundLayers;

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
        if(look_enabled)
            Look();
        Move();

        if(interact_enabled)
            Interact();

        if(attack_enabled)
            AttackAndOther();
        GroundedCheck();
        GravityAnd();
        Menu();
    }

    private void Menu()
    {
        if(_input.menu)
        {
            _menu.SetActive(!_menu.activeSelf);
            _input.menu=false;
        }
    }

    void GravityAnd()
    {
        if (Grounded)
        {
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
        }
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    float scanCooldown = 1f;
    float scanCounter = 0f;
    public bool look_enabled = true;
    public bool interact_enabled = true;
    public bool attack_enabled = true;



    void AttackAndOther()
    {
        scanCounter+=Time.deltaTime;
        if (_input.scan > 0 && scanCounter >= scanCooldown)
        {
            //var a = _canvas.transform.Find("clap").gameObject;
            //_scanner.StartWave();
            StartCoroutine(CreateDoubleScan());
            StartCoroutine(ShowImage(clap));
            AudioManager.instance.Play("FingerSnapSound");
            scanCounter=0;
        }

        if (_input.drop > 0)
        {
            if (_player.Item != null)
                _player.Item.Drop(_player);
        }

        if (_input.attack > 0)
        {
            if (_player.Weapon != null)
                _player.Weapon.Use();
        }
    }
    IEnumerator CreateDoubleScan()
    {
        _scanner.StartWave();
        yield return new WaitForSeconds(0.5f);
        _scanner.StartWave();

        yield return null;
    }
    IEnumerator ShowImage(GameObject clapImageObject)
    {
        clapImage = clap.GetComponent<Image>();
        clapImageObject.SetActive(true);
        if (isPlaying == false) {

            isPlaying = true;
            while (currentFrame < clapImageSprite.Length)
            {
                clapImage.sprite = clapImageSprite[currentFrame];
                currentFrame++;
                yield return new WaitForSeconds(frameDelay);
            }
            currentFrame = 0;
            isPlaying = false;
            clapImageObject.SetActive(false);
        }



        /*
        img.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        img.SetActive(false);
        */
        yield return null;
    }

    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, InteractRange, layerMask))
        {
            hit.transform.gameObject.GetComponent<IInteractable>().OnHover();
        }
        if (_input.interact > 0)
        {
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, InteractRange, layerMask))
            {
                hit.transform.gameObject.GetComponent<IInteractable>().OnInteract(_player);
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

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character 
        /*
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
        */
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
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (currentHorizontalSpeed > WalkSpeed + speedOffset)
        {
            stepCounter += Time.deltaTime;
            if (stepCounter >= 0.5f)
            {
                //GetComponent<scan>().StartWave(duration: GetComponent<scan>().duration / 3, size: GetComponent<scan>().size / 3);
                Vector3 wavePos = transform.position + targetDirection.normalized * 1.5f;
                AudioManager.instance.Play("Walking");
                _scanner.StartWave(duration: 3f, size: 5f, simSpeed: 4, position: wavePos);
                
                stepCounter = 0f;
            }
        }
        else
        {
            stepCounter = 0f;
        }
    }

    /*
    // CharacterController bir şeye çarptığında bu çalışır
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        // "Enemy" tag'ine sahip bir şeye mi çarptık?
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("TEMAS VAR.");

            // Eğer Player scriptin varsa ve içinde Die fonksiyonu varsa:
            if (_player != null)
            {
                _player.Die(); // Buradaki fonksiyon ismin neyse onu yaz
            }
        }
    }
    */
    public void OnTriggerEnter(Collider other)
    {
    
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("TEMAS VAR.");

            // Eğer Player scriptin varsa ve içinde Die fonksiyonu varsa:
            if (_player != null)
            {
                _player.Die(); // Buradaki fonksiyon ismin neyse onu yaz
                //hüseyin mal aq
            }
        }
    }

}
