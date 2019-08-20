﻿namespace RPG.Control {
    public interface IRaycastable {
        CursorType GetCursorType();
        bool HandleRayCast( PlayerController player );
    }
}