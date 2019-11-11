using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionUI : MonoBehaviour {
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

    [SerializeField]
    private DialogueBox dialogueBox;

    // Start is called before the first frame update
    private void OnEnable() {
        this.transform.GetComponent<RectTransform>().localPosition = this.popDownPos;
        this.popUpTween.reset();
        this.popDownTween.reset();
        this.state = 0;
    }

    private void Awake() {
        this.popDownPos = this.transform.GetComponent<RectTransform>().localPosition;
        this.popUpTween.targetVec = this.popUpPos;
        this.popUpTween.startVec = this.popDownPos;
        this.popDownTween.targetVec = this.popDownPos;
        this.popDownTween.startVec = this.popUpPos;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        float delta = Time.deltaTime;
        this.popUpTween.addTime(delta);
        this.popDownTween.addTime(delta);
        switch (this.state) {
            case 0: PopUp(); break;
            case 1: AwaitInput(); break;
            case 2: PopDown(); break;
            default: break;
        }
    }

    private void PopUp() {
        this.gameObject.transform.GetComponent<RectTransform>().localPosition = this.popUpTween.getTweenVec();
        if (this.popUpTween.isDone()) {
            this.state = 1;
        }
    }

    private void AddCrew(int pos) {
        gameManager.AddCrew(pos);
        this.InputRecieved();
    }

    private void AwaitInput() {
        if (this.profile) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                AddCrew(0);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                AddCrew(1);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                AddCrew(2);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                AddCrew(3);
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                AddCrew(4);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            this.InputRecieved();
        }
    }

    private void PopDown() {
        this.gameObject.transform.GetComponent<RectTransform>().localPosition = this.popDownTween.getTweenVec();
        if (this.popDownTween.isDone()) {
            this.gameObject.SetActive(false);
        }
    }

    public void InputRecieved() {
        this.state = 2;
        this.popDownTween.reset();
        dialogueBox.HideDialogue("Welcome aboard!");
    }
    public void SetUpItem(ItemSO item) {
        this.item = item;
        Debug.Log(item.name);
        Debug.Log("asdf" + item.PrettyText());
        dialogueBox.ShowKeepItemDialogue(item.PrettyText());

        this.gameObject.SetActive(true);
    }
    public void SetUpProfile(ProfileSO profile) {
        this.profile = profile;
        dialogueBox.ShowKeepCrewDialogue(profile.PrettyText());

        this.gameObject.SetActive(true);
    }
}
