using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : BaseInputModule {

	public List<GameObject> cards = new List<GameObject> ();
	public int cardIdx = -1; // when I generate first card, it will be idx 0 (cardIdx += 1, line 233)
	public GameObject card;
	public GameObject currentSelected;
	public GameObject mainReinforce;

	public bool justClicked = false;

	public int luna = 0;
	public int rubble = 0;

	int cardCnt = 0;
	int maxCardCnt = 100;

	public bool no4tier = true;

	public bool isWaitingSelect = false;

	AudioSource audio;

	public float firstScroll;
	public float secondScroll;

	bool isReinforceMode = false;
	[SerializeField]
	int selectedCardCnt = 0;
	public int tempExp = 0;

	bool getCollection1 = false;
	bool getCollection2 = false;

	int cardSlotExpandCost = 100;


	// update role
	int supportCnt = 8;
	int specialCnt = 19;

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
		{ "비밀결사",31 },
		{ "시민", 32},
		{ "교주",33 }
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
		{ "비밀결사","정의" },
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
		{ "비밀결사","밀사" },
		{ "시민","무능력" },
		{ "교주","포교" }
	};
		
	Dictionary <string, int> rolePercent = new Dictionary<string, int> () {
		/*
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
		{ "교주",10000 }*/
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
		new List<string> { "비밀결사", "열정", "냉정", "달변", "숙련", "탐욕", "고무", "쇼맨십", "보험" },
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
		new List<string> {"비밀결사", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "유품", "정보원", "검시", "표식"},
		new List<string> {"교주", "지박령", "도주", "배심원", "결백", "확성기", "독심술", "유언", "위증", "시한부", "암구호", "설파"},
	};
	// Use this for initialization
	void Start () {

		int mafiaPercent = 10000 / 12 * 3;
		int supportPercent = (10000 / 12) / supportCnt;
		int policePercent = (10000 / 12) / 2;
		int doctorPercent = 10000 / 12;
		int specialPercent = ((10000 / 12) * 5) / specialCnt;

		print (mafiaPercent);
		print (supportPercent);
		print (policePercent);
		print (doctorPercent);
		print (specialPercent);

		audio = this.GetComponent<AudioSource> ();
		int currentPercent = 0;
		for (int i = 0; i < 1; i++) {
			currentPercent += mafiaPercent;
			rolePercent.Add (roleTo3Tier [i] [0], currentPercent);
		}
		for (int i = 1; i < 1 + supportCnt; i++) { // 1 : mafia
			currentPercent += supportPercent;
			rolePercent.Add (roleTo3Tier [i] [0], currentPercent);
		}
		for (int i = 1 + supportCnt; i < 1 + supportCnt + 2; i++) { // 2 : police, vigilante
			currentPercent += policePercent;
			rolePercent.Add (roleTo3Tier [i] [0], currentPercent);
		}
		for (int i = 1 + supportCnt + 2; i < 1 + supportCnt + 2 + 1; i++) { // 1 : doctor
			currentPercent += doctorPercent;
			rolePercent.Add (roleTo3Tier [i] [0], currentPercent);
		}
		for (int i = 1 + supportCnt + 2 + 1; i < 1 + supportCnt + 2 + 1 + specialCnt; i++) {
			currentPercent += specialCnt;
			rolePercent.Add (roleTo3Tier [i] [0], currentPercent);
		}
		rolePercent.Add ("교주", 10000);

		foreach (string Key in rolePercent.Keys) {
			print(Key);  
		}

		foreach (int Val in rolePercent.Values) {
			print(Val);  
		}
	}

	void randCard (int cardTier) {
		string _role = "";
		int _tier = cardTier;
		string[] _tiers = new string[6];

		for (int i = 0; i < 6; i++) {
			_tiers [i] = "무능력";
		}
		int roleNum = Random.Range (1, 10000 + 1);
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
				_tiers [0] = "악행";
			}
			else {
				_role = "시민";
				_tiers [0] = "정의"; 
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
		UpdateDesktop ();
		RefreshUI ();
		CollectionCheck ();
	}

	void CollectionCheck() {
		if (!getCollection1) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).GetComponent<CardManager> ().tier == 6) {
					getCollection1 = true;
					GameObject.Find ("Collection1").GetComponent<Image> ().enabled = true;
				}
			}
		}
		if (!getCollection2) {
			int mafia = 0;
			int support = 0;
			int police = 0;
			int doctor = 0;
			int special = 0;
			int cultLeader = 0;

			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).GetComponent<CardManager> ().tier == 6) {
					if (transform.GetChild (i).GetComponent<CardManager> ().role == "마피아") {
						mafia += 1;
					} else if (transform.GetChild (i).GetComponent<CardManager> ().role == "경찰") {
						police += 1;
					} else if (transform.GetChild (i).GetComponent<CardManager> ().role == "자경단원") {
						police += 1;
					} else if (transform.GetChild (i).GetComponent<CardManager> ().role == "의사") {
						doctor += 1;
					} else if (transform.GetChild (i).GetComponent<CardManager> ().role == "교주") {
						cultLeader += 1;
					}
					else {
						for (int sup = 1; sup <= 8; sup++) {
							if (roleList[transform.GetChild (i).GetComponent<CardManager> ().role] == sup) {
								support += 1;
							}
						}
						for (int spec = 13; spec <= 31; spec++) { // ####### 직업 개수가 바뀌게 된다면 수정 필요 #######
							if (roleList[transform.GetChild (i).GetComponent<CardManager> ().role] == spec) {
								special += 1;
							}
						}
					}
				}
				if ((mafia >= 3) && (police >= 1) && (doctor >= 1) && (special >= 5) && (cultLeader >= 1)) {
					getCollection2 = true;
					GameObject.Find ("Collection2").GetComponent<Image> ().enabled = true;
				}
			}
		}
	}

	void UpdateDesktop() {
		if (isWaitingSelect) {
			if (justClicked) {
				justClicked = false;
				if (currentSelected.GetComponentInParent<CardManager> ().isSelected == false) {
					if (currentSelected.GetComponentInParent<CardManager> ().tier != 6) {
						if (selectedCardCnt == 0) {
							selectedCardCnt += 1;
							mainReinforce = currentSelected.transform.parent.gameObject;
							currentSelected.GetComponentInParent<CardManager> ().isSelectedMain = true;
							currentSelected.GetComponentInParent<CardManager> ().isSelected = true;
							currentSelected.transform.parent.Find ("SelectedFrame").GetComponent<Image> ().enabled = true;
							currentSelected.GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
						}
						else {
							if (mainReinforce.GetComponent<CardManager>().exp >= mainReinforce.GetComponent<CardManager>().maxExp) {
								if (currentSelected.GetComponentInParent<CardManager> ().role == mainReinforce.GetComponent<CardManager> ().role) {
									if (currentSelected.GetComponentInParent<CardManager> ().tier == mainReinforce.GetComponent<CardManager> ().tier) {
										if (selectedCardCnt < mainReinforce.GetComponent<CardManager> ().tier + 1) {
											selectedCardCnt += 1;
											currentSelected.GetComponentInParent<CardManager> ().isSelectedSub = true;
											currentSelected.GetComponentInParent<CardManager> ().isSelected = true;
											tempExp += currentSelected.GetComponentInParent<CardManager> ().maxExp / 4;
											currentSelected.GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
										}
									}
								}
							} else {
								if (mainReinforce.GetComponent<CardManager> ().maxExp - (mainReinforce.GetComponent<CardManager> ().exp + tempExp) > 0) {
									selectedCardCnt += 1;
									currentSelected.GetComponentInParent<CardManager> ().isSelectedSub = true;
									currentSelected.GetComponentInParent<CardManager> ().isSelected = true;
									tempExp += currentSelected.GetComponentInParent<CardManager> ().maxExp / 4;
									currentSelected.GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
								}
							}

						}
					}
				}
				else {
					if ((currentSelected.GetComponentInParent<CardManager>().isSelectedMain && selectedCardCnt == 1) || (!currentSelected.GetComponentInParent<CardManager>().isSelectedMain)) {
						if (selectedCardCnt == 1) {
							mainReinforce = null;
							currentSelected.GetComponentInParent<CardManager> ().isSelectedMain = false;
							currentSelected.GetComponentInParent<CardManager> ().isSelected = false;
							currentSelected.transform.parent.Find ("SelectedFrame").GetComponent<Image> ().enabled = false;
							currentSelected.GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
							selectedCardCnt -= 1;
						}
						else {
							selectedCardCnt -= 1;
							currentSelected.GetComponentInParent<CardManager> ().isSelectedSub = false;
							currentSelected.GetComponentInParent<CardManager> ().isSelected = false;
							tempExp -= currentSelected.GetComponentInParent<CardManager> ().maxExp / 4;
							currentSelected.GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
						}
					}
				}
			}
		}
	}

	public override void Process() {}
	protected override void OnEnable() {}
	protected override void OnDisable() {}

	public void CardSlotExpand() {
		luna += cardSlotExpandCost;
		cardSlotExpandCost += 100;
		maxCardCnt += 10;
	}

	public void BuyLowCard() {
		if (cardCnt + 1 <= maxCardCnt) {
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
		}
    }
	public void BuyLowCardpack() {
		if (cardCnt + 10 <= maxCardCnt) {
			for (int i = 1; i <= 10; i++) {
				BuyLowCard();
			}
		}
    }
	public void BuyHighCard() {
		if (cardCnt + 1 <= maxCardCnt) {
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
		}
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
		if (cardCnt + 10 <= maxCardCnt) {
			luna += 2000;
			for (int i = 1; i <= 9; i++) {
				BuyHighCardpack_Each();
			}
			if (no4tier) {
				int tempTier = Random.Range (1, 1000 + 1);
				if (tempTier <= 10) {
					randCard (5);
				} else {
					randCard (4);
				}
			} else {
				BuyHighCardpack_Each();
			}
			no4tier = true;
		}

    }
	void RefreshUI() {
        GameObject.Find("LunaText").GetComponent<Text>().text = luna + "";
        GameObject.Find("RubbleText").GetComponent<Text>().text = rubble + "";
		cardCnt = GameObject.Find ("Content").transform.childCount;
		GameObject.Find ("CardCnt").GetComponent<Text> ().text = cardCnt + "/" + maxCardCnt;
    }
	public void CardSort() {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).SetSiblingIndex(0);
		}
		for (int j = 0; j < 32; j++) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild(i).GetComponent<CardManager>().exp == j * 1000) {
					transform.GetChild(i).SetSiblingIndex(0);
				}
			}
		}
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
		if (!isReinforceMode) {
			isReinforceMode = true;
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
				GameObject.Find ("SortButton").GetComponent<Button> ().enabled = false;
				GameObject.Find ("ReinforceCancelButton").GetComponent<Image> ().enabled = true;
			}
		}
		else {
			if (mainReinforce.GetComponent<CardManager>().exp >= mainReinforce.GetComponent<CardManager>().maxExp) {
				int subCnt = 0;
				for (int i = 0; i < transform.childCount; i++) {
					if (transform.GetChild(i).GetComponent<CardManager>().isSelectedSub == true) {
						subCnt += 1;
					}
				}
				if (subCnt == mainReinforce.GetComponent<CardManager>().tier)  {
					mainReinforce.GetComponent<CardManager> ().exp = 0;
					int[] cost = new int[6] {
						0, 10000, 50000, 100000, 500000, 1000000
					};
					rubble += cost [mainReinforce.GetComponent<CardManager> ().tier];
					selectedCardCnt = 0;
					tempExp = 0;
					mainReinforce.GetComponent<CardManager> ().tier += 1;
					mainReinforce.GetComponent<CardManager> ().maxExp *= 2;
					if (mainReinforce.GetComponent<CardManager> ().tier >= 4) {
						for (int i = 0; i < roleTo4Tier.Count; i++) {
							if (roleTo4Tier [i] [0] == mainReinforce.GetComponent<CardManager> ().role) {
								bool duplCheckOK = false; // true = no duplication tier (ex. same tier in t4, t5)
								int t4rnd = Random.Range (1, roleTo4Tier [i].Count);
								while (duplCheckOK == false) {
									duplCheckOK = true;
									foreach (string tierName in mainReinforce.GetComponent<CardManager>().tiers) {
										if (tierName == roleTo4Tier [i] [t4rnd]) {
											duplCheckOK = false;
											t4rnd = Random.Range (1, roleTo4Tier [i].Count);
										}
									}
								}
								mainReinforce.GetComponent<CardManager> ().tiers [mainReinforce.GetComponent<CardManager> ().tier - 1] = roleTo4Tier [i] [t4rnd];
							}
						}
					}
					else {
						if (mainReinforce.GetComponent<CardManager> ().tier == 2) {
							if (mainReinforce.GetComponent<CardManager> ().role == "시민") {
								int tempRole = Random.Range (10, 30 + 1);
								foreach (KeyValuePair<string, int> current in roleList) { // Gen Role
									if (tempRole == current.Value) {
										mainReinforce.GetComponent<CardManager> ().role = current.Key;
										mainReinforce.GetComponent<CardManager> ().tiers [1] = roleTo2Tier [current.Key];
										break;
									}
								}
							}
							else if (mainReinforce.GetComponent<CardManager> ().role == "악인") {
								int tempRole = Random.Range (0, 30 + 1);
								foreach (KeyValuePair<string, int> current in roleList) { // Gen Role
									if (tempRole == current.Value) {
										mainReinforce.GetComponent<CardManager> ().role = current.Key;
										mainReinforce.GetComponent<CardManager> ().tiers [1] = roleTo2Tier [current.Key];
										break;
									}
								}
							}
						}
						else if (mainReinforce.GetComponent<CardManager> ().tier == 3) {
							for (int i = 0; i < roleTo3Tier.Count; i++) {
								if (roleTo3Tier [i] [0] == mainReinforce.GetComponent<CardManager> ().role) {
									int t3rnd = Random.Range (1, roleTo3Tier [i].Count);
									mainReinforce.GetComponent<CardManager> ().tiers [2] = roleTo3Tier [i] [t3rnd];
								}
							}
						}
					}
					audio.PlayOneShot (Resources.Load<AudioClip> ("SoundFX/tier_up"));
					for (int i = 0; i < transform.childCount; i++) {
						if (transform.GetChild(i).GetComponent<CardManager>().isSelectedSub == true) {
							Destroy (transform.GetChild (i).gameObject);
						}
					}
					if (mainReinforce != null) {
						mainReinforce.GetComponent<CardManager> ().isSelectedMain = false;
						mainReinforce.GetComponent<CardManager> ().isSelected = false;
						mainReinforce.transform.Find ("Alpha").GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
						mainReinforce.transform.Find ("SelectedFrame").GetComponent<Image> ().enabled = false;
					}
					mainReinforce.GetComponent<CardManager> ().GetImg ();
					mainReinforce = null;
				}

			}
			else {
				Mathf.Clamp (tempExp, 0, mainReinforce.GetComponent<CardManager> ().maxExp);
				if (tempExp > 0) {
					mainReinforce.GetComponent<CardManager> ().exp += tempExp;
					rubble += selectedCardCnt * 1000;
					selectedCardCnt = 0;
					tempExp = 0;
					audio.PlayOneShot (Resources.Load<AudioClip> ("SoundFX/experience_up"));
					for (int i = 0; i < transform.childCount; i++) {
						if (transform.GetChild(i).GetComponent<CardManager>().isSelectedSub == true) {
							Destroy (transform.GetChild (i).gameObject);
						}
					}
					if (mainReinforce != null) {
						mainReinforce.GetComponent<CardManager> ().isSelectedMain = false;
						mainReinforce.GetComponent<CardManager> ().isSelected = false;
						mainReinforce.transform.Find ("Alpha").GetComponent<Image> ().color = new Color (0, 0, 0, 0.5f);
						mainReinforce.transform.Find ("SelectedFrame").GetComponent<Image> ().enabled = false;
					}

					mainReinforce.GetComponent<CardManager> ().GetImg ();
					mainReinforce = null;
				}

			}
		}
	}
	public void OnReinforceCancel() {
		isWaitingSelect = false;
		isReinforceMode = false;
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<CardManager>().isSelected) {
				transform.GetChild (i).GetComponent<CardManager> ().isSelected = false;
			}
			if (transform.GetChild(i).GetComponent<CardManager>().isSelectedMain) {
				transform.GetChild (i).GetComponent<CardManager> ().isSelectedMain = false;
			}
			if (transform.GetChild(i).GetComponent<CardManager>().isSelectedSub) {
				transform.GetChild (i).GetComponent<CardManager> ().isSelectedSub = false;
			}
		}
		if (mainReinforce != null) {
			mainReinforce.transform.Find ("SelectedFrame").GetComponent<Image> ().enabled = false;
		}
		mainReinforce = null;
		selectedCardCnt = 0;
		tempExp = 0;
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