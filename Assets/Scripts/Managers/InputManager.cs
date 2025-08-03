using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// This input manager was made for the game Power Nap, and its speciic function implementations are based on that games functionality.
/// When you adapt this for another game, modify the actions / function / variable names to be compatible with the game you're making.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private PlayerInput _playerInput;

    [Header("Input Actions")]
    //Overworld
    private InputAction _work;
    private InputAction _sleep;
    //Dreamworld
    private InputAction _move;
    private InputAction _melee;
    private InputAction _fireball;
    //Mixed
    private InputAction _pause;
    private InputAction _restart;

    [Header("Action Access Variables")]
    //Overworld
    public bool workJustPressed;
    public bool sleepJustPressed;
    //Dreamworld
    public Vector2 moveInput;
    public bool meleeJustPressed;
    public bool fireballJustPressed;
    //Mixed
    public bool pauseJustPressed;
    public bool restartJustPressed;

    private void SetupInputActions()
    {
        //Overworld
        _work = _playerInput.actions["Work"];
        _sleep = _playerInput.actions["Sleep"];
        //Dreamworld
        _move = _playerInput.actions["Move"];
        _melee = _playerInput.actions["Attack"];
        _fireball = _playerInput.actions["Fireball"];
        //Mixed
        _pause = _playerInput.actions["Pause"];
        _restart = _playerInput.actions["Restart"];
    }

    private void UpdateInputs()
    {
        //Hardcode work
        // Loop through A to Z
        bool workPressed = false;
        for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
        {
            if (Input.GetKeyDown(key))
            {
                workPressed = true;
            }
        }
        //Overworld
        workJustPressed = workPressed;
        sleepJustPressed = _sleep.WasPressedThisFrame();
        //Dreamworld
        moveInput = _move.ReadValue<Vector2>();
        meleeJustPressed = _melee.WasPressedThisFrame();
        fireballJustPressed = _fireball.WasPressedThisFrame();
        //Mixed
        pauseJustPressed = _pause.WasPressedThisFrame();
        restartJustPressed = _restart.WasPressedThisFrame();
    }    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }
}
