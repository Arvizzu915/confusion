using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Ambient Sounds")]
    [SerializeField] private Vector2 timeBetweenSounds = new Vector2(8f, 20f);

    List<NoiseMaker> noiseMakers;

    private RoomDetector currentRoom;

    private Coroutine ambientRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ambientRoutine = StartCoroutine(AmbientRoutine());
    }

    public void RegisterNoiseMaker(NoiseMaker noiseMaker)
    {
        if (!noiseMakers.Contains(noiseMaker))
            noiseMakers.Add(noiseMaker);
    }

    public void UnregisterNoiseMaker(NoiseMaker noiseMaker)
    {
        noiseMakers.Remove(noiseMaker);
    }

    private IEnumerator AmbientRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenSounds.x, timeBetweenSounds.y));

            PlayRandomNoise();
        }
    }

    private void PlayRandomNoise()
    {
        if (currentRoom == null)
            return;

        IReadOnlyList<NoiseMaker> makers = currentRoom.NoiseMakers;

        if (makers.Count == 0)
            return;

        int start = Random.Range(0, makers.Count);

        for (int i = 0; i < makers.Count; i++)
        {
            int index = (start + i) % makers.Count;

            if (makers[index].CanPlay())
            {
                makers[index].MakeNoise();
                return;
            }
        }
    }

    public void SetCurrentRoom(RoomDetector room)
    {
        currentRoom = room;
    }
}