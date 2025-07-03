using UnityEngine;
using TMPro;

namespace Emesefe.Utilities
{
    public static class Utils
    {
        private const int SortingOrderDefault = 5000; // To appear in front of anything
        
        private const int DefaultFontSize = 12;
        private static readonly Color DefaultFontColor = Color.white;
        
        private static readonly Vector3 DefaultPopupVerticalOffset = new Vector3(0, 10f, 0);
        
        // Create Text TMPro in the World
        public static TextMeshPro CreateWorldTextTMPro(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 12, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.TopLeft, int sortingOrder = SortingOrderDefault) {
            color ??= DefaultFontColor;
            return CreateWorldTextTMPro(parent, text, localPosition, fontSize, (Color)color, textAlignment, sortingOrder);
        }
        
        // Create Text TMPro in the World
        private static TextMeshPro CreateWorldTextTMPro(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAlignmentOptions textAlignment,  int sortingOrder) {
            GameObject gameObject = new GameObject("World Text", typeof(TextMeshPro));
            
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            
            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.alignment = textAlignment;
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color;
            
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            
            return textMeshPro;
        }
        
        // Create a Text TMPro Popup in the World, no parent
        public static void CreateWorldTextTMProPopup(string text, Vector3 localPosition, float popupTime = 1f) {
            CreateWorldTextTMProPopup(null, text, localPosition, DefaultFontSize, DefaultFontColor, 
                localPosition + DefaultPopupVerticalOffset, popupTime);
        }
        
        // Create a Text TMPro Popup in the World
        private static void CreateWorldTextTMProPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
            TextMeshPro textMeshPro = CreateWorldTextTMPro(parent, text, localPosition, fontSize, color, TextAlignmentOptions.BaselineLeft, SortingOrderDefault);
            Transform transform = textMeshPro.transform;
            
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(() => {
                transform.position += moveAmount * Time.unscaledDeltaTime;
                popupTime -= Time.unscaledDeltaTime;

                if (popupTime <= 0f)
                {
                    Object.Destroy(transform.gameObject);
                    return true;
                }

                return false;
                
            }, "WorldTextTMProPopup");
        }
        
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 mousePosition = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            mousePosition.z = 0f;
            return mousePosition;
        }
        
        // Get Mouse Position in World
        private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera camera) {
            Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}
