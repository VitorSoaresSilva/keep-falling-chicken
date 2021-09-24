using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Move {Right,Top, Left, Bottom, None}
public enum Track {Left, Middle, Right}
public enum Height{Bottom,Middle,Top}
public class Player : MonoBehaviour
{
    private Vector3 initialTouch;
    private Vector3 finalTouch;
    private bool hasTouched = false;
    private Move desiredMove;
    private bool isMovingTrack;
    private bool isMovingHeight;
    [SerializeField] private float timeToStayHeight = 2;
    [SerializeField] private float minMov = 40; // change to some % of screen width

    private Track currTrack;
    private Height currHeight;
    [SerializeField] private float durationMoveHeight = .2f;
    [SerializeField] private float durationMoveTrack = .2f;

    private void Start()
    {
        currTrack = Track.Middle;
        currHeight = Height.Middle;
    }
    void Update()
    {
        if (Input.touchCount <= 0) return;
        
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                initialTouch = touch.position;
                break;
            case TouchPhase.Ended:
                finalTouch = touch.position;
                hasTouched = true;
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Canceled:
                break;
        }

        if (!hasTouched) return;
        Vector2 swipeVector = finalTouch - initialTouch;
        if (swipeVector.magnitude < minMov) return;
        Vector2 swipeVectorNormalized = swipeVector.normalized;
        desiredMove = ConvertDirectionToMove(swipeVectorNormalized);
        TryMove(desiredMove);
        hasTouched = true;
        desiredMove = Move.None;
    }
    private void TryMove(Move move)
    {
        Vector2 direction = ConvertMoveToDirection(move);
        switch (move)
        {
            case Move.Right:
            case Move.Left:
                Track desiredTrack = (Track)(direction.x + (int)currTrack);
                if (!isMovingTrack && !(desiredTrack > Track.Right || desiredTrack < Track.Left))
                {
                    currTrack = desiredTrack;
                    StartCoroutine(ChangeTrack(durationMoveTrack, direction));
                }
                break;
            case Move.Bottom:
            case Move.Top:
                Height desiredHeight = (Height)(direction.y + (int)currHeight);
                if (!isMovingHeight && !(desiredHeight > Height.Top || desiredHeight < Height.Bottom))
                {
                    currHeight = desiredHeight;
                    
                    StartCoroutine(ChangeHeight(durationMoveHeight, direction)); // salvar numa var pra poder voltar e cancelar
                }
                break;
            case Move.None:
                break;
        }
    }
    IEnumerator ChangeTrack(float duration,Vector2 direction)
    {
        isMovingTrack = true;
        float initialPositionTrack = transform.position.x;
        float currTime = 0;
        float endValue = GameManager.Instance.scaleToTrackMove * direction.x;
        while(currTime < duration){
            float lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            float temp = lerpValue + initialPositionTrack;
            transform.position = new Vector3(temp,transform.position.y,0);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(endValue + initialPositionTrack,transform.position.y,0);
        isMovingTrack = false;
    }
    IEnumerator ChangeHeight(float duration,Vector2 direction){
        isMovingHeight = true;
        float initialPositionHeight = transform.position.y;
        float currTime = 0;
        float endValue = GameManager.Instance.scaleToTrackMove * direction.y;
        while(currTime < duration){
            float lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            float temp = lerpValue + initialPositionHeight;
            transform.position = new Vector3(transform.position.x, temp ,0);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x,endValue + initialPositionHeight,0);
        isMovingHeight = false;
        yield return new WaitForSeconds(timeToStayHeight);
        initialPositionHeight = transform.position.y;
        currTime = 0;
        while(currTime < duration){
            float lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            float temp = initialPositionHeight - lerpValue;
            transform.position = new Vector3(transform.position.x, temp ,0);
            currTime += Time.deltaTime;
            yield return null;
        }
    }
    private Vector2 ConvertMoveToDirection(Move move)
    {
        float theta = (float)move * (2 * Mathf.PI) / 4;
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    private Move ConvertDirectionToMove(Vector2 direction)
    {
        return (Move)(Mathf.Round(4 * Mathf.Atan2(direction.y, direction.x) / (2 * Mathf.PI) + 4) % 4);
    }
}
