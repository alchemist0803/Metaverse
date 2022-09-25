using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class LeftRotationBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Range(1.0f, 1000.0f)]
    public float seconds = 100.0f;
    public UnityEvent onPressedOverSeconds;


    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(TrackTimePressed());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator TrackTimePressed()
    {
        float time = 0;
        this.seconds = 100.0f;

        while (time < seconds)
        {
            time += Time.deltaTime;
            CustomManager.sInstance.m_showCharacter.transform.Rotate(Vector3.up * 2);
            yield return null;
        }
        onPressedOverSeconds.Invoke();
    }
}
