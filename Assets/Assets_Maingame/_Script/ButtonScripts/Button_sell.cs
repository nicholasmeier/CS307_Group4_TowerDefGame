using UnityEngine;
using UnityEngine.UI;

public class Button_sell : MonoBehaviour {

    public GameObject TowerPrefab;

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(clickme);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void clickme() {
        Debug.Log("Sell button clicked!!");
        TowerPrefab.GetComponent<Tower_script>().TowerSell();
    }
}
