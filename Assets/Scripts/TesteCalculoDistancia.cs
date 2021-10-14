using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteCalculoDistancia : MonoBehaviour
{
    public GameObject spawnable;

    public float speed;

    public float distance;

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.speedMovement = speed;
        Invoke(nameof(ChangeSpeed),CalculateTime());
        Transform[] childs = prefab.GetComponentsInChildren<Transform>();
        foreach (var child in childs)
        {
            Vector3 temp = new Vector3(GameManager.Instance.scaleToTrackMove, GameManager.Instance.scaleToHeightMove,
                0);
            child.position = Vector3.Scale(child.transform.position, temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeSpeed()
    {
        GameManager.Instance.speedMovement = 0;
    }

    private float CalculateTime()
    {
        float time = distance / speed;
        Debug.Log(time);
        return time;
    }
}
