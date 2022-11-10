using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // set the follow/lookAt target and sensitivity
        gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().Follow = GameObject.FindGameObjectWithTag("FollowTarget").transform;
        gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().LookAt = GameObject.FindGameObjectWithTag("FollowTarget").transform;
        gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_MaxSpeed = GameManager.instance.xSens;//GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().xSens;
        gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_MaxSpeed = GameManager.instance.ySens;//GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ySens;

        // turn on/off aim acceleration depending on settings
        if (GameManager.instance.accel)
        {
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_AccelTime = .1f;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_DecelTime = .1f;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_AccelTime = .1f;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_DecelTime = .1f;
        }
        else
        {
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_AccelTime = 0;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_DecelTime = 0;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_AccelTime = 0;
            gameObject.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_DecelTime = 0;
        }
    }
}
