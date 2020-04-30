using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class veiw : MonoBehaviour
{
    public GameObject camare1;
    public GameObject camare2;



    public void change_view()
    {
        if (camare1.activeSelf)
        {
            camare1.SetActive(false);
            camare2.SetActive(true);
        }
        else
        {
            camare1.SetActive(true);
            camare2.SetActive(false);
        }
    }
}
