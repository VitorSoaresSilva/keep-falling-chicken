using System;
using System.Collections;
using UnityEngine;
public enum Move {Right,Top, Left, Bottom, None}
public enum Track {Left, Middle, Right}
public enum Height{Bottom,Middle,Top}
public class Player : MonoBehaviour
{
    public int Money { get; private set; }
    
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

    
    // [SerializeField] private PlayerManager playerController;

    private bool alreadyDamaged = false;

    [SerializeField] private float timeBetweenDamages;
    // Start is called before the first frame update
    private void Start()
    {
        currTrack = Track.Middle;
        currHeight = Height.Middle;
    }
    // Update is called once per frame
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
            default:
                throw new ArgumentOutOfRangeException();
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

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GoldComponent goldComponent))
        {
            GameManager.Instance.EarnGold(goldComponent.gold);
            Destroy(other.gameObject);
        }
        else if (other.TryGetComponent(out PowerUpColetavel powerUpColetavel))
        {
            PowerUpsManager.Instance.powerUps[powerUpColetavel.type].Collect();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Obstacle"))
        {
            if (PowerUpsManager.Instance.powerUps[(int)PowerUpTypes.dash].inUse)
            {
                //effect
                Destroy(other.gameObject);
            }
            else
            {
                if (alreadyDamaged)
                {
                    //end game
                    GameManager.Instance.FinishRun();
                }
                else
                {
                    alreadyDamaged = true;
                    StartCoroutine(TimeToSecondDamage());
                }
                //effect
                Destroy(other.gameObject);
            }
            //take damage
            //if it is the second in a few seconds -> end game
            /* if is the first ->
             * animation
             * sound effect
             * particles effects
             * velocity -= some value
             */
        }
    }

    IEnumerator TimeToSecondDamage()
    {
        yield return new WaitForSeconds(timeBetweenDamages);
        alreadyDamaged = false;
    }

    #endregion

    #region MovementFunctions

    

    
    private Vector2 ConvertMoveToDirection(Move move)
    {
        float theta = (float)move * (2 * Mathf.PI) / 4;
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    private Move ConvertDirectionToMove(Vector2 direction)
    {
        return (Move)(Mathf.Round(4 * Mathf.Atan2(direction.y, direction.x) / (2 * Mathf.PI) + 4) % 4);
    }

    private void TryMove(Move move)
    {
        Vector2 direction = ConvertMoveToDirection(move);
        Debug.Log("direction: " + direction);
        switch (move)
        {
            case Move.Right:
            case Move.Left:
                Track desiredTrack = (Track)(direction.x + (int)currTrack);
                if (!isMovingTrack && !(desiredTrack > Track.Right || desiredTrack < Track.Left))
                {
                    currTrack = desiredTrack;
                    isMovingTrack = true;
                    StartCoroutine(ChangeTrack(durationMoveTrack, direction));
                }
                break;
            case Move.Bottom:
            case Move.Top:
                Height desiredHeight = (Height)(direction.y + (int)currHeight);
                Debug.Log("Desired heigth " + desiredHeight);
                if (!isMovingHeight && !(desiredHeight > Height.Top || desiredHeight < Height.Bottom))
                {
                    currHeight = desiredHeight;
                    isMovingHeight = true;
                    StartCoroutine(ChangeHeight(durationMoveHeight, direction)); // salvar numa var pra poder voltar e cancelar
                }
                break;
            case Move.None:
                break;
        }
    }
    

    IEnumerator ChangeTrack(float duration,Vector2 direction)
    {
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

    IEnumerator ChangeHeight(float duration, Vector2 direction)
    {
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
        transform.position = new Vector3(transform.position.x,endValue - initialPositionHeight,0);
        currHeight = Height.Middle;
        isMovingHeight = false;
    }
    #endregion
}
