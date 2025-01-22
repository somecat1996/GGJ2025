using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private Image breathBar;
    [SerializeField] private GameObject bubblePrefab;

    public float blowEnergy = 5.0f;
    public float bubbleOffset = 0.5f;
    public float energyRecover = 0.5f;

    private float blowTimer;
    private float remainEnergy;
    private bool blow;
    private Bubble bubble;
    // Start is called before the first frame update
    void Start()
    {
        blow = false;
        blowTimer = 0;
        remainEnergy = blowEnergy;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!blow && remainEnergy < blowEnergy)
        {
            remainEnergy += energyRecover * Time.deltaTime;
            if (remainEnergy >= blowEnergy)
            {
                remainEnergy = blowEnergy;
            }
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.up = (mousePosition - transform.position).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && remainEnergy > 0)
        {
            bubble = ObjectPoolManager.instance.Get(bubblePrefab).GetComponent<Bubble>();
            blow = true;

            bubble.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Shoot();
            blow = false;
        }

        Blow();
        UpdateUI();
    }

    private void Blow()
    {
        if (blow)
        {
            blowTimer += Time.deltaTime;
            remainEnergy -= Time.deltaTime;
            if (remainEnergy <= 0)
            {
                Shoot();
                blow = false;
            }
            bubble.transform.localScale = new Vector3(blowTimer, blowTimer, blowTimer);
            bubble.transform.position = transform.position + transform.up * (bubbleOffset + blowTimer / 2);

        }
    }

    private void UpdateUI()
    {
        breathBar.fillAmount = remainEnergy / blowEnergy;
    }

    private void Shoot()
    {
        blowTimer = 0;
    }
}
