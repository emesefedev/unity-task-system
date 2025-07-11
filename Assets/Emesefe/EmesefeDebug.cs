using UnityEngine;
using Emesefe.Utilities;

namespace Emesefe
{
    public static class EmesefeDebug
    {
        private static readonly Vector3 DefaultOffset = .25f * Vector3.one;
        
        // Text Popup appears on Mouse Position
        public static void TextPopupMouse(string text, Vector3? offset = null)
        {
            offset ??= DefaultOffset;
            Utils.CreateWorldTextTMProPopup(text, Utils.GetMouseWorldPosition() + (Vector3)offset);
        }
        
        // Text Popup at the world position
        public static void TextPopup(string text, Vector3 position, float popupTime = 1f) {
            Utils.CreateWorldTextTMProPopup(text, position, popupTime);
        }

    }
}
