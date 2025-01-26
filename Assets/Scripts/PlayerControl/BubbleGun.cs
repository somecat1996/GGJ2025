using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private Image breathBar;
    [SerializeField] private GameObject bubblePrefab;

    public float defaultEnergy = 5.0f;
    private float MaxEnergy()
    {
        return defaultEnergy + GameManager.Instance.energy;
    }
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
        remainEnergy = MaxEnergy();

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsRunning())
        {
            if (!blow && remainEnergy < MaxEnergy())
            {
                remainEnergy += (energyRecover + GameManager.Instance.energyRegen) * Time.deltaTime;
                if (remainEnergy >= MaxEnergy())
                {
                    remainEnergy = MaxEnergy();
                }
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.up = (mousePosition - transform.position).normalized;

            if (Input.GetKeyDown(KeyCode.Mouse0) && remainEnergy > 0)
            {
                bubble = ObjectPoolManager.instance.Get(bubblePrefab).GetComponent<Bubble>();
                blow = true;

                bubble.gameObject.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && blow)
            {
                Shoot();
            }

            Blow();
            UpdateUI();
        }
    }

    private void Blow()
    {
        if (blow)
        {
            blowTimer += Time.deltaTime * (1 + GameManager.Instance.chargingSpeed);
            remainEnergy -= Time.deltaTime * (1 + GameManager.Instance.chargingSpeed);
            bubble.transform.localScale = new Vector3(blowTimer, blowTimer, blowTimer);
            bubble.transform.position = transform.position + transform.up * (bubbleOffset + blowTimer / 2);
            if (remainEnergy <= 0)
            {
                Shoot();
            }

        }
    }

    private void UpdateUI()
    {
        breathBar.fillAmount = remainEnergy / MaxEnergy();
    }

    private void Shoot()
    {
        blow = false;
        bubble.Shoot(transform.up);
        blowTimer = 0;
    }
}
