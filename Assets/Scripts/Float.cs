using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField]
    private TweenHelper upDown;
    [SerializeField]
    private TweenHelper rotate;
    // Start is called before the first frame update
    void Awake()
    {
        upDown.targetVec = this.transform.position + new Vector3(0, 0.25f, 0);
        upDown.startVec = this.transform.position - new Vector3(0, 0.25f, 0);
        rotate.startVal = -7;
        rotate.targetVal = 7;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        upDown.addTime(delta);
        rotate.addTime(delta);
        this.transform.position = upDown.getTweenVec();
        this.transform.localRotation = Quaternion.Euler(0, 0, rotate.getTweenVal());
    }
}
