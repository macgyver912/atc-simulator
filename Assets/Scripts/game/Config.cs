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
    public Color _color_sid;
    public Color _color_star;
    public IconsColorEnum _defIconsColor_navaids;
    public IconsColorEnum _defIconsColor_aircrafts;
    public Material _custom_material;
    public Shader _object_shader;
    public Shader _line_shader;

    public static Color background_color;
    public static Color color_circles;
    public static Color color_limit_circles;
    public static Color color_grid;
    public static Color color_runways;
    public static Color color_sid;
    public static Color color_star;
    public static IconsColorEnum defIconsColor_navaids;
    public static IconsColorEnum defIconsColor_aircrafts;
    public static Material custom_material;
    public static Shader object_shader;
    public static Shader line_shader;

    [Header("Icons")]
    public Texture2D _icon_aircraft;
    public Texture2D _icon_aerodrome_civil;
    public Texture2D _icon_aerodrome_civil_no_facilities;
    public Texture2D _icon_aerodrome_gorvernment_civil;
    public Texture2D _icon_aerodrome_gorvernment;
    public Texture2D _icon_vor;
    public Texture2D _icon_vor_dme;
    public Texture2D _icon_vor_dme_rose;
    public Texture2D _icon_dme;
    public Texture2D _icon_fix_empty;
    public Texture2D _icon_fix_filled;

    public static Texture2D icon_aircraft;
    public static Texture2D icon_aerodrome_civil;
    public static Texture2D icon_aerodrome_civil_no_facilities;
    public static Texture2D icon_aerodrome_gorvernment_civil;
    public static Texture2D icon_aerodrome_gorvernment;
    public static Texture2D icon_vor;
    public static Texture2D icon_vor_dme;
    public static Texture2D icon_vor_dme_rose;
    public static Texture2D icon_dme;
    public static Texture2D icon_fix_empty;
    public static Texture2D icon_fix_filled;

    

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
        color_sid = _color_sid;
        color_star = _color_star;
        object_shader = _object_shader;
        line_shader = _line_shader;
        defIconsColor_navaids = _defIconsColor_navaids;
        defIconsColor_aircrafts = _defIconsColor_aircrafts;
        custom_material = _custom_material;

        // Icons
        icon_aircraft = _icon_aircraft;
        icon_aerodrome_civil = _icon_aerodrome_civil;
        icon_aerodrome_civil_no_facilities = _icon_aerodrome_civil_no_facilities;
        icon_aerodrome_gorvernment_civil = _icon_aerodrome_gorvernment_civil;
        icon_aerodrome_gorvernment = _icon_aerodrome_gorvernment;
        icon_vor = _icon_vor;
        icon_vor_dme = _icon_vor_dme;
        icon_vor_dme_rose = _icon_vor_dme_rose;
        icon_dme = _icon_dme;
        icon_fix_empty = _icon_fix_empty;
        icon_fix_filled = _icon_fix_filled;

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
