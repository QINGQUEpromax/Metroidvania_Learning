using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public static Player_SkillManager instance {  get; private set; }

    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;

            default:
                Debug.Log($"skill type {type} is not supported.");
                return null;
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
            instance = null;
    }
}
