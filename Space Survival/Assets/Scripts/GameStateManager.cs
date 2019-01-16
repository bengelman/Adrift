using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
	
	public string[] messages;
	public Text textfield;
	public Text mission;
	public Text location;
	public float power = 1000;
	public float oxygen = 100;
	public bool ventsUp = true;
	public float temperature = 0f;
	public static GameStateManager instance;
	public static int gameIndex = 0;
	public static int playerZone = 3;
	public int usage = 2;
	public Text powerfield;
	public GameObject player;
	static bool playedSound = false;
	int messageIndex = 0;
	public static int alienRoom = 0;
	float delay;
	bool ventMessage = false;
	float messageLength;
	float messageTimer;
	public AudioSource source;
	public AudioClip jumpSound;
	public AudioSource backgroundSource;
	public AudioClip introAudio, suspenseAudio, chaseAudio, normalAudio;
	public RawImage jumpscareImage;
	public Camera cam;
	public static float jumpscareTimer = 0;
	public static Vector3 jumpLocation;
	public static float adrenalineTimer;
	public Texture jumpscare0, jumpscare1, jumpscare2, jumpscare3;
	public bool inVents = false;
	public static bool electrified = false;
	// Use this for initialization
	void Start () {
		instance = this;
		electrified = false;
		backgroundSource.clip = introAudio;
		backgroundSource.Play ();
		alienRoom = 0;
		jumpscareTimer = 0;
		adrenalineTimer = 0;
		playerZone = 3;
		gameIndex = 0;
		//AddMessage (new string[]{"Mission control: Mission Specialist Wrigley? Are you there? [Press Enter to continue]", "You: I'm here", "Mission control: Have you completed your preliminary assessment of the impact?", "You: We have. As you know, at approximately 1:52 GMT, the Apex II Space Station collided with an asteroid originating from deep space.", "You: After our inspection, we concluded that the primary solar power module and agricultural module have been destroyed, while the Soyuz capsule has been damaged.", "You: Our backup batteries have enough juice for a week, give or take. However, without the Soyuz, returning to Earth will be impossible.", "You: Our only viable course of action would be to repair the Soyuz capsule and return to Earth.", "Mission control: That is unfortunate. Are the solar panels beyond repair?", "You: They've been completely destroyed.", "You: But there's one other thing, actually. While performing a spacewalk, Commander Sheffield discovered an unidentified substance within the asteroid.", "Mission control: Could you describe this substance?", "You: It's a sort of black slime, with the colour and texture of crude oil but the consistency of silly putty.", "You: Sheffield is performing an analysis of it right now, so I'll let you know what we find out. Over and out.", "Commander Houston: Uh, Allen? Could you come see this?"});
		AddMessage(new string[]{
			"You: Audio log entry for March 13th, 2020. [Press enter to continue]", 
			"You: My name is Allen Wrigley. I am a mission specialist aboard the Vulcan I Space Station. I am keeping this log in the event that I do not survive this mission.", 
			"You: One month ago, there were two people on this station: Dr. Megan Schmidt, and Reid Sherr. On February 20th, Gabriel Walker and Dr. Kenneth Nichols arrived at the station in a Soyuz capsule, bearing a payload containing space debris that had collided with a satellite orbiting Earth.",
			"You: The four crew members sent a status report immediately after the Soyuz arrived... and were never heard from again.",
			"You: Though the station appeared to be structurally and mechanically intact, the four crew members had inexplicably disappeared.",
			"You: Commander Austin Sheffield, Dr. Kathy George, and myself were scheduled to arrive at the station a few weeks later, so we were tasked with investigating what exactly happened to the crew.", 
			"You: The mission went south almost immediately after our arrival. The station was struck by an asteroid, destroying the station's solar panels, and damaging our Soyuz capsule.",
			"You: Without solar panels, we have to rely on our auxillary batteries for power. They should be able to keep life support systems operational for a few hours.", 
			"You: Without the Soyuz, we cannot return to Earth. Fortunately, repairing it is within our capabilities.",
			"You: Unfortunately... it appears we are not alone on the station.", 
			"You: The previous crew of the station vanished without a trace. The station was in a state of disorder when we arrived, and it appeared that the crew had unsuccessfully attempted to eject an escape craft.",
			"You: The cause of their disappearance became immediatelly apparent when Dr. George, while in the agricultural module of the station, was attacked by this... thing.",
			"You: The thing was a shiny black glob... like crude oil, but more viscous. It engulfed Dr. George, and... tore her apart and consumed her, blood and all. It was truly horrifying.",
			"You: After a few seconds, all that remained of her were blood splatters on the wall of the station. Sheffield and I saw no other option but to jettison the agricultural module, releasing the thing into space.",
			"You: However, as we began the ejection process, we saw the thing burrow into a ventillation shaft... and from the sounds we can hear resonating from the vents, it appears that the thing got into the main body of the station.",
			"You: Commander Sheffield and I are now attempting to repair the Soyuz while avoiding the creature. Wrigley out.", 
			"Sheffield: Uh, Wrigley? Can I get some help over here?", 
			"You: Coming.",
			"Use [WASD] to move forwards and sideways",
			"Use [Q] and [E] to ascend and descend",
			"Right click to open doors and press buttons",
			"Hold left click on a door to listen"
		}, 10f);
	}
	public void AddMessage(string[] messages, float messageLength){
		this.messageLength = messageLength;
		messageTimer = messageLength;
		this.messages = messages;
		textfield.text = messages[0]; 
		textfield.enabled = true;
		messageIndex = 0;
		delay = 0.1f;
	}
	public static void Jumpscare(Vector3 location){
		if (jumpscareTimer != 0)
			return;
		
		jumpscareTimer = 2.7f;
		playedSound = false;
		jumpLocation = location;

		Vector3 direction = (jumpLocation - instance.cam.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		instance.cam.GetComponent<CameraLook>().yoffset += lookRotation.eulerAngles.x - instance.cam.transform.rotation.eulerAngles.x;
		instance.cam.GetComponent<CameraLook>().xoffset += lookRotation.eulerAngles.y - instance.cam.transform.rotation.eulerAngles.y + 90f;
		//instance.AddMessage (new string[]{"Jumpscare!"});
	}
	// Update is called once per frame
	void Update () {
		if (!ventMessage && inVents) {
			ventMessage = true;
			AddMessage (new string[]{"You: It sounded like the thing got into the ventillation.", "You: I can turn off the ventillation system from the ventillation control room."}, 10);
		}
		if (jumpscareTimer > 0) {
			jumpscareTimer -= Time.deltaTime;
			if (jumpscareTimer < 2 && jumpscareTimer >= 1) {
				if (!playedSound) {
					instance.backgroundSource.Stop ();
					playedSound = true;
					source.PlayOneShot (jumpSound, 2f);
				}
				cam.transform.rotation = Quaternion.LookRotation ((jumpLocation - cam.transform.position).normalized);
				jumpscareImage.enabled = true;

				if (jumpscareTimer <= 1.9) {

					jumpscareImage.texture = jumpscare3;
				} else if (jumpscareTimer <= 1.93) {

					jumpscareImage.texture = jumpscare2;
				} else if (jumpscareTimer <= 1.97) {

					jumpscareImage.texture = jumpscare1;
				} else if (jumpscareTimer < 2.0) {

					jumpscareImage.texture = jumpscare0;
				} 
			} else if (jumpscareTimer < 1) {
				if (gameIndex > 3) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

					SceneManager.LoadScene ("Menu");
				} else {
					jumpscareImage.enabled = false;
					Vector3 direction = (jumpLocation - cam.transform.position).normalized;

					//create the rotation we need to be in to look at the target
					Quaternion lookRotation = Quaternion.Euler(Quaternion.LookRotation(direction).eulerAngles + (90 * Vector3.up));

					//rotate us over time according to speed until we are in the required rotation
					cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, lookRotation, Time.deltaTime * 20f);
					if (jumpscareTimer <= 0) {
						jumpscareTimer = 0;
						adrenalineTimer = 10;
						if (gameIndex <= 3) {
							gameIndex++;
							mission.text = "Get to the control module";
							backgroundSource.clip = chaseAudio;
							backgroundSource.Play ();
							//Input.ResetInputAxes ();
						} else {
							SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
							SceneManager.LoadScene ("Menu");
						}
					} 
				}
			}
			else {
				
				Vector3 direction = (jumpLocation - cam.transform.position).normalized;

				//create the rotation we need to be in to look at the target
				Quaternion lookRotation = Quaternion.LookRotation(direction);

				//rotate us over time according to speed until we are in the required rotation
				cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, lookRotation, Time.deltaTime * 10f);

			}
		} else {
			jumpscareImage.enabled = false;
		}
		if (!ventsUp || power <= 0) {
			if (power <= 0) {
				power = 0;
			}
			oxygen-=Time.deltaTime * 3;
			if (Mathf.Floor(oxygen) == 90) {
				AddMessage (new string[]{"You: The oxygen in the station is going to run out soon..."}, 4);
			}
			if (Mathf.Floor(oxygen) == 50) {
				
			}
			if (Mathf.Floor(oxygen) == 30) {
				AddMessage (new string[]{"You: If I don't get the oxygen back online quickly, I'll suffocate."}, 4);
			}
			if (Mathf.Floor(oxygen) == 10){
				AddMessage (new string[]{"You: I can't breathe..."}, 3);
			}
			if (oxygen <= 0) {
				oxygen = 0;
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
				SceneManager.LoadScene("Menu");
			}
		} else {
			if (oxygen < 100){
				oxygen++;
				if (oxygen > 100) {
					oxygen = 100;
				}
			}
		}
			
		if (adrenalineTimer > 0) {
			adrenalineTimer = Time.deltaTime > adrenalineTimer ? 0 : adrenalineTimer - Time.deltaTime;
		}
		powerfield.text = "Reserve Power: " + (power / 15f).ToString("#.#") + "%\nPower Usage: " + (usage / 15f).ToString("#.##") + "% per second\nOxygen: " + oxygen.ToString("#.#") + "%"; 
		if (gameIndex >= 1) {
			power -= usage * Time.deltaTime;

		}
		messageTimer = messageTimer < Time.deltaTime ? 0 : messageTimer - Time.deltaTime;
		delay = delay < Time.deltaTime ? 0 : delay - Time.deltaTime;
		if (((Input.GetButton("Continue") && delay == 0) || messageTimer == 0) && textfield.enabled){
			messageIndex++;
			if (messageIndex >= messages.Length) {
				textfield.enabled = false;
				if (gameIndex == 0) {
					gameIndex++;
					backgroundSource.Stop ();
					backgroundSource.clip = suspenseAudio;
					backgroundSource.Play ();
					mission.text = "Go see Commander Sheffield";
				}
			} else {
				textfield.text = messages [messageIndex];
				delay = 0.2f;
				messageTimer = messageLength;
			}
		}
	}
}
