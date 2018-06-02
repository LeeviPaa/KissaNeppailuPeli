using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventScrollTo : MonoBehaviour, IUpdateSelectedHandler {

	private static float scroll_margin = 0.3f;

    private ScrollRect thisScrollRect;
    private void Awake()
    {
        thisScrollRect = GetComponent<ScrollRect>();
    }
    public void OnUpdateSelected(BaseEventData eventData)
    {
        Debug.Log("asdasd");
        float contHeight = thisScrollRect.content.rect.height;
        float viewportHeight = thisScrollRect.content.rect.height;

        float centerLine = eventData.selectedObject.transform.localPosition.y;
        float upperBound = centerLine + (eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f);
        float lowerBound = centerLine - (eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f);

        float lowerVisible = (contHeight - viewportHeight) * thisScrollRect.normalizedPosition.y - contHeight;
        float upperVisible = lowerVisible + viewportHeight;

        float supposedLowerBound;
        if (upperBound > upperVisible)
        {
            supposedLowerBound = upperBound - viewportHeight + eventData.selectedObject.GetComponent<RectTransform>().rect.height * scroll_margin;
        }
        else if (lowerBound < lowerVisible)
        {
            supposedLowerBound = lowerBound - eventData.selectedObject.GetComponent<RectTransform>().rect.height * scroll_margin;
        }
        else
            return;

        float normalizedSupposed = (supposedLowerBound + contHeight) / (contHeight - viewportHeight);
        thisScrollRect.normalizedPosition = new Vector2(0, Mathf.Clamp(normalizedSupposed, 0, 1));
    }
}
