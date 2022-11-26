using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundController : MonoBehaviour
{
  [SerializeField] private AudioSource[] audio;
  [SerializeField] private AudioClip[] clip;
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
      audio[0].clip = clip[0];
      audio[0].Play();
      audio[2].enabled = true; //птички
      audio[3].enabled = true; //птички
    }
    else if (dayNight == "Night")
    {
      audio[0].clip = clip[1];
      audio[0].Play();
      audio[2].enabled = false;  //птички
      audio[3].enabled = false;  //птички
    }
    
  }

}
