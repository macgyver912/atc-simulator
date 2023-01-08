using System.Collections;
using UnityEngine;
using static DrawRadarScreen;

public class Config : MonoBehaviour
{

    // Draw limits
    int _twr_range = 10;
    int _app_range = 60;
    int _rings_separation = 20;
    public static int twr_range;
    public static int app_range;
    public static int rings_separation;

    // ILS
    int _ils_range = 15;
    public static int ils_range;

    // Colors
    Color _background_color;
    Color _color_circles;
    Color _color_limit_circles;
    Color _color_grid;
    Color _color_runways;
    IconsColorEnum _defIconsColor_navaids;
    IconsColorEnum _defIconsColor_aircrafts;

    public static Color background_color;
    public static Color color_circles;
    public static Color color_limit_circles;
    public static Color color_grid;
    public static Color color_runways;
    public static IconsColorEnum defIconsColor_navaids;
    public static IconsColorEnum defIconsColor_aircrafts;

    void Awake()
    {
        // Draw limits
        twr_range = _twr_range;
        app_range = _app_range;
        rings_separation = _rings_separation;
        // ILS
        ils_range = _ils_range;
        // Colors
        background_color = _background_color;
        color_circles = _color_circles;
        color_limit_circles = _color_limit_circles;
        color_grid = _color_grid;
        color_runways = _color_runways;

        defIconsColor_navaids = _defIconsColor_navaids;
        defIconsColor_aircrafts = _defIconsColor_aircrafts;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
