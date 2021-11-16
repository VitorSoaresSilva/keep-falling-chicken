using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameView : BaseView
{
    public UnityAction OnPauseClicked;
    public UnityAction OnFinishClicked;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goldText;
    public Slider SuzeteSlider;
    public Slider ZequinhaSlider;
    
    public Slider dashSlider;
    public Button buttonActiveDash;
    public Joystick joystick;

    public void PauseClick()
    {
        OnPauseClicked?.Invoke();
    }

    public void FinishClick()
    {
        OnFinishClicked?.Invoke();
    }

    public void UpdateScoreValue(int value)
    {
        scoreText.text = value.ToString();
    }
    public void UpdateGoldValue(int value)
    {
        goldText.text = value.ToString();
    }

    public void UpdateSlider(float value)
    {
        SuzeteSlider.value = value / 2;
    }

    public void ClickDash()
    {
        PowerUpsManager.instance.powerUps[(int)PowerUpTypes.dash].Use();
    } 

    

}