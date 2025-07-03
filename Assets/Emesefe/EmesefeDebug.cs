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
    }
}
