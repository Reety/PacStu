using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainSceneManager
{
    [SerializeField] private PacStudentController mcController;

    [SerializeField] private TouristController touristController;

    private BGM bgmController;

    private CherryController cherryController;

    private Collider2D pelCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public MainSceneManager(PacStudentController mc, TouristController tc)
    {
        mcController = mc;
        touristController = tc;
        bgmController = BGM.instance;
        mcController.enabled = false;
        touristController.enabled = false;
        bgmController.enabled = false;

        cherryController = GameObject.FindGameObjectWithTag("CherryController").GetComponent<CherryController>();
        pelCollider = GameObject.FindGameObjectWithTag("Interactables").GetComponent<Collider2D>();

        cherryController.enabled = false;
        pelCollider.enabled = false;
    }

    public void StartGame()
    {
        mcController.enabled = true;
        touristController.enabled = true;
        bgmController.enabled = true;
        mcController.Initialise();
        touristController.Initialise();
        bgmController.Initialise();

        cherryController.enabled = true;
        pelCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
