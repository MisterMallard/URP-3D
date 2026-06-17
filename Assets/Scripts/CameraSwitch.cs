using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
	[SerializeField] private CinemachineCamera followCam;
	[SerializeField] private CinemachineCamera overviewCam;
	[SerializeField] private InputActionAsset inputAsset;
	private InputAction switchAction;
	private bool overviewActive = false;

	void Awake()
	{
		switchAction = inputAsset.FindActionMap("Player").FindAction("SwitchCamera");
	}

	void OnEnable() { inputAsset.FindActionMap("Player").Enable(); }
	void OnDisable() { inputAsset.FindActionMap("Player").Disable(); }

	void Update()
	{
		if (switchAction.WasPressedThisFrame())
		{
			overviewActive = !overviewActive;
			followCam.Priority = overviewActive ? 0 : 10;
			overviewCam.Priority = overviewActive ? 10 : 0;
		}
	}
}