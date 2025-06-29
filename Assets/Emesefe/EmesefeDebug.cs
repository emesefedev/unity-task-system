using UnityEngine;
using Emesefe.Utilities;

namespace Emesefe
{
    public static class EmesefeDebug
    {
        public static void TextPopupMouse(string text, Vector3? offset = null)
        {
            offset ??= .25f * Vector3.one;
            Utils.CreateWorldTextPopup(text, Utils.GetMouseWorldPosition() + (Vector3)offset);
        }
    }
}
