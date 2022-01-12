using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TMP_Text resourceText;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(GameManager.instance.DestroyWithDelay(5, this.gameObject));
    }


    private void Update()
    {
        if (GetComponent<RectTransform>())
            GetComponent<RectTransform>().anchoredPosition += Vector2.up * _speed * Time.deltaTime;
        else
            transform.position += transform.up * _speed * Time.deltaTime;



        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, Time.deltaTime * _speed);
    }

    public void SetResourceText(int amount)
    {
        if (amount < 0)
            resourceText.color = Color.red;
        else
            resourceText.color = Color.green;

        resourceText.text = amount > 0 ? $"+{amount}" : amount.ToString(); 
    }
}
