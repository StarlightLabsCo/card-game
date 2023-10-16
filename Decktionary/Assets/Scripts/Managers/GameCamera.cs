using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Starlight.Managers
{
    public class GameCamera : SingletonBehaviour<GameCamera>
    {
        public Vector2 CursorPos { get; private set; }
        public Vector2 CursorPosWorld { get; private set; }

        [SerializeField] Camera cam;

        public void OnCursorPosUpdated(CallbackContext _context)
        {
            CursorPos = _context.ReadValue<Vector2>();
        }

        private void Update()
        {
            CursorPosWorld = cam.ScreenToWorldPoint(CursorPos);
        }
    }
}