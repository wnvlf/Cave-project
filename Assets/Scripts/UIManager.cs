using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Resource Display")]
    [SerializeField] private TextMeshProUGUI resourceText;
    
    [Header("Unit Buttons")]
    [SerializeField] private Button[] unitButtons;
    [SerializeField] private Image[] unitButtonImages;
    [SerializeField] private TextMeshProUGUI[] unitCostTexts;
    
    [Header("Expand Button")]
    [SerializeField] private Button expandButton;
    
    [Header("Unit Data")]
    [SerializeField] private UnitData[] unitDataArray;
    
    void Start()
    {
        ResourceManager.Instance.OnResourceChanged += UpdateResourceDisplay;
        UpdateResourceDisplay(ResourceManager.Instance.GetCurrentResources());
        SetupUnitButtons();
        SetupExpandButton();
    }
    
    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= UpdateResourceDisplay;
    }
    
    void SetupUnitButtons()
    {
        for (int i = 0; i < unitButtons.Length && i < unitDataArray.Length; i++)
        {
            int index = i;
            
            if (unitButtonImages[i] != null && unitDataArray[i].unitSprite != null)
                unitButtonImages[i].sprite = unitDataArray[i].unitSprite;
            
            if (unitCostTexts[i] != null)
                unitCostTexts[i].text = $"{unitDataArray[i].cost}";
            
            unitButtons[i].onClick.AddListener(() => OnUnitButtonClicked(index));
        }
    }
    
    void SetupExpandButton()
    {
        if (expandButton != null)
            expandButton.onClick.AddListener(OnExpandButtonClicked);
    }
    
    void Update()
    {
        UpdateButtonStates();
    }
    
    void UpdateButtonStates()
    {
        int currentResources = ResourceManager.Instance.GetCurrentResources();
        
        // 유닛 버튼 상태 업데이트
        for (int i = 0; i < unitButtons.Length && i < unitDataArray.Length; i++)
        {
            bool canAfford = currentResources >= unitDataArray[i].cost;
            unitButtons[i].interactable = canAfford;
            
            if (UnitPlacementManager.Instance.GetSelectedUnit() == unitDataArray[i])
                unitButtons[i].GetComponent<Image>().color = new Color(0.7f, 1f, 0.7f);  // 초록색 하이라이트
            else
                unitButtons[i].GetComponent<Image>().color = canAfford ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        }
        
        // 확장 버튼 하이라이트
        if (expandButton != null)
        {
            if (WallExpansionManager.Instance != null && WallExpansionManager.Instance.IsExpansionMode())
                expandButton.GetComponent<Image>().color = new Color(1f, 0.7f, 0.3f);  // 주황색 하이라이트
            else
                expandButton.GetComponent<Image>().color = Color.white;
        }
    }
    
    void OnUnitButtonClicked(int unitIndex)
    {
        // 확장 모드 취소
        if (WallExpansionManager.Instance != null)
            WallExpansionManager.Instance.SetExpansionMode(false);
        
        UnitPlacementManager.Instance.SelectUnit(unitIndex);
    }
    
    void OnExpandButtonClicked()
    {
        // 유닛 배치 모드 취소
        if (UnitPlacementManager.Instance != null)
            UnitPlacementManager.Instance.CancelPlacement();
        
        // 확장 모드 토글
        if (WallExpansionManager.Instance != null)
            WallExpansionManager.Instance.SetExpansionMode(!WallExpansionManager.Instance.IsExpansionMode());
    }
    
    void UpdateResourceDisplay(int resources)
    {
        resourceText.text = $"자원: {resources}";
    }
}