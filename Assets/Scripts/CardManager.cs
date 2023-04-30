﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

	public string role;
	public int tier;
	public string[] tiers = new string[6];
	public int exp = 0;
	public int maxExp;

	public bool isSelectedMain = false;
	public bool isSelectedSub = false;
	public bool isSelected = false;

	string path;
	// Use this for initialization
	void Start () {
		maxExp = (int)(Mathf.Pow(2, tier - 1) * 1000f);
		GetImg();

    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			GetImg();
		}
	}

	public void CardClick() {
		if (GameObject.Find ("Content").GetComponent<GameManager> ().isWaitingSelect) {
			GameObject.Find ("Content").GetComponent<GameManager> ().currentSelected = this.gameObject.transform.Find("Alpha").gameObject;
			GameObject.Find ("Content").GetComponent<GameManager> ().justClicked = true;
		}

	}
	public void GetImg() {
		transform.Find ("JobName").GetComponent<Text> ().text = role;

		path = "JobIllustKR/" + role;
		transform.Find ("JobIllust").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "JobThumb/" + role;
		transform.Find ("TierFrame").transform.Find("JobThumb").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "UI/" + "frame" + (tier + "");
		transform.Find ("CardFrame").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "UI/" + "thumb" + (tier + "");
		transform.Find ("TierFrame").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "Duel3/" + tiers [2];
		transform.Find ("Tier3").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "Duel4+/" + tiers [3];
		transform.Find ("Tier4").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "Duel4+/" + tiers [4];
		transform.Find ("Tier5").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		path = "Duel4+/" + tiers [5];
		transform.Find ("Tier6").GetComponent<Image> ().sprite = Resources.Load<Sprite> (path);

		if (exp > maxExp) {
			exp = maxExp;
		}
		transform.Find ("ExpBar").GetComponent<RectTransform> ().sizeDelta = new Vector2((310 * ((float)exp / (float)maxExp)), transform.Find ("ExpBar").GetComponent<RectTransform> ().sizeDelta.y);
		if (exp >= maxExp) {
			transform.Find ("ExpBar").GetComponent<Image> ().color = new Color (0, 0.8f, 1.0f);
		}
		else {
			transform.Find ("ExpBar").GetComponent<Image> ().color = new Color (1.0f, 0.7f, 0);
		}
	}
}