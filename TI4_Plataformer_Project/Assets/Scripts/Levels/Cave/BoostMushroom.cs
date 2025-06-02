using System.Collections;
using UnityEngine;

public class BoostMushroom : Activated
{
    [Header("Boost")]

    [SerializeField, Tooltip("In meters")]
    private float defaultHeight = 6f;

    [SerializeField, Tooltip("In seconds")]
    private float defaultTimeToApex = 0.5f;

    [Header("Growth")]

    [SerializeField, Tooltip("In meters")]
    private float growth = 1f;

    [SerializeField, Tooltip("In seconds")]
    private float growthDuration = 1f;

    bool isGrown = false;

    Material material;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        transform.Translate(0, -growth, 0);
    }

    public override void Activate()
    {
        StartCoroutine(Grow_Coroutine());
    }

    private IEnumerator Grow_Coroutine()
    {
        float speed = growth / growthDuration;

        float elapsed = 0;
        while (elapsed < growthDuration)
        {
            transform.position += transform.up * (speed * Time.deltaTime);
            material.color = Color.Lerp(Color.grey, Color.white, elapsed / growthDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isGrown = true;
    }

    public void Boost(Player player)
    {
        if (!isGrown)
        {
            player.SwitchState<PlayerState_Jump>();
            return;
        }

        PlayerState_Jump state = player.GetState<PlayerState_Jump>();

        float jumpDefaultHeight = state.defaultHeight;
        float jumpDefaultTimeToApex = state.defaultTimeToApex;

        state.defaultHeight = defaultHeight;
        state.defaultTimeToApex = defaultTimeToApex;

        player.SwitchState(state);

        state.defaultHeight = jumpDefaultHeight;
        state.defaultTimeToApex = jumpDefaultTimeToApex;
    }
}
