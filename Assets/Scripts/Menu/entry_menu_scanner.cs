using UnityEngine;

public class entry_menu_scanner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    scan menu_scan ;
    const float scan_timer = 0.4f;
    float scan_timer_counter = 0f;

    void Start()
    {
        menu_scan = GetComponent<scan>();
    }

    // Update is called once per frame
    void Update()
    {
        scan_timer_counter+=Time.deltaTime;
        if(scan_timer_counter>=scan_timer)
        {
            menu_scan.StartWave();
            scan_timer_counter=0;
        }
    }
}
