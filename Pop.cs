using UnityEngine;
using System.Collections;

public class Pop : MonoBehaviour {

	public int score = 0;
	public int coins;

	bool isBlue = true;
	public GameObject[] balloons = new GameObject[6];
	public GameObject[] colorEffects = new GameObject[2];

	public GameObject[] popNoise = new GameObject[20];

	int popIndex = 0;
	public bool disabled;

	int totalpopped = 0;

	public Camera tCam;

	private string GC = "n";

	GameObject[] bluePops = new GameObject[50];
	GameObject[] redPops = new GameObject[20];

	int blueIndex = 0;
	int redIndex = 0;


	// Use this for initialization
	void Awake () {
		Input.multiTouchEnabled = true;
		CameraFade.StartAlphaFade(Color.white, true, 0.3f, 0.0f);

		for(int i = 0; i < bluePops.Length; i++) {
			bluePops[i] = Instantiate(colorEffects[0], new Vector3(-50, 0, 0), Quaternion.identity) as GameObject;
		}

		for(int k = 0; k < redPops.Length; k++) {
			redPops[k] = Instantiate(colorEffects[1], new Vector3(-45, 0, 0), Quaternion.identity) as GameObject;
		}

		coins = PlayerPrefs.GetInt("coins");

		totalpopped = PlayerPrefs.GetInt("totalpopped");

		AdjustScreen();

		GC = PlayerPrefs.GetString("GC_Enabled");
	}

	void spawnPop(bool blue, Vector3 coord) {
		if(blue) {
			if(blueIndex == 48) blueIndex = 0;
			GameObject spawned = bluePops[blueIndex];
			spawned.SetActive(true);
			#if UNITY_EDITOR
				Debug.DrawLine(spawned.transform.position, coord, Color.cyan, 0.5f);
			#endif
			spawned.transform.position = coord;
			blueIndex++;
		}
		else {
			if(redIndex == 18) redIndex = 0;
			GameObject spawned = redPops[redIndex];
			spawned.SetActive(true);
			#if UNITY_EDITOR
				Debug.DrawLine(spawned.transform.position, coord, Color.red, 0.5f);
			#endif
			spawned.transform.position = coord;
			redIndex++;
		}
	}

	void AdjustScreen() {
		
		int screenH = Screen.height;
		int screenW = Screen.width;
		
		float aRatio = (float)screenH/(float)screenW;
		Debug.Log(aRatio.ToString());
		
		if(aRatio > 1.4 && aRatio <= 1.6) {   //iPhone 4S and below
			
			GetComponent<Camera>().fieldOfView = 53;
			tCam.fieldOfView = 53;
		}
		else if(aRatio > 1.6 && aRatio <= 1.8) {	//iPhone 5 to iPhone 6+
			
			GetComponent<Camera>().fieldOfView = 60;
		}
		else if(aRatio > 1.2 && aRatio <= 1.4) {	//All iPAD
			
			GetComponent<Camera>().fieldOfView = 48;
			tCam.fieldOfView = 48;
		}
			
	}
	
	void Update() {
		if(disabled) return;

		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_WP8 || UNITY_ANDROID)
		//MULTI TOUCH
		int touchCount = Input.touchCount;

