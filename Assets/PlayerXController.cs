using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerXController : MonoBehaviour
{
    NewControls controls;
    public Player_Main playerMain;

    Vector2 move;

    private void Awake()
    {
        controls = new NewControls();

        controls.Gameplay.Attack.performed += ctx => playerMain.attackMain();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);
    }

}
