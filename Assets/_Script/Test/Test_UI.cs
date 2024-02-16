using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UI : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.StaminaDamege(10.0f);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.TestHPSet(-100.0f);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.TestHPSet(10.0f);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        player.TestIsGuard(true);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        player.TestIsGuard(false);

    }
}
