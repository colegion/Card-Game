using UnityEngine;
using UnityEngine.InputSystem;

namespace Helpers
{
    public class CardTapHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private GameInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new GameInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Gameplay.Tap.performed += OnTapPerformed;
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Tap.performed -= OnTapPerformed;
            _inputActions.Disable();
        }

        private void OnTapPerformed(InputAction.CallbackContext context)
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var card = hit.collider.GetComponent<Card>();
                if (card != null)
                {
                    Debug.Log($"Tapped on card: {card.name}");
                    //card.OnTapped(); // Call a method to handle the card tap
                }
            }
        }
    }
}