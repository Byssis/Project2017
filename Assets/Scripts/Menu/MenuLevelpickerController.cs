﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// To be attached to the panel hosting the levelpicker buttons. Class controls the paging of the levelpicker menu.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class MenuLevelpickerController : MonoBehaviour
{	public Button menuButton;
    private const string BACK_BTN_NAME = "BackBtn";
    private const string STANDARD_LEVEL_BTN_NAME = "LevelBtn";
    private const string RIGHT_SWIPE_BTN_NAME = "RightSwipeBtn";
    private const string LEFT_SWIPE_BTN_NAME = "LeftSwipeBtn";
    private int currentPage = 0;
	private Transform levelButtonCanvas;
    // Use this for initialization
    void Start()
    {
        AddSwipeEventListeners();
		levelButtonCanvas = transform.Find ("menu").transform.Find ("LevelButtons");
		Button[] buttons = levelButtonCanvas.GetComponentsInChildren<Button>();
        // stäng ned vänster swipe
        CheckToDisableOrEnableLeftSwipe();
        //hämta standard knapp för levels
        //Button levelBtn = GetStandardLevelButton();



        //hämta en lista med levels scener som finns i buildsettings
        List<SceneInfo> sceneList = SceneInfoLoader.GetSceneInfo();

        // skapa action/event för level 1 knapp
        //levelBtn.onClick.AddListener(delegate { SceneManager.LoadScene(sceneList.First().Path); });

        // generera knappar för övriga levels
        GenerateButtons(sceneList);

        CheckToDisableOrEnableRightSwipe(sceneList);
    }
    /// <summary>
    /// Creates event listners for the swipe buttons
    /// </summary>
    private void AddSwipeEventListeners()
    {
		Button[] buttons = transform.Find("menu").GetComponentsInChildren<Button>();
        Button rightSwipe = buttons.Single(x => x.name == RIGHT_SWIPE_BTN_NAME);
        rightSwipe.onClick.AddListener(delegate { this.NextPage(); });
        Button leftSwipe = buttons.Single(x => x.name == LEFT_SWIPE_BTN_NAME);
        leftSwipe.onClick.AddListener(delegate { this.PreviousPage(); });
    }

    private void GenerateButtons(List<SceneInfo> sceneList)
    {
        Text buttonText;
		RectTransform rectParent = GetComponentInParent<RectTransform> ();

        //skapa nya knappar för övriga nivåer, hoppa över första knappen som alltid skall finnas på menyn
        for (int i = ((currentPage) * 3 ); i < Mathf.Min(new int[] { (currentPage + 1) * 3, sceneList.Count }); i++){

			GameObject newButton = Instantiate(menuButton.gameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.LookRotation(menuButton.transform.forward));
			newButton.GetComponent<RectTransform> ().localScale = new Vector3 ((float)rectParent.lossyScale.x, (float)rectParent.lossyScale.y, (float)rectParent.lossyScale.z);


			newButton.transform.SetParent(levelButtonCanvas.transform);
            Button standardLevelBtn = newButton.GetComponent<Button>();
            string scenePath = sceneList[i].Path;
            //skapa event för knappklick och byta bana
            standardLevelBtn.onClick.AddListener(delegate() { SceneManager.LoadScene(scenePath); });
            buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = "Level " + (i + 1).ToString();
			newButton.SetActive (true);
		//	int starCount = getStarCount ();  IMPLEMENT GETSTARCOUNT
			int starCount = 3;
			GenerateStars(standardLevelBtn, starCount);
        }
    }



	private void GenerateStars (Button levelButton, int starCount){
		switch(starCount){
		case 1:
			LightStar (levelButton, 1);
			break;
		case 2:
			LightStar (levelButton, 2);
			break;
		case 3: 
			LightStar (levelButton, 3);
			break;
		}
	}

	private void LightStar(Button levelButton, int i) {
		for(int j=1;j<=i; j++){
			levelButton.transform.Find ("EmptyStar" + j).transform.GetChild (0).gameObject.SetActive (true);
		}
	}


    /// Get the standard level button, the levelpicker button which always exists on the menu
	private Button GetStandardLevelButton()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        //hitta standard knapp för levels
        Button levelBtn = buttons.Single(x => x.name == STANDARD_LEVEL_BTN_NAME);
		//int starCount = getStarCount ();
		int starCount = 2;
		GenerateStars (levelBtn, starCount);
        return levelBtn;
    }
		
    /// Set the leveltext on the standard level button
    private void SetLevelForStandardLevelButton()
    {
        Text buttonText;
        //hitta standard knapp för levels
		Button levelBtn = GetStandardLevelButton();
        buttonText = levelBtn.GetComponentInChildren<Text>();
        // sätt leveltext för första standard levelknapp
        buttonText.text = "Level " + (currentPage * 3 + 1).ToString();

    }

    /// <summary>
    /// Method for browsing to the next page
    /// </summary>
    private void NextPage()
    {

        currentPage++;
        UpdatePage();
    }
    /// <summary>
    /// Method for browsing to the previous page
    /// </summary>
    private void PreviousPage()
    {
        currentPage--;
        UpdatePage();
    }
    /// <summary>
    /// Updates current page on levelpicker menu
    /// </summary>
    private void UpdatePage() {
        //kontrollera om vänstra swipeknappen skall vara aktiv
        CheckToDisableOrEnableLeftSwipe();

        List<SceneInfo> sceneList = SceneInfoLoader.GetSceneInfo();
        //förstör alla knappar för levels (utom standardknapp)
        DestroyButtonClones();
        //generera övriga knappar
        GenerateButtons( sceneList);
        //kontrollera om högra swipeknappen skall vara aktiv
        CheckToDisableOrEnableRightSwipe(sceneList);
    }
    /// <summary>
    /// Destroys all levelbuttons except standard (first) level button
    /// </summary>
    private void DestroyButtonClones()
    {
		Button[] buttons = transform.Find("menu").transform.Find("LevelButtons").GetComponentsInChildren<Button>();
        // radera andra knappar
        for (int i = 0; i < buttons.Length; i++) 
			Destroy(buttons[i].gameObject);
    }

    /// Checks current page and identifies if more levels exists that are not listed, disables the right swipe if more levels do not exist
    private void CheckToDisableOrEnableRightSwipe(List<SceneInfo> sceneList)
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        Text buttonText;
        Button rightSwipe = buttons.Single(x => x.name == RIGHT_SWIPE_BTN_NAME);
        if ((currentPage + 1) * 3 > sceneList.Count)
        { // det finns inga fler scener/levels som inte har visats, höger swipeknapp skall vara avstängd
            rightSwipe.interactable = false;
            buttonText = rightSwipe.GetComponentInChildren<Text>();
            buttonText.color = Color.gray;
        }
        else { // finns fler levels/scener att visa/välja, höger swipeknapp skall vara aktiv
            rightSwipe.interactable = true;
            buttonText = rightSwipe.GetComponentInChildren<Text>();
            buttonText.color = Color.white;
        }
    }
    /// <summary>
    /// Checks if there are previous pages, if current page == 0, the function will disable left swipe
    /// </summary>
    private void CheckToDisableOrEnableLeftSwipe()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        Text buttonText;
        Button leftSwipe = buttons.Single(x => x.name == LEFT_SWIPE_BTN_NAME);
        if (currentPage > 0)
        { // vi har föregående sidor, vänsterswipe knapp skall vara aktiv
            leftSwipe.interactable = true;
            buttonText = leftSwipe.GetComponentInChildren<Text>();
            buttonText.color = Color.white;
        }
        else
        { // inga föregående sidor, vänster swipeknapp skall vara inaktiv
            leftSwipe.interactable = false;
            buttonText = leftSwipe.GetComponentInChildren<Text>();
            buttonText.color = Color.gray;
        }
    }
}