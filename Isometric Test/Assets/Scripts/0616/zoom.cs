using UnityEngine;
using Cinemachine;

public class zoom : MonoBehaviour
{
    //public GameObject player_follow_camera;
    //public Camera camera;
    DefaultControl defaultControl;
    public float mouseScrollY;

    private void Awake()
    {
        defaultControl = new DefaultControl();
        defaultControl.Player.MouseScrollY.performed += x => mouseScrollY = x.ReadValue<float>();
    }

    private void Update()
    {
        var camera = Camera.main;
        var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        var vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        if (mouseScrollY > 0)
        {
            Debug.Log("Scrolled Up");
            vcam.m_Lens.OrthographicSize -= 1;
            //vcam.m_Lens.OrthographicSize = 5;
        }
        
        if (mouseScrollY < 0)
        {
            Debug.Log("Scrolled Down");
            vcam.m_Lens.OrthographicSize += 1;
            //vcam.m_Lens.OrthographicSize = 10;
        }
    }

    #region - Enable / Disable -
    void OnEnable()
    {
        defaultControl.Enable();
    }

    void OnDisable()
    {
        defaultControl.Disable();
    }
    #endregion
}
