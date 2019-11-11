using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Vector3 popUpPos;
    [SerializeField]
    private TweenHelper popUpTween;
    private Vector3 popDownPos;
    [SerializeField]
    private TweenHelper popDownTween;

    private ItemSO item;
    private ProfileSO profile;
    private int state = 0;
    // 0 = pop-up
    // 1 = wait reply
    // 2 = pop-down

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.transform.position = this.popDownPos;
        this.popUpTween.reset();
        this.popDownTween.reset();
        this.state = 0;
    }

    private void Awake()
    {
        this.popDownPos = this.transform.position;
        this.popUpTween.targetVec = this.popUpPos;
        this.popUpTween.startVec = this.popDownPos;
        this.popDownTween.targetVec = this.popDownPos;
        this.popDownTween.startVec = this.popUpPos;
        this.popUpTween.reset();
        this.state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        this.popUpTween.addTime(delta);
        this.popDownTween.addTime(delta);
        switch (this.state)
        {
            case 0: popUp(); break;
            case 1: awaitInput(); break;
            case 2: popDown(); break;
            default: break;
        }
    }

    private void popUp()
    {
        this.gameObject.transform.position = this.popUpTween.getTweenVec();
        if (this.popUpTween.isDone())
        {
            this.state = 1;
        }
    }
    private void awaitInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.inputRecieved();
        }
    }
    private void popDown()
    {
        this.gameObject.transform.position = this.popDownTween.getTweenVec();
        if (this.popDownTween.isDone())
        {
            this.gameObject.SetActive(false);
        }
    }

    public void inputRecieved()
    {
        this.state = 2;
        this.popDownTween.reset();
    }
    public void setUpItem(ItemSO item)
    {
        this.item = item;
        // Set up UI

        this.gameObject.SetActive(true);
    }
    public void setUpProfile(ProfileSO profile)
    {
        this.profile = profile;
        // Set up UI

        this.gameObject.SetActive(true);
    }
}
