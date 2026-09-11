using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class Settings : MonoBehaviour
{
    [HideInInspector] public static Settings instance;
    float m_frameRate;
    float m_averageRate;
    float m_updateSpeed;
    float m_averageupdateSpeed;
    int updateLoops = 0;

    [SerializeField] TextMeshProUGUI m_frameInfoTF;
    public void SetFrameInfo()
    {
        m_frameInfoTF.text = m_updateSpeed.ToString() + "ms " + m_frameRate + "FPS";
    }
    [SerializeField] TextMeshProUGUI m_averageFrameInfoTF;
    public void SetAverageFrameInfo()
    {
        m_averageFrameInfoTF.text = "AVG:" + m_averageupdateSpeed.ToString() + "ms " + m_averageRate + "FPS";
    }

    [SerializeField] Slider m_gridSizeSlider;
    [SerializeField] TextMeshProUGUI m_gridSizeTF;
    public void SetGridSize()
    {
        World.instance.SetGridSize((int)m_gridSizeSlider.value);
        m_gridSizeTF.text = m_gridSizeSlider.value.ToString();
    }

    [SerializeField] Slider m_stepIntervalSlider;
    [SerializeField] TextMeshProUGUI m_stepIntervalTF;
    [SerializeField] TextMeshProUGUI m_stepIntervalElapsedTF;
    public void SetStepInterval()
    {
        World.instance.SetIntervalSeconds(m_stepIntervalSlider.value);
        m_stepIntervalTF.text = m_stepIntervalSlider.value.ToString();

        m_stepIntervalElapsedTF.text = World.instance.GetStepElapsedSeconds().ToString();
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        updateLoops++;
        m_updateSpeed = Time.deltaTime * 1000;
        m_averageupdateSpeed = (m_averageupdateSpeed + m_updateSpeed) / (float)updateLoops;

        m_frameRate = 1f / Time.deltaTime;
        m_averageRate = (m_frameRate + m_averageRate) / (float)updateLoops;

        SetAverageFrameInfo();
        SetFrameInfo();
    }
}
