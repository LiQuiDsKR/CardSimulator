using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : BaseInputModule {

	public List<GameObject> cards = new List<GameObject> ();
	public int cardIdx = -1; // when I generate first card, it will be idx 0 (cardIdx += 1, line 233)
	public GameObject card;
	public GameObject mainReinforce;

	public int luna = 0;
	public int rubble = 0;

	public bool no4tier = true;

	public bool isWaitingSelect = false;

	Canvas canv;
	GraphicRaycaster gr;
	PointerEventData ped;
	List<RaycastResult> raycastResults;

	RaycastHit hit;

	Dictionary <string, int> roleList = new Dictionary<string, int> () {
		{ "마피아", 0 },
		{ "스파이", 1 },
		{ "짐승인간", 2 },
		{ "마담", 3 },
		{ "도둑",4 },
		{ "마녀",5 },
		{ "과학자",6 },
		{ "사기꾼",7 },
		{ "청부업자",8 },
		{ "악인",9 },
		{ "경찰",10 },
		{ "자경단원",11 },
		{ "의사",12 },
		{ "군인",13 },
		{ "정치인",14 },
		{ "영매",15 },
		{ "연인",16 },
		{ "건달",17 },
		{ "기자",18 },
		{ "사립탐정",19 },
		{ "도굴꾼",20 },
		{ "테러리스트",21 },
		{ "성직자",22 },
		{ "예언자",23 },
		{ "판사",24 },
		{ "간호사",25 },
		{ "마술사",26 },
		{ "해커",27 },
		{ "심리학자",28 },
		{ "용병",29 },
		{ "공무원",30 },
		{ "시민", 31},
		{ "교주",32 }
	};

	Dictionary <string, string> roleTo1Tier = new Dictionary<string, string> () {
		{ "마피아","악행" },
		{ "스파이","악행" },
		{ "짐승인간","악행" },
		{ "마담","악행" },
		{ "도둑","악행" },
		{ "마녀","악행" },
		{ "과학자","악행" },
		{ "사기꾼","악행" },
		{ "청부업자","악행" },
		{ "악인","악행" },
		{ "경찰","정의" },
		{ "자경단원","정의" },
		{ "의사","정의" },
		{ "군인","정의" },
		{ "정치인","정의" },
		{ "영매","정의" },
		{ "연인","정의" },
		{ "건달","정의" },
		{ "기자","정의" },
		{ "사립탐정","정의" },
		{ "도굴꾼","정의" },
		{ "테러리스트","정의" },
		{ "성직자","정의" },
		{ "예언자","정의" },
		{ "판사","정의" },
		{ "간호사","정의" },
		{ "마술사","정의" },
		{ "해커","정의" },
		{ "심리학자","정의" },
		{ "용병","정의" },
		{ "공무원","정의" },
		{ "시민","정의" },
		{ "교주","악행" }
	};

	Dictionary <string, string> roleTo2Tier = new Dictionary<string, string> () {
		{ "마피아","처형" },
		{ "스파이","첩보" },
		{ "짐승인간","갈망" },
		{ "마담","유혹" },
		{ "도둑","도벽" },
		{ "마녀","저주" },
		{ "과학자","재생" },
		{ "사기꾼","사기" },
		{ "청부업자","청부" },
		{ "악인","무능력" },
		{ "경찰","수색" },
		{ "자경단원","숙청" },
		{ "의사","치료" },
		{ "군인","방탄" },
		{ "정치인","처세" },
		{ "영매","성불" },
		{ "연인","연애" },
		{ "건달","공갈" },
		{ "기자","특종" },
		{ "사립탐정","추리" },
		{ "도굴꾼","도굴" },
		{ "테러리스트","자폭" },
		{ "성직자","소생" },
		{ "예언자","계시" },
		{ "판사","선고" },
		{ "간호사","처방" },
		{ "마술사","트릭" },
		{ "해커","해킹" },
		{ "심리학자","관찰" },
		{ "용병","의뢰" },
		{ "공무원","조회" },
		{ "시민","무능력" },
		{ "교주","포교" }
	};

	Dictionary <string, int> rolePercent = new Dictionary<string, int> () {
		{ "마피아", 2500 },
		{ "스파이", 2604 },
		{ "짐승인간", 2708 },
		{ "마담", 2812 },
		{ "도둑",2916 },
		{ "마녀",3020 },
		{ "과학자",3124 },
		{ "사기꾼",3228 },
		{ "청부업자",3332 },
		{ "경찰",3749 },
		{ "자경단원",4166 },
		{ "의사",4999 },
		{ "군인",5230 },
		{ "정치인",5461 },
		{ "영매",5692 },
		{ "연인",5923 },
		{ "건달",6154 },
		{ "기자",6385 },
		{ "사립탐정",6616 },
		{ "도굴꾼",6847 },
		{ "테러리스트",7078 },
		{ "성직자",7309 },
		{ "예언자",7540 },
		{ "판사",7771 },
		{ "간호사",8002 },
		{ "마술사",8233 },
		{ "해커",8464 },
		{ "심리학자",8695 },
		{ "용병",8926 },
		{ "공무원",9157 },
		{ "교주",10000 }
	};

	List <List<string>> roleTo3Tier = new List<List<string>> () {
		new List<string> { "마피아", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "광기" },
		new List<string> { "스파이", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "짐승인간", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "광기" },
		new List<string> { "마담", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "도둑", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "마녀", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "과학자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "사기꾼", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "청부업자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "경찰", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "자경단원", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "의사", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "군인", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "정치인", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "영매", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "연인", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "건달", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "기자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "사립탐정", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "도굴꾼", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "테러리스트", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "성직자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "예언자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "판사", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "간호사", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "마술사", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "해커", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "심리학자", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "용병", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "공무원", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
		new List<string> { "교주", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" }
	};

	List <List<string>> roleTo4Tier = new List<List<string>>() {
		new List<string> {"마피아", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "수습", "야습", "승부수", "퇴마", "위선", "은폐", "저격", "수배", "무법자"},
		new List<string> {"스파이", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "미인계", "부검", "자객"},
		new List<string> {"짐승인간", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "포효", "야만성"},
		new List<string> {"마담", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "현혹", "데뷔"},
		new List<string> {"도둑", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "후계자"},
		new List<string> {"마녀", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "망각술"},
		new List<string> {"과학자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "왜곡", "최면"},
		new List<string> {"사기꾼", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "지령", "시한부", "암구호", "밀정", "미인계"},
		new List<string> {"청부업자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "시한부", "암구호", "밀정", "직감"},
		new List<string> {"경찰", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "부검", "기밀", "영장", "도청"},
		new List<string> {"자경단원", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "결사"},
		new List<string> {"의사", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "검진", "진정", "박애"},
		new List<string> {"군인", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "불굴", "정신력"},
		new List<string> {"정치인", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "독재", "불문율"},
		new List<string> {"영매", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "강령"},
		new List<string> {"연인", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "헌신", "원한"},
		new List<string> {"건달", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "갈취", "길동무"},
		new List<string> {"기자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "속보", "부고"},
		new List<string> {"사립탐정", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "함정"},
		new List<string> {"도굴꾼", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "계승", "망령"},
		new List<string> {"테러리스트", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "유폭", "섬광"},
		new List<string> {"성직자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "희생", "구마"},
		new List<string> {"예언자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "선각자"},
		new List<string> {"판사", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "관권", "불문율"},
		new List<string> {"간호사", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "검시"},
		new List<string> {"마술사", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "조수", "투시"},
		new List<string> {"해커", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "동기화"},
		new List<string> {"심리학자", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "프로파일링"},
		new List<string> {"용병", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "추적", "결사"},
		new List<string> {"공무원", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "색출"},
		new List<string> {"교주", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "시한부", "암구호", "설파"},
	};
	// Use this for initialization
	void Start () {
		canv = GameObject.Find("Canvas").GetComponent<Canvas>();
		gr = canv.GetComponent<GraphicRaycaster> ();
		ped = new PointerEventData (null);
		raycastResults = new List<RaycastResult>();
	}

	void randCard (int cardTier) {
		string _role = "";
		int _tier = cardTier;
		string[] _tiers = new string[6];

		for (int i = 0; i < 6; i++) {
			_tiers [i] = "무능력";
		}
		int roleNum = Random.Range (1, 10000 + 1);
		print (roleNum); // asfdasdf
		if (_tier != 1) {
			foreach (KeyValuePair<string, int> current in rolePercent) { // Gen Role
				if (roleNum <= current.Value) {
					_role = current.Key;
					break;
				}
			}
			_tiers [0] = roleTo1Tier [_role];
			if (_tier >= 2) { // Gen Tier2
				_tiers [1] = roleTo2Tier [_role];
			}
			if (_tier >= 3) { // Gen Tier3
				for (int i = 0; i < roleTo3Tier.Count; i++) {
					if (roleTo3Tier [i] [0] == _role) {
						int t3rnd = Random.Range (1, roleTo3Tier [i].Count);
						print (t3rnd + ", " + _role); //wadfasdfasdf
						_tiers [2] = roleTo3Tier [i] [t3rnd];
					}
				}
			}
			if (_tier >= 4) {
				for (int t = 3; t < 6; t++) { // Gen Tier4~6
					if (t + 1 <= _tier) {
						for (int i = 0; i < roleTo4Tier.Count; i++) {
							if (roleTo4Tier [i] [0] == _role) {
								bool duplCheckOK = false; // true = no duplication tier (ex. same tier in t4, t5)
								int t4rnd = Random.Range (1, roleTo4Tier [i].Count);
								while (duplCheckOK == false) {
									duplCheckOK = true;
									foreach (string tierName in _tiers) {
										if (tierName == roleTo4Tier [i] [t4rnd]) {
											duplCheckOK = false;
											t4rnd = Random.Range (1, roleTo4Tier [i].Count);
										}
									}
								}
								print (t4rnd + ", " + _role);
								_tiers [t] = roleTo4Tier [i] [t4rnd];
							}
						}
					}

				}
			}
		}
		else {
			if (roleNum <= 4175) {
				_role = "악인";
			}
			else {
				_role = "시민";
			}
		}
		GameObject newcard = Instantiate (card, this.transform);
		newcard.GetComponent<CardManager> ().role = _role;
		newcard.GetComponent<CardManager> ().tier = _tier;
		newcard.GetComponent<CardManager> ().tiers[0] = _tiers[0];
		newcard.GetComponent<CardManager> ().tiers[1] = _tiers[1];
		newcard.GetComponent<CardManager> ().tiers[2] = _tiers[2];
		newcard.GetComponent<CardManager> ().tiers[3] = _tiers[3];
		newcard.GetComponent<CardManager> ().tiers[4] = _tiers[4];
		newcard.GetComponent<CardManager> ().tiers[5] = _tiers[5];

	}
	// Update is called once per frame
	void Update () {
		if (isWaitingSelect) {
			if (Input.GetMouseButtonUp (0)) {
				ped.position = Input.mousePosition;
				gr.Raycast (ped, raycastResults);

				if (raycastResults.Count > 0) {
					for (int i = 0; i < raycastResults.Count; i++) {
						print (raycastResults [i].gameObject.name);
					}
					foreach (RaycastResult raycastResult in raycastResults) {
						HandlePointerExitAndEnter(ped, raycastResult.gameObject);

						if (raycastResult.gameObject.tag == "CardCollider") {
							raycastResult.gameObject.GetComponentInParent<CardManager> ().isSelectedMain = true;
							raycastResult.gameObject.GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
							mainReinforce = raycastResult.gameObject.transform.parent.gameObject;
						}
					}
				}
			}
			raycastResults.Clear ();
		}
	}

	public override void Process() {}
	protected override void OnEnable() {}
	protected override void OnDisable() {}
	public void BuyLowCard() {
		rubble += 10000;
		int tempTier = Random.Range (1, 1000 + 1);
		if (tempTier <= 10) {
			randCard (3);
		}
		else if (tempTier <= 100){
			randCard (2);
		}
		else {
			randCard (1);
        }
        RefreshUI();

    }
	public void BuyLowCardpack() {
		for (int i = 1; i <= 10; i++) {
			BuyLowCard();
        }
        RefreshUI();

    }
	public void BuyHighCard() {
		luna += 220;
		int tempTier = Random.Range (1, 1000 + 1);
		if (tempTier <= 10) {
			randCard (5);
		}
		else if (tempTier <= 100){
			randCard (4);
		}
		else {
			randCard (3);
        }
        RefreshUI();
    }
	public void BuyHighCardpack_Each() {
		int tempTier = Random.Range (1, 1000 + 1);
		if (tempTier <= 10) {
			randCard (5);
			no4tier = false;
		}
		else if (tempTier <= 100){
			randCard (4);
			no4tier = false;
		}
		else {
			randCard (3);
		}
	}
	public void BuyHighCardpack() {
		luna += 2000;
		for (int i = 1; i <= 9; i++) {
			BuyHighCardpack_Each();
		}
		if (no4tier) {
			int tempTier = Random.Range (1, 1000 + 1);
			if (tempTier <= 10) {
				randCard (5);
			}
			else {
				randCard (4);
			}
		}
		no4tier = true;
		RefreshUI();

    }
	void RefreshUI() {
        GameObject.Find("LunaText").GetComponent<Text>().text = luna + "";
        GameObject.Find("RubbleText").GetComponent<Text>().text = rubble + "";
    }
	public void CardSort() {
        // transform.SetSiblingIndex(0);
		for (int j = 0; j < roleList.Count; j++) {
            for (int i = 0; i < transform.childCount; i++) {
                if (roleList[transform.GetChild(i).GetComponent<CardManager>().role] == j) {
                    transform.GetChild(i).SetSiblingIndex(0);
                }
            }
        }
		for (int j = 1; j <= 6; j++) {
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).GetComponent<CardManager>().tier == j) {
                    transform.GetChild(i).SetSiblingIndex(0);
                }
            }
        }
        
    }

	public void OnReinforce() {
		if (!isWaitingSelect) {
			isWaitingSelect = true;
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).Find ("Alpha").GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
			}
			GameObject.Find ("DarkEffect").GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
			GameObject.Find ("BuyLowCard").GetComponent<Button> ().enabled = false;
			GameObject.Find ("BuyLowCardpack").GetComponent<Button> ().enabled = false;
			GameObject.Find ("BuyHighCard").GetComponent<Button> ().enabled = false;
			GameObject.Find ("BuyHighCardpack").GetComponent<Button> ().enabled = false;
			GameObject.Find ("ReinforceButton").GetComponent<Button> ().enabled = false;
			GameObject.Find ("SortButton").GetComponent<Button> ().enabled = false;
			GameObject.Find ("ReinforceCancelButton").GetComponent<Image> ().enabled = true;
		}
	}
	public void OnReinforceCancel() {
		isWaitingSelect = false;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).Find ("Alpha").GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
		}
		GameObject.Find ("DarkEffect").GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
		GameObject.Find ("BuyLowCard").GetComponent<Button> ().enabled = true;
		GameObject.Find ("BuyLowCardpack").GetComponent<Button> ().enabled = true;
		GameObject.Find ("BuyHighCard").GetComponent<Button> ().enabled = true;
		GameObject.Find ("BuyHighCardpack").GetComponent<Button> ().enabled = true;
		GameObject.Find ("ReinforceButton").GetComponent<Button> ().enabled = true;
		GameObject.Find ("SortButton").GetComponent<Button> ().enabled = true;
		GameObject.Find ("ReinforceCancelButton").GetComponent<Image> ().enabled = false;
	}
}