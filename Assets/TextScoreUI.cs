using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreUI : MonoBehaviour
{
    public static TextScoreUI Instance { get; private set; }
    const int POOL_SIZE = 3;

    private class ActiveText
    {
        public Text UIText;
        public float maxTime;
        public float timer;
        public Vector3 roadPosition;

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (timer / maxTime);
            Vector3 pos = roadPosition + new Vector3(delta, delta, 0.0f);
            pos = camera.WorldToScreenPoint(pos);
            pos.z = 0.0f;

            UIText.transform.position = pos;
        }
    }

    [SerializeField] private Text _textPrefab;
    private Camera _camera;
    private Transform _transformParent;
    private List<ActiveText> activeTextList = new List<ActiveText>();
    private Queue<Text> _textPool = new Queue<Text>();


    private void Awake() => Instance = this;

    void Start()
    {
        _camera = Camera.main;
        _transformParent = transform;

        for (int i = 0; i < POOL_SIZE; i++)
        {
            Text temp = Instantiate(_textPrefab, _transformParent);
            temp.gameObject.SetActive(false);
            _textPool.Enqueue(temp);
        }
    }

    void Update()
    {
        for (int i = 0; i < activeTextList.Count; i++)
        {
            ActiveText activeText = activeTextList[i];
            activeText.timer -= Time.deltaTime;

            if (activeText.timer <= 0.0f)
            {
                activeText.UIText.gameObject.SetActive(false);
                _textPool.Enqueue(activeText.UIText);
                activeTextList.RemoveAt(i);
                --i;
            }
            else
            {
                var color = activeText.UIText.color;
                color.a = activeText.timer / activeText.maxTime;
                activeText.UIText.color = color;

                activeText.MoveText(_camera);
            }
        }
    }

    public void AddText(int score, Vector3 roadPos)
    {
        var t = _textPool.Dequeue();
        t.text = "+" + score.ToString();
        t.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText() { maxTime = 1.0f };
        activeText.timer = activeText.maxTime;
        activeText.UIText = t;
        activeText.roadPosition = roadPos + Vector3.up;

        activeText.MoveText(_camera);
        activeTextList.Add(activeText);
    }
}
