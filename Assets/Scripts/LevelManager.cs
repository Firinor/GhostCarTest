#if UNITY_EDITOR
using UnityEditor;
#endif
using Ashsvp;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private InputSystem_Actions input;
    [SerializeField] 
    private SimcadeVehicleController player;
    [SerializeField] 
    private GhostManager GhostManager;
    [SerializeField] 
    private Transform playerStartPosition;
    public InputSystem_Actions Input => input;

    private void Awake()
    {
        input = new();
        ResetPlayer();
        GhostManager.StartNewRace(player.transform);
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input?.Disable();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;  
#else
        Application.Quit();
#endif 
    }
    public void ResetPlayer()
    {
        player.ResetVehicle(playerStartPosition);
    }
}