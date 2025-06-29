using UnityEngine;

namespace Emesefe.Utilities
{
    public static class Utils
    {
        public const int sortingOrderDefault = 5000; // To appear in front of anything
        
        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 12, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
            color ??= Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        
        // Create Text in the World
        private static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
            GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
            
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            
            return textMesh;
        }
        
        // Create a Text Popup in the World, no parent
        public static void CreateWorldTextPopup(string text, Vector3 localPosition, float popupTime = 1f) {
            CreateWorldTextPopup(null, text, localPosition, 12, Color.white, localPosition + new Vector3(0, 7.5f), popupTime);
        }
        
        // Create a Text Popup in the World
        private static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate () {
                transform.position += moveAmount * Time.unscaledDeltaTime;
                popupTime -= Time.unscaledDeltaTime;

                if (popupTime <= 0f)
                {
                    Object.Destroy(transform.gameObject);
                    return true;
                }

                return false;
                
            }, "WorldTextPopup");
        }
        
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        
        private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}
