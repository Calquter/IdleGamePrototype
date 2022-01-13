using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Building _building;
    public Building getBuilding { get { return _building; }}

    [SerializeField] private Image _buildImage;
    [SerializeField] private TMP_Text _buildGoldCost;
    [SerializeField] private TMP_Text _buildGemCost;

    private Vector2 _startPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _startPos = _rectTransform.anchoredPosition;

        _buildImage.sprite = _building.type.constructionSprite;
        _buildGoldCost.text = _building.type.goldCost.ToString();
        _buildGemCost.text = _building.type.gemCost.ToString();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = .5f;
        GameManager.instance.constructionManager.isBuildingDragging = true;
        GameManager.instance.SelectBuilding(_building);
    }
     
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        _rectTransform.anchoredPosition = _startPos;
        GameManager.instance.constructionManager.isBuildingDragging = false;
    }
}
