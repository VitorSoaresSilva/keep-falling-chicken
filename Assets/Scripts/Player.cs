using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum Height{ Down, Middle, Up }
    private Vector3 direction;
    private float initialPositionTrack;
    private float initialPositionHeight;
    [SerializeField] private float timeToMoveSides = 0.2f;
    [SerializeField] private float timeToMoveHeight = 0.2f;
    [SerializeField] private float timeToStayHeight = 0.2f;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isMovingHeight = false;
    private GameManager.Track currTrack;
    private Height currHeight;

    private void Start(){
        currTrack = GameManager.Track.Middle;
    }
    private void Update(){
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");
        if(horizontal != 0 && !isMoving){
            int desiredTrack = (int)currTrack + horizontal;
            if(desiredTrack > 2 || desiredTrack < 0) return;
            currTrack = (GameManager.Track)desiredTrack;
            isMoving = true;
            initialPositionTrack = transform.position.x;
            StartCoroutine(LerpMoveSides(timeToMoveSides,horizontal));
        }
        if(vertical != 0 && !isMovingHeight){
            isMovingHeight = true;
            initialPositionHeight = transform.position.y;
            StartCoroutine(LerpFunctionHeight(timeToMoveHeight,vertical));
        }
    }
    IEnumerator LerpMoveSides(float duration,int direction){ //ok
        float currTime = 0,lerpValue,temp;
        float endValue = GameManager.Instance.scaleToTrackMove * direction;
        while(currTime < duration){
            lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            temp = lerpValue + initialPositionTrack;
            transform.position = new Vector3(temp,transform.position.y,0);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(endValue + initialPositionTrack,transform.position.y,0);
        isMoving = false;
    }
    IEnumerator LerpFunctionHeight(float duration,float direction){// ta errado
        float currTime = 0,lerpValue,temp;
        float endValue = GameManager.Instance.scaleToHeightMove * direction;
        while(currTime < duration){
            lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            temp = lerpValue + initialPositionHeight;
            transform.position = new Vector3(transform.position.x,temp,0);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x,endValue,0);
        yield return new WaitForSeconds(timeToStayHeight);
        currTime = 0;
        lerpValue = 0;
        while(currTime < duration){
            lerpValue = Mathf.Lerp(0,endValue,currTime/duration);
            temp = endValue - lerpValue;
            transform.position = new Vector3(transform.position.x,temp,0);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x,0,0);
        isMovingHeight = false;
    }

    

}
