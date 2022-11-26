using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FXController : MonoBehaviour
{
  [SerializeField] private GameObject[] fxSunLight;
  
  private void OnEnable()  //подписываемся на событие при включенном объекте
  {
    EVENT.changeDayNight += ChangeTimeOfDay;
  }

  private void OnDisable() //отписываемся от событие при отключенном объекте
  {
    EVENT.changeDayNight -= ChangeTimeOfDay;
  }
  
  public void ChangeTimeOfDay(string dayNight)
  {
    if(dayNight =="Day")
    {
      foreach (var t in fxSunLight)
      {
        t.SetActive(true);
      }
    }
    else if (dayNight == "Night")
    {
      foreach (var t in fxSunLight)
      {
        t.SetActive(false);
      }
    }
   
  }
  
}
