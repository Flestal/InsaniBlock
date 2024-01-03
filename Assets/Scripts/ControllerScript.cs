using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] int screen_width, screen_height;
    [SerializeField] RectTransform Controller, Lever;
    [SerializeField] float leverRange;
    bool dragFlag = false;
    //PlayerBlock playerBlock;
    Vector2 controller_position;
    Vector2 controlDir;
    float MoveSpeed;
    private void Awake()
    {
        screen_width = Screen.width;
        screen_height = Screen.height;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 InputPosition = new Vector2((eventData.position.x / screen_width) * 1080, (eventData.position.y / screen_height) * 2340);
        Vector2 inputDir = InputPosition - controller_position;
        Vector2 clampedDir = inputDir.magnitude<leverRange?inputDir:inputDir.normalized*leverRange;
        Lever.anchoredPosition = clampedDir;
        controlDir = clampedDir;
        MoveSpeed = clampedDir.magnitude / leverRange;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        dragFlag = false;
        Controller.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        float refineX = eventData.pressPosition.x/screen_width*1080;
        float refineY = eventData.pressPosition.y/screen_height*2340;
        Vector2 pressPositon_refined = new Vector2(refineX, refineY);
        Controller.anchoredPosition = pressPositon_refined;
        controller_position = Controller.anchoredPosition;
        Vector2 inputDir = pressPositon_refined - controller_position;
        Vector2 clampedDir = inputDir.magnitude<leverRange?inputDir:inputDir.normalized*leverRange;
        Lever.anchoredPosition = clampedDir;
        dragFlag = true;
        Controller.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(dragFlag&&PlayerBlock.Instance.isLive)
        {
            PlayerBlock.Instance.Move(controlDir, PlayerBlock.Instance.speed*MoveSpeed);
        }
    }
}
