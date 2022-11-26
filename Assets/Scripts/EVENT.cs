using System;

public static class EVENT

{
    public static Action <int> pointAction;   //активация деактивация указателя мыши
    public static Action<int> girlOffer;      //голос анимешки
    public static Action<string> changeDayNight; //смена дня и ночи
    
    public static void ZvonokSobitie(int bodyAction)  // активация мыши
    {
        pointAction?.Invoke(bodyAction);   
    }
    public static void GetGirlOffer(int bodyAction)  //голос анимешки
    {
        girlOffer?.Invoke(bodyAction);    
    }

    public static void DayNight(string dayOrNight) //смена дня и ночи 
    {
        changeDayNight?.Invoke(dayOrNight);
    }
    
}
