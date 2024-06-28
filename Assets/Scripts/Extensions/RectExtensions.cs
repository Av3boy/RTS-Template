using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class RectExtensions
    {
        public static Rect GetRectFromMousePositions(Vector2 startPosition, Vector2 endPosition)
        {
            // Calculate the correct width and height
            float width = Mathf.Abs(endPosition.x - startPosition.x);
            float height = Mathf.Abs(endPosition.y - startPosition.y);

            // Calculate the correct x and y positions for the Rect
            float x = Mathf.Min(startPosition.x, endPosition.x);
            float y = Mathf.Max(startPosition.y, endPosition.y); // Flip the y-coordinates to match the screen coordinate system

            Rect rect = new Rect(x, y, width, height);
            return rect;
        }
    }
}
