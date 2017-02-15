using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public float emissionRate = 6f;
	public GameObject[] clusters;

	int[] rightBuild = new int[5];
	int[] leftBuild = new int[5];
	int[,] fullBuild = new int[2, 5];

	public int handicap = 0;

	public GameObject instructions;
	public bool over = false;

	int built;

	// Use this for initialization
	void Start () {
		StartCoroutine(Spawn());

		if(PlayerPrefsX.GetBool("nohelp") == false) {
			StartCoroutine(Instructions());
			PlayerPrefsX.SetBool("nohelp", true);
		}

		//
		// BUILT COUNTER
		rightBuild = PlayerPrefsX.GetIntArray("rightspots");
		leftBuild = PlayerPrefsX.GetIntArray("leftspots");
		
		if(rightBuild.Length < 4) rightBuild = new int[5];
		if(leftBuild.Length < 4) leftBuild = new int[5];
		
		//initialize full build
		for (int i = 0; i < 2; i++) {
			
			for (int j = 0; j < 5; j++) {
				
				if (i == 0)
					fullBuild[i,j] = rightBuild[j];
				else
					fullBuild[i,j] = leftBuild[j];
				
				if (fullBuild [i,j] != 0) {
					built++;
					
				}
			}
		}
	}

	IEnumerator Instructions() {

		yield return new WaitForSeconds(0.5f);
		instructions.SetActive(true);
		iTween.ScaleFrom(instructions, new Vector3(0,0,0), 0.5f);
		yield return new WaitForSeconds(0.6f);
		yield return new WaitForSeconds(2.8f);
		iTween.ScaleTo(instructions, new Vector3(0,0,0), 0.6f);
		yield return new WaitForSeconds(0.6f);
		instructions.SetActive(false);
	}
	
	IEnumerator Spawn() {
		Instantiate(clusters[Random.Range(0, clusters.Length)], transform.position, Quaternion.identity);
		emissionRate = Mathf.Clamp(5f - (built*0.2f) - Time.timeSinceLevelLoad/30, 0.6f, 10f);
		if(!over) {
			yield return new WaitForSeconds(Random.Range (0.6f, emissionRate));

			handicap = (int)(Mathf.Clamp(Time.timeSinceLevelLoad - 40, 0, 10000)/20);
			for(int f = 0; f < handicap; f++) {
				if(Random.Range(0, 1.0f) < 0.2f) Instantiate(clusters[Random.Range(0, clusters.Length)], transform.position, Quaternion.identity);
			}
		}
		else yield return new WaitForSeconds(3);

		StartCoroutine(Spawn());
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, new Vector3(6, 1, 1));
	}



}
