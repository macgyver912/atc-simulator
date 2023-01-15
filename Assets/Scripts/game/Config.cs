using System.Collections;
using UnityEngine;
using static DrawRadarScreen;

public class Config : MonoBehaviour
{

    [Header("Radar Preferences")]
    public float _radarPeriod = 5.0f;
    public float _aircraftDataPeriod = 5.0f;
    public static float radarPeriod;
    public static float aircraftDataPeriod;

    [Header("Draw limits")]
    public int _twr_range = 10;
    public int _app_range = 60;
    public int _rings_separation = 20;  // nm 
    public static int twr_range;
    public static int app_range;
    public static int rings_separation;

    [Header("ILS")]
    public int _ils_range = 15; // nm
    public static int ils_range;

    [Header("Colors")]
    public Color _background_color;
    public Color _color_circles;
    public Color _color_limit_circles;
    public Color _color_grid;
    public Color _color_runways;
    public IconsColorEnum _defIconsColor_navaids;
    public IconsColorEnum _defIconsColor_aircrafts;
    public Shader _object_shader;    

    public static Color background_color;
    public static Color color_circles;
    public static Color color_limit_circles;
    public static Color color_grid;
    public static Color color_runways;
    public static IconsColorEnum defIconsColor_navaids;
    public static IconsColorEnum defIconsColor_aircrafts;
    public static Shader object_shader;

    [Header("Text style")]
    public GUISkin _labelStyle_navaids;
    public GUISkin _labelStyle_aircrafts;

    public static GUISkin labelStyle_navaids;
    public static GUISkin labelStyle_aircrafts;

    [Header("Scale")]
    public float _scale_vor = 1.3f;
    public float _scale_vor_rose = 1.3f;
    public float _scale_fix = 0.4f;
    public float _scale_acf = 1.4f;

    public static float scale_vor;
    public static float scale_vor_rose;
    public static float scale_fix;
    public static float scale_acf;

    void Awake()
    {
        // Radar config
        radarPeriod = _radarPeriod;
        aircraftDataPeriod = _aircraftDataPeriod;

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
        object_shader = _object_shader;
        defIconsColor_navaids = _defIconsColor_navaids;
        defIconsColor_aircrafts = _defIconsColor_aircrafts;

        // Text style
        labelStyle_navaids = _labelStyle_navaids;
        labelStyle_aircrafts = _labelStyle_aircrafts;

        // Scale
        scale_vor = _scale_vor;
        scale_vor_rose = _scale_vor_rose;
        scale_fix = _scale_fix;
        scale_acf= _scale_acf; 
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
