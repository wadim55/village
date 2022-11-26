using UnityEngine;
using UnityEngine.Serialization;

public class ChangeSkyboxMaterial : MonoBehaviour
{
 
    [SerializeField] private Material materialDay;
    [SerializeField] private Material materialNight;
    [SerializeField] private Light sunLight;
    
    
    private void OnEnable()  //подписываемся на событие при включенном объекте
    {
        EVENT.changeDayNight += ChangeToMaterial;
    }

    private void OnDisable() //отписываемся от событие при отключенном объекте
    {
        EVENT.changeDayNight -= ChangeToMaterial;
    }
    
    public void ChangeToMaterial(string dayNight)
    {
        if(dayNight =="Day")
        {
            RenderSettings.skybox = materialDay;
            sunLight.color = Color.yellow;
        }
        else if (dayNight == "Night")
        {
            RenderSettings.skybox = materialNight; 
            sunLight.color = Color.blue;
        }
   
    }
    
}
