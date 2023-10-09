
public class InputService : Service
{
    public PlayerActions playerInputActions;
    
    public InputService()
    {
        playerInputActions = new PlayerActions();
        playerInputActions.Enable();
    }

    public void DisablePlayerInput()
    {
        playerInputActions.Disable();
    }

    public void EnablePlayerInput()
    {
        playerInputActions.Enable();
    }
}

