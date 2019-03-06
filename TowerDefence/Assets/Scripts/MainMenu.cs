using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string levelToLoad = "Level_1";

    public string playLevel = "Level_1";
    public string aboutToLoad = "LevelAbout";
    public string menuToLoad = "MainMenu";
    
        
        
    public SceneFader sceneFader;

	public void Play()
    {
        sceneFader.FadeTo(playLevel);
        //sceneFader.FadeTo(levelToLoad);
    }
    public void About()
    {
        sceneFader.FadeTo(aboutToLoad);
    }
    public void Menu()
    {
        sceneFader.FadeTo(menuToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
