using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameView : BaseView
{
    public UnityAction OnPauseClicked;
    public UnityAction OnPlayerLoses;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goldText;
    public Slider SuzeteSlider;
    public Slider ZequinhaSlider;
    
    public Slider dashSlider;
    public Button buttonActiveDash;
    public Joystick joystick;
    public RectTransform handleJoystick;
    public GameObject doublePointsIcon;
    public GameObject dashIcon;
    public GameObject shieldsIcon;
    public GameObject magnetIcon;
    public void PauseClick()
    {
        OnPauseClicked?.Invoke();
    }

    public void FinishClick()
    {
        OnPlayerLoses?.Invoke();
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
    public void UpdateSliderTarget(float value)
    {
        ZequinhaSlider.value += value;
    }

    public void ClickDash()
    {
        PowerUpsManager.instance.powerUps[(int)PowerUpTypes.dash].Use();
    } 

    

}