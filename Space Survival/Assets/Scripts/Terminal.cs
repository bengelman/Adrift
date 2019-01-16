using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Terminal : Interactable {
	public Text terminal;
	public int systemsRebooted = 0;
	public int systemsLeft = 9;
	public int commandProgress = 0;
	public float charsToShow = -1;
	public float load = 0;
	float windelay = 10;
	bool on = false;

	string[,] commands = new string[,]{
		{
			"init mainlogicmodule -o awrigley", "init mainlogicmodule.coursecalculator", "run mainlogicmodule.coursecalculator -o flightparameters.dat", "run mainlogicmodule.exec"
		}, 
		{
			"copy environmentdata.zip main/data", "unzip main/data/environmentdata.zip", "set package default", "run environmentloader -o main/data/environmentdata"
		},
		{
			"copy atmosphere.zip main/data", "unzip main/data/atmosphere.zip", "set package gas_conditions", "run environmentloader -o main/data/atmosphere"
		},
		{
			"copy fuellevels.dat main/data", "run fuelcalculator -o main/data/fuellevels.dat", "set package fuel_conditions", "copy fuel_output.dat main/missionparams"
		},
		{
			"copy fuelmixture.dat main/data", "run fuelcalculator -a mixture -o main/data/fuelmixture.dat", "set package fuel_mixture_conditions", "copy fuelmixture_output.dat main/missionparams"
		},
		{
			"init refuel", "cd main/missionparams", "set refuel -o fuelmixture_output.dat fuel_output.dat", "run refuel"
		},
		{
			"init flightpathmodule -o awrigley", "init flightpathmodule.coursecalculator", "run flightpathmodule.coursecalculator -o flightparameters.dat", "run flightpathmodule.exec"
		},
		{
			"run loadingsequence", "run prepareundock", "load flightpath.dat", "run navigation"
		}, 
		{
			"run finalizefuellevels", "runpreparecabin", "run beginundock", "run opencabin"
		}
	};
	public void OnOff(){
		if (GameStateManager.gameIndex < 4)
			return;
		on = !on;
		if (!on) {
			GameStateManager.instance.usage -= 2;
			commandProgress = 0;
			charsToShow = -1;
		} else {
			GameStateManager.instance.usage += 2;
		}
	}
	public override string Description(){
		return "Right click: enter command | Left click: turn on/off";
	}
	public override void Interact(){
		if (!on) {
			return;
		}
		if (charsToShow == -1) {
			charsToShow = commands [systemsRebooted, commandProgress].Length;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (systemsLeft <= 0) {
			windelay -= Time.deltaTime;
			GameStateManager.instance.AddMessage (new string[]{"You: The Soyuz is ready to launch."}, 10);
			if (windelay <= 0) {
				Cursor.lockState = CursorLockMode.None;
				SceneManager.LoadScene ("Credits");
			}
			return;
		}
		if (charsToShow > 0) {
			charsToShow -= Time.deltaTime * 40;
			if (charsToShow <= 0) {
				charsToShow = 0;
				load++;
			}
		}
		if (load > 0 && load < 100) {
			load += Time.deltaTime * 40;
			if (load >= 100) {
				load = 0;
				commandProgress++;
				charsToShow = -1;
				if (commandProgress >= 4) {
					commandProgress = 0;
					systemsRebooted++;
					systemsLeft--;
					if (systemsLeft <= 0) {
						on = false;

					}
				}
			}
		}
		if (on) {
			string display = "Systems online: " + systemsRebooted + "\nSystems offline: " + systemsLeft;
			for (int i = 0; i < commandProgress; i++) {
				display += "\n>" + commands [systemsRebooted, i] + "\n100%";
			}
			if (charsToShow == -1) {
				display += "\n>_";
			} else if (charsToShow == 0) {
				display += "\n>" + commands [systemsRebooted, commandProgress] + "\n" + load + "%";
			} else {
				display += "\n>" + commands [systemsRebooted, commandProgress].Substring (0, commands [systemsRebooted, commandProgress].Length - (int)charsToShow) + "_";
			}
			terminal.text = display;
		} else {
			terminal.text = null;
			commandProgress = 0;
		}

	}
}
