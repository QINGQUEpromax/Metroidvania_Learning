using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectionHandler connectHandler;

    [Header("技能解锁")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    [Header("技能细节")]
    public Skill_DataSO skillData;
    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#9F9797";
    private Color lastColor;

  

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectionHandler>();

        UpdateIconColor(GetColorByHex(lockedColorHex));

    }

    private void Start()
    {
        if (skillData.unlockedByDefault)
            UnLock();
        
    }

    //返还技能点
    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skillTree.SpendSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(true);

        Player_SkillManager.instance.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private bool CanBeUnlocked()
    {
        if (isUnlocked || isLocked)
            return false;

        if(skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;   

        foreach(var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach(var node in conflictNodes)
        {
            if(node.isUnlocked)
                return false;
        }
        return true;
    }

    private void LockConflictNodes()
    {
        foreach(var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void LockChildNodes()
    {
        isLocked = true;

        foreach(var node in connectHandler.GetChildNodes())
        {
            node.LockChildNodes();
        }
    }

    //更新颜色
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            UnLock();
        else if(isLocked)
            ui.skillToolTip.LockedSkillEffect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;

            ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

            ToggleNodeHighlight(false);

    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f;highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;

        UpdateIconColor(colorToApply);
    }
    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color  color);

        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));

        if(isUnlocked)
            UpdateIconColor(Color.white);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