		for(int t = 0; t < touchCount; t++) {
			if( (Input.GetTouch(t).phase == TouchPhase.Began) && t != 0) {

				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(t).position);
				if (Physics.Raycast(ray, out hit))
					Debug.DrawLine(ray.origin, hit.point);
				
				if(hit.collider == null) return;
				GameObject popped = hit.collider.gameObject;
				
				if(popped.layer == 8) {
					PopBalloon(popped);
				}

			}	   
		}
		#endif

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				Debug.DrawLine(ray.origin, hit.point);

				if(hit.collider == null) return;
				GameObject popped = hit.collider.gameObject;

				if(popped.layer == 8) {
					PopBalloon(popped);
				}
		}

	}

	public void PopBalloon(GameObject target) {
		//popEffect = colorEffects[0];
		isBlue = true;
		switch(target.name[0]) {
			
		case 'R':
			Red(target);
			break;
		case 'G':
			Green(target);
			break;
		case 'B':
			Blue(target);
			break;
		default:
			Red(target);
			break;
		}
	}

	public void PopBalloonRed(GameObject target) {
		isBlue = false;
		//popEffect = colorEffects[1];
		switch(target.name[0]) {
			
		case 'R':
			Red(target);
			break;
		case 'G':
			Green(target);
			break;
		case 'B':
			Blue(target);
			break;
		default:
			Red(target);
			break;
		}
	}

	public void Red(GameObject target) {
		target.SetActive(false);
		spawnPop(isBlue, target.transform.position);
		AddScore(1);
		
	}
	
	public void Green(GameObject target) {
		GameObject one = Instantiate(balloons[0], new Vector3(target.transform.position.x+0.2f, target.transform.position.y, target.transform.position.z), Quaternion.identity) as GameObject;
		GameObject two = Instantiate(balloons[0], new Vector3(target.transform.position.x-0.2f, target.transform.position.y, target.transform.position.z), Quaternion.identity) as GameObject;
		
		Physics.IgnoreCollision(one.GetComponent<Collider>(), two.GetComponent<Collider>());
		
		target.SetActive(false);
		spawnPop(isBlue, target.transform.position);
		
		one.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50,100), Random.Range(-20, 100), 20));
		two.GetComponent<Rigidbody>().AddForce(new Vector3(-Random.Range(50,100), Random.Range(-70,100), 20));
		
		AddScore(1);
	}

	public void Blue(GameObject target) {
		GameObject one = Instantiate(balloons[1], new Vector3(target.transform.position.x+0.2f, target.transform.position.y, target.transform.position.z), Quaternion.identity) as GameObject;
		GameObject two = Instantiate(balloons[1], new Vector3(target.transform.position.x-0.2f, target.transform.position.y, target.transform.position.z), Quaternion.identity) as GameObject;
		
		Physics.IgnoreCollision(one.GetComponent<Collider>(), two.GetComponent<Collider>());
		
		target.SetActive(false);
		spawnPop(isBlue, target.transform.position);
		
		one.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50,100), Random.Range(-20, 100), 20));
		two.GetComponent<Rigidbody>().AddForce(new Vector3(-Random.Range(50,100), Random.Range(-70,100), 20));
		
		AddScore(1);
	}
	
	public void AddScore(int amount) {
		if(disabled) return;

		PopSound();

		score += amount;
		coins += amount;
		totalpopped += amount;
		PlayerPrefs.SetInt("totalpopped", totalpopped);
		PlayerPrefs.SetInt("coins", coins);
		// Animation

		if(GC == "y") {

			if(score == 1) {

				if(!PlayerPrefs.HasKey("pop1")){                                 //make sure not already gotten
					Social.ReportProgress("1", 100.0, result => {
						if (result)
							PlayerPrefs.SetString("pop1", "done");                    //store it as done
						else
							Debug.Log ("Failed to report achievement");
					});
				}
			}
			else if(totalpopped == 100) {

				if(!PlayerPrefs.HasKey("pop100")){                                 //make sure not already gotten
					Social.ReportProgress("100", 100.0, result => {
						if (result)
							PlayerPrefs.SetString("pop100", "done");                    //store it as done
						else
							Debug.Log ("Failed to report achievement");
					});
				}
			}
			else if(totalpopped == 1000) {

				if(!PlayerPrefs.HasKey("pop1000")){                                 //make sure not already gotten
					Social.ReportProgress("1000", 100.0, result => {
						if (result)
							PlayerPrefs.SetString("pop1000", "done");                    //store it as done
						else
							Debug.Log ("Failed to report achievement");
					});
				}
			}
			else if(totalpopped == 5000) {

				if(!PlayerPrefs.HasKey("pop5000")){                                 //make sure not already gotten
					Social.ReportProgress("5000", 100.0, result => {
						if (result)
							PlayerPrefs.SetString("pop5000", "done");                    //store it as done
						else
							Debug.Log ("Failed to report achievement");
					});
				}
			}
		}

	}

	public void disableActions() {
		disabled = true;
	}

	void PopSound() {
		if(popIndex == 19) popIndex = 0;
		popIndex++;
		popNoise[popIndex].GetComponent<AudioSource>().Play();
	}
	
}
