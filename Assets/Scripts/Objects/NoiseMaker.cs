using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] clips;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 20f;

    [Header("Randomization")]
    [SerializeField] private Vector2 volumeRange = new Vector2(0.9f, 1f);
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    private AudioSource source;
    private float nextPlayTime;

    [SerializeField] private RoomDetector room;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        room = GetComponentInParent<RoomDetector>();

        if (room == null)
            Debug.LogError($"{name} has no RoomDetector parent.");
    }

    private void OnEnable()
    {
        room.RegisterNoiseMaker(this);
    }

    private void OnDisable()
    {
        room.UnregisterNoiseMaker(this);
    }

    public bool CanPlay()
    {
        return clips.Length > 0 &&
               Time.time >= nextPlayTime &&
               !source.isPlaying;
    }

    public void MakeNoise()
    {
        if (!CanPlay())
            return;

        nextPlayTime = Time.time + cooldown;

        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.volume = Random.Range(volumeRange.x, volumeRange.y);

        AudioClip clip = clips[Random.Range(0, clips.Length)];

        source.PlayOneShot(clip);
    }
}