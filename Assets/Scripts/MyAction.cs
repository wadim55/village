
using UnityEngine;

public class MyAction : MonoBehaviour
{
    public void GirlOffer(int numGirl) // принято предложение квестодателя
    {
        EVENT.GetGirlOffer(numGirl); // голос анимешки
    }
    
    public void EnterTriggerGirl(int numGirl) //вход в зону триггера анимешки
    {
        EVENT.ZvonokSobitie(numGirl); // активация-деактивация мыши
    }

    public void ChangeTimeOfDay(string TimeOfDay)
    {
        EVENT.changeDayNight(TimeOfDay);
    }
}
