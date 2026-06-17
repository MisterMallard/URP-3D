using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
	[SerializeField] private InputActionAsset input;
	[SerializeField] private float walkSpeed = 5f;
	[SerializeField] private float turnSpeed = 150f;
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private string mapName = "Player";

	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction sprintAction;

	private Rigidbody rb;
	[SerializeField] private bool isGrounded = false;

	private Animator animator;

	void Awake()
	{
		InputActionMap map = input.FindActionMap(mapName);
		moveAction = map.FindAction("Move");
		jumpAction = map.FindAction("Jump");
		sprintAction = map.FindAction("Sprint");
		rb = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
	}

	void OnEnable() { input.FindActionMap(mapName).Enable(); }
	void OnDisable() { input.FindActionMap(mapName).Disable(); }

	void Update()
	{
		// Opvragen van de input
		Vector2 moveInput = moveAction.ReadValue<Vector2>();

		// Bepalen wat de snelheid is
		float speed = walkSpeed * moveInput.y;

		// Sprinten
		if (sprintAction.IsPressed())
			speed *= 2f;

		// Bewegen van de speler
		Vector3 movement = transform.forward * speed * Time.deltaTime;
		transform.Translate(movement, Space.World);

		// Draaien van de speler
		float angle = moveInput.x * turnSpeed * Time.deltaTime;
		transform.Rotate(0f, angle, 0f, Space.World);

		// Springen
		if (jumpAction.WasPressedThisFrame() && isGrounded)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			isGrounded = false;

			// Spring animatie triggeren
			animator.SetTrigger("JumpTrigger");
		}

		// Loop en sprint animaties aansturen
		animator.SetFloat("Speed", speed);
		//animator.SetBool("Grounded", isGrounded);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = true;
	}
}