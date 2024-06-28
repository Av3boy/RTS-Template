using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _selectedUnitsText;
        public TextMeshProUGUI SelectedUnitsText { get => _selectedUnitsText; set => _selectedUnitsText = value; }

        [SerializeField]
        private RectTransform _selectionRect;
        public RectTransform SelectionRect { get => _selectionRect; set => _selectionRect = value; }

        public Rect? MouseDragRect { get; set; }

        private void Awake()
        {
            SelectedUnitsText = GameObject.Find("SelectedUnitsText").GetComponent<TextMeshProUGUI>();
        }

        private void OnGUI()
        {
            if (MouseDragRect.HasValue)
            {
                SelectionRect.gameObject.SetActive(true);

                SelectionRect.sizeDelta = new Vector2(MouseDragRect.Value.width, MouseDragRect.Value.height);
                SelectionRect.transform.position = new Vector2(MouseDragRect.Value.x, MouseDragRect.Value.y);
            }
            else
            {
                SelectionRect.gameObject.SetActive(false);
            }
        }
    }
}