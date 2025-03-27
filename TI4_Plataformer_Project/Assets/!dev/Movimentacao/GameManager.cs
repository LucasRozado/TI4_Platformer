using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance => instance;

    private InputSystem_Actions actions;
    private Player.Skill skillsUnlocked;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        actions = new InputSystem_Actions();
        skillsUnlocked = Player.Skill.None;
    }

    public InputSystem_Actions Actions => actions;
    public Player.Skill SkillsUnlocked => skillsUnlocked;

    public void LockSkill(Player.Skill skill)
    => skillsUnlocked &= ~skill;
    public void UnlockSkill(Player.Skill skill)
    => skillsUnlocked |= skill;
}
